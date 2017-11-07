using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using CQ.Core;
using CQ.Core.Security;
using CQ.Plugin.Cache;

namespace CQ.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        
    }
}