using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.WebSite.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News()
        {
            return View();
        }
    }
}