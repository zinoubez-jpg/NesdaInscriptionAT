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


        public string GetCNIUrl(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return "#";

            // Extraire le nom du fichier et le sous-dossier du chemin complet
            var fileName = Path.GetFileName(filePath);
            var subFolder = Path.GetFileName(Path.GetDirectoryName(filePath));

            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}/secure-files/inscrits/{subFolder}/{fileName}";
        }

        public string GetDiplomeUrl(string filePath)
        {
            return GetCNIUrl(filePath); // Même logique
        }

        //public string GetCNIUrl(string filename)
        //{
        //    if (string.IsNullOrEmpty(filename)) return string.Empty;

        //    var baseUrl = _configuration["AppSettings:MyProjectBaseUrl"];
        //    if (string.IsNullOrEmpty(baseUrl))
        //    {
        //        return string.Empty;
        //    }

        //    return $"{baseUrl}/CNIFolder/{filename}";
        //}

        //public string GetDiplomeUrl(string filename)
        //{
        //    if (string.IsNullOrEmpty(filename)) return string.Empty;

        //    var baseUrl = _configuration["AppSettings:MyProjectBaseUrl"];
        //    if (string.IsNullOrEmpty(baseUrl))
        //    {
        //        return string.Empty;
        //    }

        //    return $"{baseUrl}/DiplomeFolder/{filename}";
        //}

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