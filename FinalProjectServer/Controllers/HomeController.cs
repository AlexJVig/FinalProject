using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinalProjectServer.Models;
using FinalProjectServer.Models.GA;
using Newtonsoft.Json;

namespace FinalProjectServer.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public IActionResult StartMutating([FromBody]FreeTextData data)
        {
            var result = GaService.StartGa(data);

            return Json(result);
        }

        [HttpPost]
        public IActionResult StartMutatingFreeText([FromBody]FreeTextData data)
        {
            var result = GaService.StartGa(data);

            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Functastic - Let us do your math!";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Functastic";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Result([FromForm]string result)
        {
            ViewData["result"] = result;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}