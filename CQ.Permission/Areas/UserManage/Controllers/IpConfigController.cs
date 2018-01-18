using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Core;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class IpConfigController : BaseController
    {
        #region 属性

        readonly IpConfigApp _ipApp = new IpConfigApp();

        #endregion

        #region 视图



        #endregion

        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetIpGridJson(Pagination pagination, string queryJson)
        {
            var data = _ipApp.GetIpConfigList(pagination, queryJson);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitForm(string IPAddress,string IPType, string IsMac)
        {
            var result = _ipApp.SubmitIpList(IPAddress, IPType, IsMac);
            if (result > 0)
            {
                return Success("操作成功。");
            }
            return Error("操作失败，请重试。");
        }

        #endregion
    }
}