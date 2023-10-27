using dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

     public IActionResult Index()
{
    // Récupérer les données du modèle GenderModel
    var genders = GenderModel.GetGenderData();

    // Passer les données à la vue
    return View(genders);
}


        public IActionResult Privacy()
        {
            return View();
        }

    
    }
}
