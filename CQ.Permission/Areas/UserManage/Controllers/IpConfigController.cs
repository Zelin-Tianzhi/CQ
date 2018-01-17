using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class IpConfigController : BaseController
    {
        #region 属性



        #endregion

        #region 视图



        #endregion

        #region Ajax请求

        public ActionResult GetIpGridJson(string keyword, string type)
        {
            
            return Content("");
        }

        #endregion
    }
}