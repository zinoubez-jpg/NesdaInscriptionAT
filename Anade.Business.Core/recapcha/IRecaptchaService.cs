using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public interface IRecaptchaService
    {
        Task<bool> ValidateRecaptchaAsync(string recaptchaResponse);
    }
}
