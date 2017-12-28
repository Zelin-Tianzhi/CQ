using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Core;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class PropController : BaseController
    {
        #region 属性



        #endregion

        #region 视图



        #endregion

        #region Ajax请求

        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {

            return Content("");
        }

        #endregion
    }
}