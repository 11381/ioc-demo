using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMvcApp.Services;

namespace SimpleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private ISimpleService service;
        public HomeController(ISimpleService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            ViewBag.Message = service.GetMessage();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = service.GetMessage();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = service.GetMessage();

            return View();
        }
    }
}