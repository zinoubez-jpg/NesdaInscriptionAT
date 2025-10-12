using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Services
{
    // Dans les deux solutions, configurez le même chemin de stockage
    // Par exemple : D:\SecureUploads\Inscrits\ ou un réseau partagé
    public class FileStorageConfig
    {
        public string BasePath { get; set; } = @"D:\SecureUploads\Inscrits\";
        public string BaseUrl { get; set; } = "/secure-files/"; // URL pour accéder aux fichiers
    }
}
