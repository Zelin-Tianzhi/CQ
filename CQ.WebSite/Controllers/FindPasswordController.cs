using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Core;
using CQ.Plugin.Cache;

namespace CQ.WebSite.Controllers
{
    public class FindPasswordController : BaseController
    {
        // GET: FindPassword
        public ActionResult Index()
        {
            string token = Md5.md5(Guid.NewGuid().ToString(), 16);
            ViewBag.Token = token;
            return View();
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

        public ActionResult SubmitAccount(string account, string yzm, string token)
        {
            var verifyCode = Cache.Get(token);
            if (verifyCode.IsEmpty())
            {
                return Error("验证码超时，请重新获取");
            }
            else if (Md5.md5(yzm.ToLower(), 16) != verifyCode.ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            return Success("");
        }
    }
}