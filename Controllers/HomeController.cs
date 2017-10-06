using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestAppWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult IndexUploadFile()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult IndexFile()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult IndexLogin()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
