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
            var verificationLink = $" http://192.168.172.106:8043/Inscription/VerifyEmail?email={email}&token={token}";
            var emailBody = $@"
            <!DOCTYPE html>
            <html lang='fr'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f4f4f4;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        text-align: center;
                        margin-bottom: 20px;
                    }}
                    .header img {{
                        max-width: 120px;
                    }}
                    .header h1 {{
                        font-size: 24px;
                        color: #333333;
                    }}
                    .content p {{
                        font-size: 16px;
                        line-height: 1.6;
                        color: #555555;
                    }}
                    .content a {{
                        display: inline-block;
                        padding: 10px 20px;
                        margin-top: 10px;
                        background-color: #007BFF;
                        color: #ffffff;
                        text-decoration: none;
                        border-radius: 4px;
                    }}
                    .content a:hover {{
                        background-color: #0056b3;
                    }}
                    .footer {{
                        margin-top: 20px;
                        text-align: center;
                        font-size: 14px;
                        color: #888888;
                    }}
                    .footer p {{
                        margin: 5px 0;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='header'>
                       
                        <h1>Bienvenue au service d'inscription de NESDA</h1>
                    </div>
                    <div class='content'>
                        <h2>Vérification de votre email</h2>
                        <p>Merci de vous être inscrit. Veuillez cliquer sur le lien ci-dessous pour vérifier votre email :</p>
                        <p><a href='{verificationLink}'>Vérifier mon email</a></p>
                        <p>Ce lien est valable pendant 24 heures.</p>
                    </div>
                    <div class='footer'>
                        <p>NESDA - Direction générale</p>
                        <p>Adresse : 08 Rue arezki ben bouzid les anasers, Alger, Algerie</p>
                        <p>Téléphone : +213 97 56 51</p>
                        <p>Email : contact@nesda.dz</p>
                    </div>
                </div>
            </body>
            </html>";


            await _emailService.SendEmailAsync(email, "Validation d'inscription", emailBody);
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
