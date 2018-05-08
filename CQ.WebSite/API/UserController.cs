using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using CQ.Core;
using CQ.Plugin.Cache;
using CQ.Core.Security;
using System.Text;
using System.Web;
using CQ.Application.GameUsers;

namespace CQ.WebSite.API
{
    [ApiPermissionFilter]
    public class UserController : BaseApiController
    {
        #region 属性

        UsersApp userApp = new UsersApp();

        #endregion

        #region 视图



        #endregion

        #region Ajax请求处理

        public IHttpActionResult ModifyInfo(string aid, string nickname, string uuid)
        {
            

            return null;
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        [HttpGet, HttpPost]
        public IHttpActionResult MemberRegister(string username, string userpwd, string verify, string yzm, string useryz, string fw, string type, string pcode,string pid)
        {
            string telphone = null;
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

                telphone = username;
            }
            if (userApp.UserNameIsExist(username))
            {
                return Json(4);
            }

            int result = 0;
            string response = userApp.MemberRegister(username, userpwd, verify, pid, telphone);
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

        [HttpGet]
        public IHttpActionResult Register(string username, string userpwd, string pid)
        {
            

            if (userApp.UserNameIsExist(username))
            {
                return Json("-4");
            }
            dynamic result = 0;
            dynamic response = userApp.Register(username, userpwd, pid);
            if (response != "-1" && response != "-3" && response != "-999" && response != "-404")
            {
                result = $"0|{response}";
            }
            else
            {
                result = response;
            }
            return Json(result);
        }

        [HttpGet, HttpPost]
        public IHttpActionResult TouristLogin(string verify, int type=0)
        {
            string result = userApp.TouristLogin(verify, type);
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
        [HttpGet]
        public IHttpActionResult PerfectInfo(string account, string phonenum, string idcardno, string realname, string isenablephone="0")
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(phonenum) || string.IsNullOrEmpty(idcardno) || string.IsNullOrEmpty(realname))
            {
                return Json("用户信息错误。");
            }

            string accountid = userApp.GetIdByNum(account, 0);
            if (!(accountid.ToInt64() > 0))
            {
                return Json("帐号不存在。");
            }

            int rows = userApp.ModifyUserInfo(accountid, phonenum, idcardno, realname, isenablephone);
            if (rows >=1)
            {
                return Json("0");
            }
            else
            {
                return Json("-1");
            }
        }



        [HttpGet]
        public IHttpActionResult BindInfo(string pid, string account, string pwd, string nickname, string type)
        {
            string mbid = HttpUtility.UrlDecode(pid);
            if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(type))
            {
                return Json("参数错误。");
            }

            dynamic result = userApp.BindInfo(mbid, account, pwd, nickname, type);
            
            return Json(result);
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