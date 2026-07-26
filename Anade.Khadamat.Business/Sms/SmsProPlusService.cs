using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Anade.Khadamat.Business.Sms
{
    public class SmsProPlusService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly SmsSettings _settings;
        private readonly ILogger<SmsProPlusService> _logger;

        public SmsProPlusService(HttpClient httpClient, IOptions<SmsSettings> settings, ILogger<SmsProPlusService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<SmsSendResult> SendSmsAsync(string to, string message)
        {
            if (!_settings.Enabled)
            {
                _logger.LogWarning("SMS désactivé (SmsSettings.Enabled=false). Envoi simulé vers {Phone}.", to);
                return SmsSendResult.Success("SMS_DISABLED", 200);
            }

            if (string.IsNullOrWhiteSpace(_settings.Username) || string.IsNullOrWhiteSpace(_settings.Token))
            {
                return SmsSendResult.Failure(null, null, "Configuration SMS incomplète (Username/Token).");
            }

            var destination = NormalizeAlgerianPhone(to);
            if (string.IsNullOrEmpty(destination))
            {
                return SmsSendResult.Failure(null, null, "Numéro de téléphone invalide.");
            }

            try
            {
                var baseUrl = string.IsNullOrWhiteSpace(_settings.BaseUrl)
                    ? "https://smspro-plus.com/playsms/index.php"
                    : _settings.BaseUrl.TrimEnd('?', '&');

                var query =
                    $"app=ws&op=pv" +
                    $"&u={Uri.EscapeDataString(_settings.Username)}" +
                    $"&h={Uri.EscapeDataString(_settings.Token)}" +
                    $"&to={Uri.EscapeDataString(destination)}" +
                    $"&msg={Uri.EscapeDataString(message)}" +
                    $"&unicode=1";

                var requestUrl = $"{baseUrl}?{query}";
                using var response = await _httpClient.GetAsync(requestUrl);
                var body = await response.Content.ReadAsStringAsync();
                var statusCode = (int)response.StatusCode;

                var isSuccess = response.IsSuccessStatusCode
                    && !string.IsNullOrWhiteSpace(body)
                    && body.IndexOf("ERR", StringComparison.OrdinalIgnoreCase) < 0;

                if (isSuccess)
                {
                    _logger.LogInformation("SMS envoyé avec succès vers {Phone}. HTTP {Status}.", destination, statusCode);
                    return SmsSendResult.Success(body, statusCode);
                }

                _logger.LogWarning("Échec envoi SMS vers {Phone}. HTTP {Status}. Réponse: {Body}", destination, statusCode, body);
                return SmsSendResult.Failure(body, statusCode, "Échec de l'envoi SMS.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors de l'envoi SMS vers {Phone}.", to);
                return SmsSendResult.Failure(ex.Message, null, "Erreur technique lors de l'envoi SMS.");
            }
        }

        /// <summary>
        /// Convertit 0555... / 0 555... / 213555... en format international 213XXXXXXXXX.
        /// </summary>
        public static string NormalizeAlgerianPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            var digits = phone.Trim()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("+", "");

            if (digits.StartsWith("00213"))
                digits = digits.Substring(2);

            if (digits.StartsWith("0") && digits.Length == 10)
                digits = "213" + digits.Substring(1);

            if (digits.StartsWith("213") && digits.Length == 12)
                return digits;

            return digits;
        }
    }
}
