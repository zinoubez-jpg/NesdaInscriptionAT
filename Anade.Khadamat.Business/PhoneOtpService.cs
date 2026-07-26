using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Business.Sms;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Anade.Khadamat.Business
{
    /// <summary>
    /// Orchestrates OTP generation, SMS logging, one-shot API send, verification and resend rules.
    /// </summary>
    public class PhoneOtpService
    {
        public const string GenericResendMessage =
            "Si ce numéro est enregistré et non encore validé, un SMS vous sera envoyé.";

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;
        private readonly OtpSettings _otpSettings;
        private readonly ILogger<PhoneOtpService> _logger;

        public PhoneOtpService(
            IUnitOfWork unitOfWork,
            ISmsService smsService,
            IOptions<OtpSettings> otpSettings,
            ILogger<PhoneOtpService> logger)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _otpSettings = otpSettings.Value;
            _logger = logger;
        }

        public async Task<BusinessResult> GenerateAndSendOtpAsync(Inscription inscription)
        {
            if (inscription == null)
                return Fail("Inscription introuvable.");

            if (string.IsNullOrWhiteSpace(inscription.NumTelephone))
                return Fail("Numéro de téléphone manquant.");

            if (inscription.IsPhoneValidated)
                return Fail("Ce numéro a déjà été validé.");

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
                return Fail("Impossible d'enregistrer le code OTP.");

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
                // OTP is still stored — user may retry after expiry via Resend.
                return Fail("L'envoi du SMS a échoué. Vous pourrez renvoyer un code après expiration.");
            }

            return BusinessResult.Success;
        }

        public BusinessResult VerifyOtp(int inscriptionId, string code)
        {
            if (inscriptionId <= 0 || string.IsNullOrWhiteSpace(code))
                return Fail("Code de vérification invalide.");

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();
            var inscription = inscriptionRepo.GetById(inscriptionId);

            if (inscription == null)
                return Fail("Inscription introuvable.");

            if (inscription.IsPhoneValidated)
                return Fail("Ce numéro a déjà été validé.");

            if (string.IsNullOrEmpty(inscription.OtpCode) || !inscription.OtpExpiresAt.HasValue)
                return Fail("Aucun code OTP actif. Veuillez renvoyer un nouveau code.");

            if (DateTime.UtcNow > inscription.OtpExpiresAt.Value)
                return Fail("Le code OTP a expiré. Veuillez en demander un nouveau.");

            if (!string.Equals(inscription.OtpCode.Trim(), code.Trim(), StringComparison.Ordinal))
                return Fail("Le code saisi est incorrect.");

            inscription.IsPhoneValidated = true;
            inscription.OtpCode = null;
            inscription.OtpExpiresAt = null;
            inscriptionRepo.Update(inscription);

            if (_unitOfWork.SaveChanges() == 0)
                return Fail("Impossible de valider le numéro de téléphone.");

            return BusinessResult.Success;
        }

        /// <summary>
        /// Always returns a generic success-style message (no phone/existence leak).
        /// Sends only when phone exists, not validated, and previous OTP is expired or absent.
        /// </summary>
        public async Task<BusinessResult> ResendOtpAsync(string phone)
        {
            var generic = GenericSuccess();

            var normalized = NormalizeStoredPhone(phone);
            if (string.IsNullOrEmpty(normalized))
                return generic;

            var inscriptionRepo = _unitOfWork.GetRepository<Inscription, int>();

            // Prefer the latest non-validated inscription for this phone (same spirit as email uniqueness).
            var candidates = inscriptionRepo.GetAllFiltered(
                x => x.NumTelephone == normalized || x.NumTelephone == phone.Replace(" ", ""));

            Inscription target = null;
            foreach (var item in candidates)
            {
                if (item.IsPhoneValidated)
                    continue;

                if (target == null || item.DateInscription > target.DateInscription)
                    target = item;
            }

            if (target == null)
                return generic;

            var otpStillValid = !string.IsNullOrEmpty(target.OtpCode)
                && target.OtpExpiresAt.HasValue
                && DateTime.UtcNow <= target.OtpExpiresAt.Value;

            if (otpStillValid)
                return generic;

            await GenerateAndSendOtpAsync(target);
            return generic;
        }

        public static string MaskPhone(string phone)
        {
            var digits = NormalizeStoredPhone(phone) ?? phone ?? "";
            if (digits.Length < 4)
                return "****";
            return new string('*', Math.Max(0, digits.Length - 4)) + digits.Substring(digits.Length - 4);
        }

        private static string BuildOtpMessage(string otp, int validityMinutes)
        {
            return $"NESDA: votre code de verification est {otp}. Valide {validityMinutes} min. Ne le partagez pas.";
        }

        private static string GenerateOtpCode(int length)
        {
            if (length < 4) length = 4;
            if (length > 10) length = 10;

            var max = (int)Math.Pow(10, length);
            var min = (int)Math.Pow(10, length - 1);
            var value = RandomNumberGenerator.GetInt32(min, max);
            return value.ToString();
        }

        private static string NormalizeStoredPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;
            return phone.Replace(" ", "").Replace("-", "").Trim();
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
                Message = GenericResendMessage,
                MessageType = MessageType.Success
            });
            return result;
        }
    }
}
