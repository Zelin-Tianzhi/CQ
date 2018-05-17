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
        public readonly RechargeOrderApp App = new RechargeOrderApp();
        // GET: Recharge
        public ActionResult Index()
        {
            string token = Md5.md5(Guid.NewGuid().ToString(), 16);
            ViewBag.Token = token;
            return View();
        }

        public ActionResult UploadHeader()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ChongzhiYB(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            if (queryParam["userName"].IsEmpty() || queryParam["czType"].IsEmpty()|| queryParam["amount"].IsEmpty())
            {
                return Error("参数错误。");
            }
            if (queryParam["yzm"].IsEmpty() || queryParam["token"].IsEmpty())
            {
                return Error("验证码错误。");
            }
            var userName = queryParam["userName"].ToString();
            var account = App.GetIdByNum(userName, 2);
            if (account == null || account == "0")
            {
                return Error("游戏账户错误。");
            }
            var czType = queryParam["czType"].ToString();
            var yzm = queryParam["yzm"].ToString();
            var tokne = queryParam["token"].ToString();
            var amounts = queryParam["amount"].ToInt64();
            var verifyCode = Cache.Get(tokne);
            if (verifyCode.IsEmpty() || !verifyCode.Equals(Md5.md5(yzm,16)))
            {
                return Error("验证码错误。");
            }
            var result = App.SubmitEntity(userName, amounts, czType, userName);
            if (result == 0)
            {
                Cache.Remove(tokne);
                return Success("充值完成。");
            }
            return Error("系统错误，请重试。");
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
            var accountid = keyValue.ToInt64();
            string account = App.GetIdByNum(accountid+"", 2);
            string data = account;
            if (account == null || account == "0")
            {
                data = "用户不存在。";
            }
            return Content(data);
        }
    }
}