using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Anade.Khadamat.Web.Models;
using Anade.Khadamat.Web.Services;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Identity;

namespace Anade.Khadamat.Web.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        public HomeController(UserService userService, ILogger<HomeController> logger)
        {

            _logger = logger;


        }
        public  IActionResult Index()
        {
            
            return View();
        }
       

        public IActionResult Inscrire()
        {

            return View();
        }

        public IActionResult NesdaPrivacyPolicy()
        {

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
