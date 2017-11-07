using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.WebSite.Controllers
{
    public class GameController : BaseController
    {
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            return View();
        }
    }
}