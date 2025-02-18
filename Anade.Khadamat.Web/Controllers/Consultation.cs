using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    public class Consultation : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
