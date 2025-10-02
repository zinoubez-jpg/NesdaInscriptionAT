using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public FileService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<FileStreamResult> GetCNIFileAsync(string fileName, ClaimsPrincipal user)
        {
            if (!UserHasAccessToCNI(user))
                throw new UnauthorizedAccessException("Accès non autorisé");

            var filePath = GetFullPath("CNIFolder", fileName);
            return await GetFileAsync(filePath, fileName);
        }

        public async Task<FileStreamResult> GetDiplomeFileAsync(string fileName, ClaimsPrincipal user)
        {
            if (!UserHasAccessToDiplome(user))
                throw new UnauthorizedAccessException("Accès non autorisé");

            var filePath = GetFullPath("DiplomeFolder", fileName);
            return await GetFileAsync(filePath, fileName);
        }

        public bool UserHasAccessToCNI(ClaimsPrincipal user)
        {
            return user.IsInRole("Admin") || user.IsInRole("HR");
        }

        public bool UserHasAccessToDiplome(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        private string GetFullPath(string folderKey, string fileName)
        {
            var folderName = _configuration[$"FileStorage:{folderKey}"];
            return Path.Combine(_environment.WebRootPath, folderName, fileName);
        }

        private async Task<FileStreamResult> GetFileAsync(string filePath, string fileName)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Fichier non trouvé");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // Retourner directement FileStreamResult au lieu d'appeler File()
            return new FileStreamResult(memory, GetContentType(filePath))
            {
                FileDownloadName = fileName
            };
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
        {
            { ".pdf", "application/pdf" },
            { ".txt", "text/plain" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" }
        };
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}
