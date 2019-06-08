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
            var ioData = JsonConvert.DeserializeObject<IoData>(data.Points);

            var result = GaService.StartGa(ioData);

            return Json(result);
        }

        [HttpPost]
        public IActionResult StartMutatingFreeText([FromBody]FreeTextData data)
        {
            var parsedData = new IoData(data.Points);

            var result = GaService.StartGa(parsedData);

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

        public IActionResult Result()
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