using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstates.Services;
using RealEstates.Web.Models;

namespace RealEstates.Web.Controllers
{
    public class HomeController : Controller
    {
        private IDistrictService districtService;

        public HomeController(IDistrictService districtService)
        {
            this.districtService = districtService;
        }

        //Methodite v controllera vryshtat specialen type IActionResult, kojto e chast ot ASP.NET Core technologiqta:
        public IActionResult Index()
        {
            var districts = this.districtService.GetTopDistrictsByAveragePrice(1000);
            return View(districts); //vrystha View-to ot papka Views, koeto se namira v papka Home i ima ime Index.cshtml!
        }

        public IActionResult Privacy()
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
