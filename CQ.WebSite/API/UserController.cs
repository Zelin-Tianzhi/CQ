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
        public IHttpActionResult MemberRegister(string username, string userpwd, string verify, string yzm, string useryz, string fw)
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
            if (userApp.UserNameIsExist(username))
            {
                return Json(4);
            }

            int result = 0;
            string respson = userApp.MemberRegister(username, userpwd, verify);
            if (respson != "-1" && respson != "-3" && respson != "-999" && respson != "-404")
            {
                result = 0;
            }
            else
            {
                result = respson.ToInt();
            }
            return Json(result);

        }
        [HttpGet, HttpPost]
        public IHttpActionResult TouristLogin(string verify)
        {
            string result = userApp.TouristLogin(verify);
            return Json(result);
        }

        #endregion

    }
}