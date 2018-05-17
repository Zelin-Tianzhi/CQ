using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CQ.Application.BusinessData;
using CQ.Core;
using CQ.Plugin.Cache;

namespace CQ.WebSite.Controllers
{
    public class FindPasswordController : BaseController
    {
        #region 属性

        FindPasswordApp pwdApp = new FindPasswordApp();

        #endregion
        // GET: FindPassword
        public ActionResult Index()
        {
            string token = Md5.md5(Guid.NewGuid().ToString(), 16);
            ViewBag.Token = token;
            return View();
        }

        public ActionResult ModifyForm()
        {
            var superiorUrl = HttpContext.Request.UrlReferrer?.LocalPath;
            if (superiorUrl != "/FindPassword/Index" && superiorUrl != "/FindPassword" && superiorUrl != "/FindPassword/")
            {
                return Redirect("/FindPassword/Index");
            }
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
            if (account.IsEmpty() || yzm.IsEmpty())
            {
                return Error("账号验证码不能为空。");
            }
            object verifyCode = Cache.Get(token);
            if (verifyCode.IsEmpty())
            {
                return Error("验证码超时，请重新获取");
            }
            else if (Md5.md5(yzm.ToLower(), 16) != verifyCode.ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            string accountKey = account + "SendCodeTime";
            object sendCodeTime = Cache.Get(accountKey);
            if (sendCodeTime != null)
            {
                return Success("成功");
            }

            string result = pwdApp.SendCheckCode(account);
            if (result == "0")
            {
                object timeUnix = Common.GetCurrentTimeUnix() + 60;
                Cache.Insert(accountKey, timeUnix, 1);
                return Success("发送成功");
            }
            return Error(result);
        }

        public ActionResult SendCode(string account)
        {
            if (account.IsEmpty() )
            {
                return Error("账号不能为空。");
            }
            string accountKey = account + "SendCodeTime";
            object sendCodeTime = Cache.Get(accountKey);
            if (sendCodeTime == null)
            {
                object timeUnix = Common.GetCurrentTimeUnix() + 60;
                Cache.Insert(accountKey, timeUnix, 1);
            }
            else
            {
                return Error("请等待一分钟再次请求。");
            }

            string result = pwdApp.SendCheckCode(account);
            if (result == "0")
            {
                return Success("发送成功。");
            }
            return Error(result);
        }

        public ActionResult SubmitModifyPwd(string account, string newpwd, string checkcode)
        {
            if (account.IsEmpty() || newpwd.IsEmpty() || checkcode.IsEmpty())
            {
                return Error("参数错误。");
            }
            Regex rgx = new Regex("^[A-Za-z0-9_]{6,20}$");
            if (!rgx.IsMatch(newpwd))
            {
                return Error("密码不符合规则。");
            }

            string result = pwdApp.SubmitModifyPwd(account,checkcode,newpwd);
            if (result == "0")
            {
                return Success("发送成功。");
            }
            return Error(result);
        }
    }
}