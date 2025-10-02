using Anade.Khadamat.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IConfiguration configuration, IFileService fileService, ILogger<FilesController> logger)
        {
            _configuration = configuration; 
            _fileService = fileService;
            _logger = logger;
        }

        [HttpGet("cni/{fileName}")]
        public async Task<IActionResult> GetCNI(string fileName)
        {
            try
            {
                var fileResult = await _fileService.GetCNIFileAsync(fileName, User);
                return fileResult; // Retourne directement le FileStreamResult
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du fichier CNI");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpGet("diplome/{fileName}")]
        public async Task<IActionResult> GetDiplome(string fileName)
        {
            try
            {
                var fileResult = await _fileService.GetDiplomeFileAsync(fileName, User);
                return fileResult; // Retourne directement le FileStreamResult
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du fichier diplôme");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpGet("internal/{type}/{fileName}")]
        public async Task<IActionResult> GetFileInternal(string type, string fileName)
        {
            try
            {
                // Vérification de la clé API
                var apiKey = Request.Headers["X-Api-Key"].FirstOrDefault();
                if (!IsValidApiKey(apiKey))
                {
                    _logger.LogWarning("Tentative d'accès avec clé API invalide: {ApiKey}", apiKey);
                    return Unauthorized("Clé API invalide");
                }

                _logger.LogInformation("Accès autorisé pour le fichier {Type}/{FileName}", type, fileName);

                if (type.ToLower() == "cni")
                {
                    var fileResult = await _fileService.GetCNIFileAsync(fileName, User);
                    return fileResult;
                }
                else if (type.ToLower() == "diplome")
                {
                    var fileResult = await _fileService.GetDiplomeFileAsync(fileName, User);
                    return fileResult;
                }
                else
                {
                    return BadRequest("Type de fichier non supporté");
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du fichier interne");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        private bool IsValidApiKey(string apiKey)
        {
            var validApiKey = _configuration["InternalApi:ApiKey"];

            // Log pour debug
            _logger.LogInformation("Clé reçue: {ReceivedKey}, Clé attendue: {ExpectedKey}",
                apiKey, validApiKey);

            return !string.IsNullOrEmpty(apiKey) && apiKey == validApiKey;
        }
    }
}
