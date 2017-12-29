using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Core;
using CQ.Core.Security;
using CQ.Plugin.Cache;

namespace CQ.WebSite.Controllers
{
    public class RechargeController : BaseController
    {
        GoldOperApp _goldOperApp = new GoldOperApp();
        // GET: Recharge
        public ActionResult Index()
        {
            string token = Md5.md5(Guid.NewGuid().ToString(), 16);
            ViewBag.Token = token;
            return View();
        }

        [HandlerAjaxOnly]
        public ActionResult Chongzhi()
        {

            return Content("success");
        }

        [HttpGet]
        public ActionResult GetAuthCode(string token)
        {
            VerifyCode verifyCode = new VerifyCode();
            byte[] bytes = verifyCode.CreateImage();
            string code = verifyCode.ValidationCode;
            Cache.Insert(token, (object)Md5.md5(code.ToLower(), 16), 10);
            return File(bytes, @"image/Gif");
        }

        public ActionResult CheckUser(string keyValue)
        {
            string account = _goldOperApp.GetIdByNum(keyValue, 2);
            return Content(account);
        }
    }
}