using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Business.Sms;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Anade.Khadamat.Business
{
    /// <summary>
    /// OTP SMS is sent only for email-verified inscriptions.
    /// Verify / resend never reveal whether the phone exists.
    /// </summary>
    public class PhoneOtpService
    {
        public const string GenericOtpMessage =
            "إذا كان رقم هاتفك مسجلاً ولم يُفعّل بعد، وبريدك الإلكتروني مؤكداً، فسيصلك رمز التحقق عبر رسالة قصيرة (SMS).";

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly OtpSettings _otpSettings;
        private readonly ILogger<PhoneOtpService> _logger;

        public PhoneOtpService(
            IUnitOfWork unitOfWork,
            ISmsService smsService,
            IEmailService emailService,
            IOptions<OtpSettings> otpSettings,
            ILogger<PhoneOtpService> logger)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _emailService = emailService;
            _otpSettings = otpSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Called right after successful email verification — one SMS for that inscription.
        /// </summary>
        public async Task<BusinessResult> GenerateAndSendOtpAsync(Inscription inscription)
        {
            if (inscription == null)
                return Fail("لم يتم العثور على طلب التسجيل.");

            if (!inscription.IsEmailVerified)
                return Fail("يجب تأكيد البريد الإلكتروني أولاً قبل إرسال رمز الهاتف.");

            if (string.IsNullOrWhiteSpace(inscription.NumTelephone))
                return Fail("رقم الهاتف غير متوفر في طلب التسجيل.");

            if (inscription.IsPhoneValidated)
                return Fail("تم تأكيد هذا الرقم مسبقاً.");

            return await SendOtpInternalAsync(inscription);
        }

        /// <summary>
        /// Public entry (VerifyPhone / Resend): requires both email and phone.
        /// Sends only if email + phone matches an email-verified, not-yet-validated inscription.
        /// If an active OTP is still valid, it informs the user without wasting an extra SMS.
        /// </summary>
        public async Task<BusinessResult> RequestOtpAsync(string email, string phone)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
                return Fail("يرجى إدخال البريد الإلكتروني ورقم الهاتف.");

            var target = FindEligibleInscription(email, phone);
            if (target == null)
            {
                var any = FindAnyInscription(email, phone);
                if (any != null)
                {
                    if (!any.IsEmailVerified)
                        return Fail("يجب تأكيد بريدك الإلكتروني أولاً بالضغط على الرابط المرسل إلى بريدك قبل تفعيل الهاتف.");
                    if (any.IsPhoneValidated)
                        return Fail("تم تفعيل هذا الحساب ورقم الهاتف مسبقاً. يمكنك تسجيل الدخول مباشرة.");
                }
                return Fail("لم نتمكن من العثور على تسجيل مطابق للبريد الإلكتروني ورقم الهاتف المدخلين.");
            }

            if (HasActiveOtp(target))
            {
                var activeResult = new BusinessResult(true);
                activeResult.Messages.Add(new MessageResult
                {
                    Message = "رمز التحقق لا يزال صالحاً، يرجى إدخال الرمز الذي وصلك عبر رسالة قصيرة (SMS).",
                    MessageType = MessageType.Success
                });
                return activeResult;
            }

            return await SendOtpInternalAsync(target);
        }

        public BusinessResult VerifyOtp(string email, string phone, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
                return Fail("يرجى إدخال جميع بيانات التحقق.");

            var inscription = FindEligibleInscription(email, phone);
            if (inscription == null)
            {
                var any = FindAnyInscription(email, phone);
                if (any != null && any.IsPhoneValidated)
                    return Fail("تم تأكيد رقم الهاتف مسبقاً.");
                return Fail("لم يتم العثور على طلب تسجيل مؤهل للتفعيل بالبيانات المدخلة.");
            }

            if (string.IsNullOrEmpty(inscription.OtpCode) || !inscription.OtpExpiresAt.HasValue)
                return Fail("لا يوجد رمز نشط. يرجى طلب رمز جديد.");

            if (DateTime.UtcNow > inscription.OtpExpiresAt.Value)
                return Fail("انتهت صلاحية الرمز. يرجى طلب رمز جديد.");

            if (!string.Equals(inscription.OtpCode.Trim(), code.Trim(), StringComparison.Ordinal))
                return Fail("الرمز الذي أدخلته غير صحيح.");

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            inscription.IsPhoneValidated = true;
            inscription.OtpCode = null;
            inscription.OtpExpiresAt = null;
            inscriptionRepo.Update(inscription);

            if (_unitOfWork.SaveChanges() == 0)
                return Fail("تعذّر تأكيد رقم الهاتف.");

            return BusinessResult.Success;
        }

        public Inscription GetVerifiedInscription(string email, string phone)
        {
            var normalizedPhone = NormalizeStoredPhone(phone);
            var normalizedEmail = email?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(normalizedPhone) || string.IsNullOrEmpty(normalizedEmail))
                return null;

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var candidates = inscriptionRepo.GetAllFiltered(x =>
                (x.Email == email.Trim() || x.Email.ToLower() == normalizedEmail) &&
                (x.NumTelephone == normalizedPhone || x.NumTelephone == phone.Replace(" ", "")) &&
                x.IsEmailVerified &&
                x.IsPhoneValidated);

            return candidates
                .OrderByDescending(x => x.DateInscription)
                .FirstOrDefault();
        }

        public int GetRemainingSeconds(string email, string phone)
        {
            var target = FindEligibleInscription(email, phone);
            if (target != null && target.OtpExpiresAt.HasValue && target.OtpExpiresAt.Value > DateTime.UtcNow)
            {
                return (int)Math.Max(0, Math.Ceiling((target.OtpExpiresAt.Value - DateTime.UtcNow).TotalSeconds));
            }
            return 0;
        }

        public Inscription FindEligibleInscription(string email, string phone)
        {
            var normalizedPhone = NormalizeStoredPhone(phone);
            var normalizedEmail = email?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(normalizedPhone) || string.IsNullOrEmpty(normalizedEmail))
                return null;

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var candidates = inscriptionRepo.GetAllFiltered(x =>
                (x.Email == email.Trim() || x.Email.ToLower() == normalizedEmail) &&
                (x.NumTelephone == normalizedPhone || x.NumTelephone == phone.Replace(" ", "")));

            return candidates
                .Where(x => x.IsEmailVerified && !x.IsPhoneValidated)
                .OrderByDescending(x => x.DateInscription)
                .FirstOrDefault();
        }

        public Inscription FindAnyInscription(string email, string phone)
        {
            var normalizedPhone = NormalizeStoredPhone(phone);
            var normalizedEmail = email?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(normalizedPhone) || string.IsNullOrEmpty(normalizedEmail))
                return null;

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var candidates = inscriptionRepo.GetAllFiltered(x =>
                (x.Email == email.Trim() || x.Email.ToLower() == normalizedEmail) &&
                (x.NumTelephone == normalizedPhone || x.NumTelephone == phone.Replace(" ", "")));

            return candidates
                .OrderByDescending(x => x.DateInscription)
                .FirstOrDefault();
        }

        /// <summary>
        /// Legacy helper for phone-only lookup (backward compatibility).
        /// </summary>
        public async Task<BusinessResult> RequestOtpByPhoneAsync(string phone)
        {
            var generic = GenericSuccess();
            var target = FindEligibleInscriptionByPhone(phone);
            if (target == null)
                return generic;

            if (HasActiveOtp(target))
                return generic;

            await SendOtpInternalAsync(target);
            return generic;
        }

        public BusinessResult VerifyOtpByPhone(string phone, string code)
        {
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
                return Fail("رمز التحقق غير صحيح.");

            var inscription = FindEligibleInscriptionByPhone(phone);
            if (inscription == null)
                return Fail("رمز التحقق غير صحيح أو منتهي الصلاحية.");

            if (string.IsNullOrEmpty(inscription.OtpCode) || !inscription.OtpExpiresAt.HasValue)
                return Fail("لا يوجد رمز نشط. يرجى طلب رمز جديد.");

            if (DateTime.UtcNow > inscription.OtpExpiresAt.Value)
                return Fail("انتهت صلاحية الرمز. يرجى طلب رمز جديد.");

            if (!string.Equals(inscription.OtpCode.Trim(), code.Trim(), StringComparison.Ordinal))
                return Fail("الرمز الذي أدخلته غير صحيح.");

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            inscription.IsPhoneValidated = true;
            inscription.OtpCode = null;
            inscription.OtpExpiresAt = null;
            inscriptionRepo.Update(inscription);

            if (_unitOfWork.SaveChanges() == 0)
                return Fail("تعذّر تأكيد رقم الهاتف.");

            return BusinessResult.Success;
        }

        public Inscription GetVerifiedInscriptionByPhone(string phone)
        {
            var normalized = NormalizeStoredPhone(phone);
            if (string.IsNullOrEmpty(normalized))
                return null;

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var candidates = inscriptionRepo.GetAllFiltered(x =>
                (x.NumTelephone == normalized || x.NumTelephone == phone.Replace(" ", ""))
                && x.IsEmailVerified
                && x.IsPhoneValidated);
            
            // Dernienr inscription
            return candidates
                .OrderByDescending(x => x.DateInscription)
                .FirstOrDefault();
        }

        public static string MaskPhone(string phone)
        {
            var digits = NormalizeStoredPhone(phone) ?? phone ?? "";
            if (digits.Length < 4)
                return "****";
            return new string('*', Math.Max(0, digits.Length - 4)) + digits.Substring(digits.Length - 4);
        }

        public static string NormalizeStoredPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;
            return phone.Replace(" ", "").Replace("-", "").Trim();
        }

        private async Task<BusinessResult> SendOtpInternalAsync(Inscription inscription)
        {
            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var smsRepo = _unitOfWork.GetRepository<SmsSent, int>();

            var otp = GenerateOtpCode(_otpSettings.CodeLength > 0 ? _otpSettings.CodeLength : 6);
            var validity = _otpSettings.ValidityMinutes > 0 ? _otpSettings.ValidityMinutes : 2;
            var expiresAt = DateTime.UtcNow.AddMinutes(validity);

            inscription.OtpCode = otp;
            inscription.OtpExpiresAt = expiresAt;
            inscription.IsPhoneValidated = false;
            inscriptionRepo.Update(inscription);

            var message = BuildOtpMessage(otp, validity);
            var smsLog = new SmsSent
            {
                NumTelephone = NormalizeStoredPhone(inscription.NumTelephone),
                Message = message,
                TypeSmsId = TypeSmsIds.OTP,
                InscriptionId = inscription.Id > 0 ? inscription.Id : (int?)null,
                DateEnvoi = DateTime.UtcNow,
                StatutEnvoiSmsId = StatutEnvoiSmsIds.ATT
            };
            smsRepo.Add(smsLog);

            if (_unitOfWork.SaveChanges() == 0)
                return Fail("تعذّر حفظ رمز التحقق.");

            var sendResult = await _smsService.SendSmsAsync(inscription.NumTelephone, message);

            smsLog.ReponseApi = Truncate(sendResult.RawResponse ?? sendResult.ErrorMessage, 2000);
            smsLog.DateReponse = DateTime.UtcNow;
            smsLog.StatutEnvoiSmsId = sendResult.Succeeded ? StatutEnvoiSmsIds.OK : StatutEnvoiSmsIds.ERR;
            smsRepo.Update(smsLog);
            _unitOfWork.SaveChanges();

            if (!sendResult.Succeeded)
            {
                _logger.LogWarning(
                    "OTP généré pour Inscription {InscriptionId} mais SMS en échec: {Error}",
                    inscription.Id, sendResult.ErrorMessage);
                return Fail("فشل إرسال الرسالة القصيرة. يمكنك إعادة الطلب بعد انتهاء صلاحية الرمز الحالي.");
            }

            var successResult = new BusinessResult(true);
            successResult.Messages.Add(new MessageResult
            {
                Message = $"تم إرسال رمز التحقق إلى رقم هاتفك {MaskPhone(inscription.NumTelephone)} بنجاح.",
                MessageType = MessageType.Success
            });
            return successResult;
        }

        private Inscription FindEligibleInscriptionByPhone(string phone)
        {
            var normalized = NormalizeStoredPhone(phone);
            if (string.IsNullOrEmpty(normalized))
                return null;

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var candidates = inscriptionRepo.GetAllFiltered(x =>
                x.NumTelephone == normalized || x.NumTelephone == phone.Replace(" ", ""));

            return candidates
                .Where(x => x.IsEmailVerified && !x.IsPhoneValidated)
                .OrderByDescending(x => x.DateInscription)
                .FirstOrDefault();
        }

        private static bool HasActiveOtp(Inscription inscription)
        {
            return !string.IsNullOrEmpty(inscription.OtpCode)
                && inscription.OtpExpiresAt.HasValue
                && DateTime.UtcNow <= inscription.OtpExpiresAt.Value;
        }

        private static string BuildOtpMessage(string otp, int validityMinutes)
        {
            return $"رمز التحقق الخاص بك هو {otp}. صالح لمدة {validityMinutes} دقيقة. لا تشاركه مع أحد.";
        }

        private static string GenerateOtpCode(int length)
        {
            //jsut limiting the Lenth min 10 anf max as min
            if (length < 4) length = 4;
            if (length > 10) length = 10;

            var max = (int)Math.Pow(10, length);
            var min = (int)Math.Pow(10, length - 1);
            var value = RandomNumberGenerator.GetInt32(min, max);
            return value.ToString();
        }

        private static string Truncate(string value, int max)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= max)
                return value;
            return value.Substring(0, max);
        }

        private static BusinessResult Fail(string message)
        {
            var result = new BusinessResult(false);
            result.Messages.Add(new MessageResult
            {
                Message = message,
                MessageType = MessageType.Warning
            });
            return result;
        }

        private static BusinessResult GenericSuccess()
        {
            var result = new BusinessResult(true);
            result.Messages.Add(new MessageResult
            {
                Message = GenericOtpMessage,
                MessageType = MessageType.Success
            });
            return result;
        }


        public async Task SendPhoneVerificationEmailAsync(string email)
        {
            var phoneVerificationLink = $"https://myproject.nesda.dz/Inscription/VerifyPhone?email={Uri.EscapeDataString(email ?? "")}";

            var emailBody = $@"
      <!DOCTYPE html>
      <html lang='ar' dir='rtl'>
      <head>
          <meta charset='UTF-8'>
          <meta name='viewport' content='width=device-width, initial-scale=1.0'>
          <style>
              @import url('https://fonts.googleapis.com/css2?family=Cairo:wght@300;400;500;700&display=swap');

              body {{
                  font-family: 'Cairo', Arial, sans-serif;
                  margin: 0;
                  padding: 0;
                  background-color: #f5f7fb;
                  color: #333;
                  text-align: right;
                  direction: rtl;
              }}

              .email-container {{
                  max-width: 650px;
                  margin: 30px auto;
                  background: #ffffff;
                  padding: 0;
                  border-radius: 10px;
                  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
                  overflow: hidden;
              }}

              .header {{
                  background: linear-gradient(135deg, #3a7bd5, #00d2ff);
                  padding: 30px 20px;
                  text-align: center;
              }}

              .header h1 {{
                  font-size: 24px;
                  font-weight: 300;
                  color: #ffffff;
                  margin: 0;
                  text-transform: uppercase;
                  letter-spacing: 1px;
              }}
              .header h3 {{
                  font-size: 24px;
                  font-weight: 600;
                  color: #ffffff;
                  margin: 0;
                  text-transform: uppercase;
                  letter-spacing: 1px;
              }}

              .content {{
                  padding: 30px 40px;
              }}

              .content h2 {{
                  color: #3a7bd5;
                  font-size: 20px;
                  font-weight: 500;
                  margin-top: 0;
                  margin-bottom: 20px;
                  border-bottom: 2px solid #f0f0f0;
                  padding-bottom: 10px;
              }}

              .content p {{
                  font-size: 16px;
                  line-height: 1.6;
                  color: #555;
                  margin-bottom: 15px;
              }}

              .status-line {{
                  display: inline-block;
                  color: #1a9e5c;
                  font-weight: 600;
                  font-size: 15px;
                  margin-bottom: 10px;
              }}

              .button-container {{
                  text-align: center;
                  margin: 30px 0;
              }}

              .button {{
                  display: inline-block;
                  padding: 12px 30px;
                  background-color: #3a7bd5;
                  color: #ffffff !important;
                  text-decoration: none;
                  border-radius: 50px;
                  font-weight: 500;
                  text-transform: uppercase;
                  font-size: 14px;
                  letter-spacing: 1px;
                  transition: all 0.3s ease;
                  box-shadow: 0 4px 10px rgba(58, 123, 213, 0.3);
              }}

              .button:hover {{
                  background-color: #2d62aa;
                  transform: translateY(-2px);
                  box-shadow: 0 6px 15px rgba(58, 123, 213, 0.4);
              }}

              .notice {{
                  background-color: #f8f9fa;
                  border-right: 4px solid #d9534f;
                  border-left: none;
                  padding: 15px;
                  margin: 25px 0;
                  border-radius: 5px 0 0 5px;
              }}

              .notice p {{
                  margin: 0;
                  font-size: 14px;
                  color: #666;
              }}

              .divider {{
                  height: 1px;
                  background: #eaeaea;
                  margin: 30px 0;
              }}

              .footer {{
                  background: #f8f9fa;
                  padding: 20px;
                  text-align: center;
              }}

              .footer p {{
                  margin: 5px 0;
                  font-size: 13px;
                  color: #777;
              }}

              .social-links {{
                  margin-top: 15px;
              }}

              .social-links a {{
                  display: inline-block;
                  margin: 0 8px;
                  color: #3a7bd5;
                  font-size: 16px;
                  text-decoration: none;
              }}
          </style>
      </head>
      <body dir='Rtl'>
          <div class='email-container'>
              <div class='header'>
                  <h1>الوكالة الوطنية لدعم وتنمية المقاولاتية</h1>
                  <h3>خدمة التسجيل الإلكتروني</h3>
              </div>
              <div class='content'>
                  <h2>تفعيل رقم الهاتف</h2>
                  <p>عزيزي المستخدم،</p>
                  <p>تم التحقق من بريدك الإلكتروني, تبقّت خطوة أخيرة لإتمام تسجيلك: تفعيل رقم هاتفك عبر رمز التحقق.</p>

                  <div class='button-container'>
                      <a href='{phoneVerificationLink}' class='button'>تفعيل رقم الهاتف</a>
                  </div>

                  <div class='notice'>
                      <p style='color:#d9534f;'><strong>هام:</strong> لن يُعتمد تسجيلكم ولن نتمكن من التواصل معكم دون إتمام هذه الخطوة.</p>
                  </div>

                  <div class='divider'></div>

                  <p>نشكرك على ثقتك.</p>
                  <p>فريق ناسدا</p>
              </div>
              <div class='footer'>
                  <p><strong>الوكالة الوطنية لدعم وتنمية المقاولاتية</strong></p>
                  <p>08 شارع أرزقي بن بوزيد، العناصر، الجزائر، الجزائر</p>
                  <p>هاتف: 033 033 0770 | البريد الإلكتروني: contact@nesda.dz</p>
                  <div class='social-links'>
                      <a href='https://web.facebook.com/Cc.nesda.dz'>f</a> |
                      <a href='https://www.linkedin.com/company/nesda-%D8%A7%D9%84%D9%88%D9%83%D8%A7%D9%84%D8%A9-%D8%A7%D9%84%D9%88%D8%B7%D9%86%D9%8A%D8%A9-%D9%84%D8%AF%D8%B9%D9%85-%D9%88-%D8%AA%D9%86%D9%85%D9%8A%D8%A9-%D8%A7%D9%84%D9%85%D9%82%D8%A7%D9%88%D9%84%D8%A7%D8%AA%D9%8A%D8%A9/posts/?feedView=all'>in</a>
                  </div>
              </div>
          </div>
      </body>
      </html>";

            await _emailService.SendEmailAsync(email, "تفعيل رقم الهاتف - الخطوة الأخيرة لإتمام تسجيلكم", emailBody);
        }

        public async Task SendAccountCreatedEmailAsync(string email)
        {
            var accountLink = "https://myproject.nesda.dz/EspacePromoteur/SuiviDossier";

            var emailBody = $@"
      <!DOCTYPE html>
      <html lang='ar' dir='rtl'>
      <head>
          <meta charset='UTF-8'>
          <meta name='viewport' content='width=device-width, initial-scale=1.0'>
          <style>
              @import url('https://fonts.googleapis.com/css2?family=Cairo:wght@300;400;500;700&display=swap');

              body {{
                  font-family: 'Cairo', Arial, sans-serif;
                  margin: 0;
                  padding: 0;
                  background-color: #f5f7fb;
                  color: #333;
                  text-align: right;
                  direction: rtl;
              }}

              .email-container {{
                  max-width: 650px;
                  margin: 30px auto;
                  background: #ffffff;
                  padding: 0;
                  border-radius: 10px;
                  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
                  overflow: hidden;
              }}

              .header {{
                  background: linear-gradient(135deg, #3a7bd5, #00d2ff);
                  padding: 30px 20px;
                  text-align: center;
              }}

              .header h1 {{
                  font-size: 24px;
                  font-weight: 300;
                  color: #ffffff;
                  margin: 0;
                  text-transform: uppercase;
                  letter-spacing: 1px;
              }}
              .header h3 {{
                  font-size: 24px;
                  font-weight: 600;
                  color: #ffffff;
                  margin: 0;
                  text-transform: uppercase;
                  letter-spacing: 1px;
              }}

              .content {{
                  padding: 30px 40px;
                  text-align: center;
              }}

              .success-icon {{
                  width: 64px;
                  height: 64px;
                  line-height: 64px;
                  border-radius: 50%;
                  background-color: #e9f9f0;
                  color: #1a9e5c;
                  font-size: 32px;
                  margin: 10px auto 20px;
              }}

              .content h2 {{
                  color: #1a9e5c;
                  font-size: 22px;
                  font-weight: 600;
                  margin-top: 0;
                  margin-bottom: 20px;
              }}

              .content p {{
                  font-size: 16px;
                  line-height: 1.6;
                  color: #555;
                  margin-bottom: 15px;
                  text-align: right;
              }}

              .button-container {{
                  text-align: center;
                  margin: 30px 0;
              }}

              .button {{
                  display: inline-block;
                  padding: 12px 30px;
                  background-color: #3a7bd5;
                  color: #ffffff !important;
                  text-decoration: none;
                  border-radius: 50px;
                  font-weight: 500;
                  text-transform: uppercase;
                  font-size: 14px;
                  letter-spacing: 1px;
                  transition: all 0.3s ease;
                  box-shadow: 0 4px 10px rgba(58, 123, 213, 0.3);
              }}

              .button:hover {{
                  background-color: #2d62aa;
                  transform: translateY(-2px);
                  box-shadow: 0 6px 15px rgba(58, 123, 213, 0.4);
              }}

              .notice {{
                  background-color: #f8f9fa;
                  border-right: 4px solid #3a7bd5;
                  border-left: none;
                  padding: 15px;
                  margin: 25px 0;
                  border-radius: 5px 0 0 5px;
                  text-align: right;
              }}

              .notice p {{
                  margin: 0;
                  font-size: 14px;
                  color: #666;
              }}

              .divider {{
                  height: 1px;
                  background: #eaeaea;
                  margin: 30px 0;
              }}

              .footer {{
                  background: #f8f9fa;
                  padding: 20px;
                  text-align: center;
              }}

              .footer p {{
                  margin: 5px 0;
                  font-size: 13px;
                  color: #777;
              }}

              .social-links {{
                  margin-top: 15px;
              }}

              .social-links a {{
                  display: inline-block;
                  margin: 0 8px;
                  color: #3a7bd5;
                  font-size: 16px;
                  text-decoration: none;
              }}
          </style>
      </head>
      <body dir='Rtl'>
          <div class='email-container'>
              <div class='header'>
                  <h1>الوكالة الوطنية لدعم وتنمية المقاولاتية</h1>
                  <h3>خدمة التسجيل الإلكتروني</h3>
              </div>
              <div class='content'>
                  <div class='success-icon'>✔</div>
                  <h2>تهانينا! تم إنشاء حسابكم بنجاح</h2>
                  <p>عزيزي المستخدم،</p>
                  <p>تم التحقق من بريدكم الإلكتروني ورقم هاتفكم، وأصبح تسجيلكم معتمداً بشكل نهائي.</p>

                  <div class='button-container'>
                      <a href='{accountLink}' class='button'>الدخول إلى حسابي</a>
                  </div>

                  <div class='notice'>
                      <p>سيقوم فريقنا بالتواصل معكم قريباً لدعوتكم لزيارة الوكالة ومتابعة ملفكم.</p>
                  </div>

                  <div class='divider'></div>

                  <p>نشكرك على ثقتك.</p>
                  <p>فريق ناسدا</p>
              </div>
              <div class='footer'>
                  <p><strong>الوكالة الوطنية لدعم وتنمية المقاولاتية</strong></p>
                  <p>08 شارع أرزقي بن بوزيد، العناصر، الجزائر، الجزائر</p>
                  <p>هاتف: 033 033 0770 | البريد الإلكتروني: contact@nesda.dz</p>
                  <div class='social-links'>
                      <a href='https://web.facebook.com/Cc.nesda.dz'>f</a> |
                      <a href='https://www.linkedin.com/company/nesda-%D8%A7%D9%84%D9%88%D9%83%D8%A7%D9%84%D8%A9-%D8%A7%D9%84%D9%88%D8%B7%D9%86%D9%8A%D8%A9-%D9%84%D8%AF%D8%B9%D9%85-%D9%88-%D8%AA%D9%86%D9%85%D9%8A%D8%A9-%D8%A7%D9%84%D9%85%D9%82%D8%A7%D9%88%D9%84%D8%A7%D8%AA%D9%8A%D8%A9/posts/?feedView=all'>in</a>
                  </div>
              </div>
          </div>
      </body>
      </html>";

            await _emailService.SendEmailAsync(email, "تهانينا! تم تسجيل طلبكم و إنشاء حسابكم بنجاح", emailBody);
        }

    }
}
