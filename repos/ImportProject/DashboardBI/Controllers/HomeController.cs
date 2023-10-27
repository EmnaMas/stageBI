using DashboardBI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashboardBI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Instancier le modèle GenderModel
            GenderModel genderModel = new GenderModel();

            // Appeler la méthode pour récupérer les données par genre
            var genderDataList = genderModel.GetEmailsByGender();

            // Convertir les données en format JSON et les stocker dans ViewData
            ViewData["GenderData"] = Newtonsoft.Json.JsonConvert.SerializeObject(genderDataList);

            return View();
        }
    }
}





