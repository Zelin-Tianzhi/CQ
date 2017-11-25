using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.WebSite.Controllers
{
    public class RechargeController : BaseController
    {
        // GET: Recharge
        public ActionResult Index()
        {
            return View();
        }

        [HandlerAjaxOnly]
        public ActionResult Chongzhi()
        {

            return Content("success");
        }
    }
}