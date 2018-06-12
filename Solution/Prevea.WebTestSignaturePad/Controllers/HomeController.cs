using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prevea.WebTestSignaturePad.Models;

namespace Prevea.WebTestSignaturePad.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View(new MyDummyModel());
        }

        [HttpPost]
        public ActionResult Contact(MyDummyModel signaturePad)
        {
            return View(signaturePad);
        }
    }
}