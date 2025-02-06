using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public interface IEmailVerificationService
    {
        Task SendVerificationEmailAsync(string email, string token);
        Task<bool> VerifyEmailAsync(string email, string token);
    }
}
