using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Services
{
    public interface IFileService
    {
        Task<FileStreamResult> GetCNIFileAsync(string fileName, ClaimsPrincipal user);
        Task<FileStreamResult> GetDiplomeFileAsync(string fileName, ClaimsPrincipal user);
        bool UserHasAccessToCNI(ClaimsPrincipal user);
        bool UserHasAccessToDiplome(ClaimsPrincipal user);
    }
}
