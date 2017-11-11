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
        public ActionResult GetGameEntitys()
        {
            List<ProductEntity> productEntitys = new List<ProductEntity>();
            productEntitys = productApp.GetList();
            for (int i = 0; i < productEntitys.Count; i++)
            {
                productEntitys[i].F_Id = DESEncrypt.Encrypt(productEntitys[i].F_Id.ToLower(), SysConstant.DES_KEY).ToLower();
            }
            return Content(productEntitys.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            string key = DESEncrypt.Decrypt(keyValue, SysConstant.DES_KEY).ToLower();
            var data = productApp.GetForm(key);
            return Content(data.ToJson());
        }
    }
}