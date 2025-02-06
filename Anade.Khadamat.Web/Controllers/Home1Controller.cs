using Anade.Business.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    public class Home1Controller : Controller
    {
        private readonly IEmailService _emailService;

        public Home1Controller(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> SendEmail()
        {
            try
            {
                await _emailService.SendEmailAsync(
                    "hebabchakamel@gmail.com",
                    "Test Subject",
                    "<h1>This is a test email</h1><p>Hello from your Macherou3i application!</p>"
                );
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}");
            }
        }
    }
}
