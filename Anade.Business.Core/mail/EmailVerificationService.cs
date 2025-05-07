using Anade.Khadamat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailService _emailService;
        private readonly KhadamtDbContext _context;
        public EmailVerificationService(IEmailService emailService, KhadamtDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        public async Task SendVerificationEmailAsync(string email, string token)
        {

            var verificationLink = $"https://machrou3i.nesda.dz/Inscription/VerifyEmail?email={email}&token={token}";
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
            <body>
                <div class='email-container'>
                    <div class='header'>
                        <h1>الوكالة الوطنية لدعم وتنمية المقاولاتية</h1>
                        <h3>خدمة التسجيل الإلكتروني</h3>
                    </div>
                    <div class='content'>
                        <h2>التحقق من بريدك الإلكتروني</h2>
                        <p>عزيزي المستخدم،</p>
                        <p>نشكرك على التسجيل في خدماتنا. لإتمام عملية التسجيل وتفعيل حسابك، يرجى النقر على الزر أدناه للتحقق من عنوان بريدك الإلكتروني:</p>
            
                        <div class='button-container'>
                            <a href='{verificationLink}' class='button'>تحقق من بريدي الإلكتروني</a>
                        </div>
            
                        <div class='notice'>
                            <p style='color:red;'>رابط التحقق هذا صالح لمدة 24 ساعة. إذا لم تقم بهذا الطلب، يرجى تجاهل هذا البريد الإلكتروني.</p>
                        </div>
            
                        <p>إذا كان الزر أعلاه لا يعمل، يمكنك أيضًا نسخ ولصق الرابط التالي في متصفحك:</p>
                        <p style='font-size: 13px; word-break: break-all; color: #777;'>https://machrou3i.nesda.dz/Inscription/VerifyEmail?email={email}&token={token}</p>
            
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

            await _emailService.SendEmailAsync(email, "Verification d'email", emailBody);
        }

        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var inscrit =  _context.Inscriptions
                .FirstOrDefault(u => u.Email == email &&
                                        u.EmailVerificationToken == token &&
                                        u.EmailVerificationTokenExpiry > DateTime.UtcNow);

            if (inscrit == null) return false;

            inscrit.IsEmailVerified = true;
            inscrit.EmailVerificationToken = null;
            inscrit.EmailVerificationTokenExpiry = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
