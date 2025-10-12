using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Anade.Khadamat.Web.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly FileStorageConfig _config;

        public FileStorageService(IOptions<FileStorageConfig> config)
        {
            _config = config.Value;

            // Créer le répertoire s'il n'existe pas
            if (!Directory.Exists(_config.BasePath))
                Directory.CreateDirectory(_config.BasePath);
        }

        public string SaveFile(IFormFile file, string subFolder, string fileNameSuffix)
        {
            var secureFolder = Path.Combine(_config.BasePath, subFolder);
            if (!Directory.Exists(secureFolder))
                Directory.CreateDirectory(secureFolder);

            var fileName = $"{Guid.NewGuid():D}{fileNameSuffix}.pdf";
            var filePath = Path.Combine(secureFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filePath; // ou retournez seulement le nom du fichier selon vos besoins
        }

        public string GetFilePath(string fileName, string subFolder)
        {
            return Path.Combine(_config.BasePath, subFolder, fileName);
        }

        public Stream GetFileStream(string fileName, string subFolder)
        {
            var filePath = GetFilePath(fileName, subFolder);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public bool FileExists(string fileName, string subFolder)
        {
            var filePath = GetFilePath(fileName, subFolder);
            return File.Exists(filePath);
        }

        public void DeleteFile(string fileName, string subFolder)
        {
            var filePath = GetFilePath(fileName, subFolder);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
