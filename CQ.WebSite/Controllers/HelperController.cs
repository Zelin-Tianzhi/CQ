using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.WebSite.Controllers
{
    public class HelperController : BaseController
    {
        #region 属性



        #endregion


        #region 视图

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult UserProtocol()
        {
            return View();
        }

        public ActionResult Auction()
        {
            return View();
        }

        #endregion

        #region Ajax请求

        

        #endregion
    }
}