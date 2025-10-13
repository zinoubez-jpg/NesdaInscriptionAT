using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Anade.Khadamat.Web.Services
{
    public class FileUrlService : IFileUrlService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUrlService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCNIUrl(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return string.Empty;

            var baseUrl = _configuration["AppSettings:MyProjectBaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                return string.Empty;
            }

            return $"{baseUrl}/CNIFolder/{filename}";
        }

        public string GetDiplomeUrl(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return string.Empty;

            var baseUrl = _configuration["AppSettings:MyProjectBaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                return string.Empty;
            }

            return $"{baseUrl}/DiplomeFolder/{filename}";
        }

        public string GetDiplomeCDEUrl(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return string.Empty;

            var baseUrl = _configuration["AppSettings:MyProjectBaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                return string.Empty;
            }

            return $"{baseUrl}/DiplomeCDEFolder/{filename}";
        }
    }
}