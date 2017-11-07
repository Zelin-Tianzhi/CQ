using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.Permission.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}