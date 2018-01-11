using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Domain.Entity.BusinessData;
using CQ.Application.BusinessData;
using CQ.Core;

namespace CQ.WebSite.Controllers
{
    public class GameController : BaseController
    {
        ProductApp productApp = new ProductApp();

        // GET: Game
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetListJson()
        {
            var productEntitys = productApp.GetList();
            return Content(productEntitys.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = productApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
    }
}