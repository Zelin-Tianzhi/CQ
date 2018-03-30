using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using CQ.Core;
using CQ.Plugin.Cache;
using CQ.Core.Security;
using System.Text;
using CQ.Application.GameUsers;

namespace CQ.WebSite.API
{
    public class UserController : BaseApiController
    {
        #region 属性

        UsersApp userApp = new UsersApp();

        #endregion

        #region 视图



        #endregion

        #region Ajax请求处理
        /// <summary>
        /// 用户注册
        /// </summary>
        [HttpGet, HttpPost]
        public IHttpActionResult MemberRegister(string username, string userpwd, string verify, string yzm, string useryz, string fw, string type, string pcode,string pid)
        {
            if (type == "1")
            {
                Regex accountRex = new Regex("^[A-Za-z0-9_][A-Za-z0-9_]*$");
                if (!accountRex.IsMatch(username))
                {
                    return Json(3);
                }
                string code = Cache.Get(fw)?.ToString();
                string yzmMd5 = Md5.md5(yzm.ToLower(), 16);
                if (code != yzmMd5)
                {
                    return Json(2);
                }
            }
            else
            {
                string msg = VerifyCode(username, pcode);
                if (msg != "0")
                {
                    return Json(9);
                }
            }
            if (userApp.UserNameIsExist(username))
            {
                return Json(4);
            }

            int result = 0;
            string response = userApp.MemberRegister(username, userpwd, verify, pid);
            if (response != "-1" && response != "-3" && response != "-999" && response != "-404")
            {
                result = 0;
            }
            else
            {
                result = response.ToInt();
            }
            return Json(result);

        }
        [HttpGet, HttpPost]
        public IHttpActionResult TouristLogin(string verify)
        {
            string result = userApp.TouristLogin(verify);
            return Json(result);
        }
        [HttpGet]
        public IHttpActionResult SendCode(string account)
        {
            if (account.IsEmpty())
            {
                return Json("账号不能为空。");
            }
            Regex regex = new Regex("^1[34578]\\d{9}$");
            if (!regex.IsMatch(account))
            {
                return Json("手机号码输入错误。");
            }
            string accountKey = account + "RegSendCodeTime";
            object sendCodeTime = Cache.Get(accountKey);
            if (sendCodeTime == null)
            {
                object timeUnix = Common.GetCurrentTimeUnix() + 60;
                Cache.Insert(accountKey, timeUnix, 1);
            }
            else
            {
                return Json("请等待一分钟再次请求。");
            }
            string url = $"http://183.131.69.236:6001/?ysfunction=send&code={account}&phonenumber={account}";
            string msg = HttpMethods.HttpGet(url);
            var result = string.Empty;
            return Json(msg);
        }

        #endregion

        #region 私有方法

        private string VerifyCode(string account, string verifycode)
        {
            string url = $"http://183.131.69.236:6001/?ysfunction=verify&phonenumber={account}&verifymask={verifycode}&code={account}";
            string msg = HttpMethods.HttpGet(url);
            return msg;
        }

        #endregion

    }
}