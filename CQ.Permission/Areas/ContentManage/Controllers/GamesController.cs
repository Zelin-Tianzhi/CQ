using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.Permission.Areas.ContentManage.Controllers
{
    public class GamesController : Controller
    {
        // GET: ContentManage/Games
        public ActionResult Index()
        {
            return View();
        }
    }
}