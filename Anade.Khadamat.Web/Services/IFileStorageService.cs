using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Services
{
    public interface IFileStorageService
    {
        string SaveFile(IFormFile file, string subFolder, string fileNameSuffix);
        string GetFilePath(string fileName, string subFolder);
        Stream GetFileStream(string fileName, string subFolder);
        bool FileExists(string fileName, string subFolder);
        void DeleteFile(string fileName, string subFolder);
    }
}
