using System;

namespace Anade.Khadamat.Web.Services
{
    public interface IFileUrlService
    {
        string GetCNIUrl(string filename);
        string GetDiplomeUrl(string filename);
        string GetDiplomeCDEUrl(string filename);
    }
}
