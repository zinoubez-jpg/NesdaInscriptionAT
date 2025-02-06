using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anade.Khadamat.Web.Models;
using Anade.Khadamat.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Anade.Khadamat.Web.Controllers
{
    public class EmbedInfoController : Controller
    {

        private readonly IOptions<ConfigurationModel> appSettings;

        public EmbedInfoController(IOptions<ConfigurationModel> appSettings)
        {
            this.appSettings = appSettings;
        }

        /// <summary>
        /// Returns Embed token, Embed URL, and Embed token expiry to the client
        /// </summary>
        /// <returns>JSON containing parameters for embedding</returns>
        [HttpGet]
        public string GetEmbedInfo()
        {
            try
            {
                string configValidationResult = ConfigValidatorService.ValidateConfig(appSettings);
                if (configValidationResult != null)
                {
                    HttpContext.Response.StatusCode = 400;
                    return configValidationResult;
                }
                string accessToken = AadService.GetAccessToken(appSettings);
                string embedInfo = PbiEmbedService.GetEmbedParam(accessToken, appSettings);
                return embedInfo;
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;
                return ex.Message + "\n\n" + ex.StackTrace;
            }
        }
    }

}

