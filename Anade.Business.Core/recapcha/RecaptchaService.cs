using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Anade.Business.Core.recapcha
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RecaptchaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> ValidateRecaptchaAsync(string recaptchaResponse)
        {
            if (string.IsNullOrEmpty(recaptchaResponse))
                return false;

            var secretKey = _configuration["Recaptcha:SecretKey"];
            var verifyUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";

            var response = await _httpClient.PostAsync(verifyUrl, null);
            var content = await response.Content.ReadAsStringAsync();
            var recaptchaResult = JsonSerializer.Deserialize<RecaptchaResponse>(content);

            return recaptchaResult?.Success ?? false;
        }
    }

}
