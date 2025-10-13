using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public DocumentsController(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        // GET api/documents/{folder}/{fileName}
        [HttpGet("{folder}/{fileName}")]
        public IActionResult GetFile(string folder, string fileName, [FromHeader] string apiKey)
        {
            // 🔐 Vérification clé API
            var expectedKey = _config["Security:ApiKey"];
            if (apiKey != expectedKey)
                return Unauthorized("Clé API invalide");

            string filePath = Path.Combine(_env.ContentRootPath, "ProtectedFiles", folder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/pdf", fileName);
        }
    }
}
