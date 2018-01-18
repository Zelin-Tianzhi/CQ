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

        public ActionResult BigQuotaMac()
        {
            return View();
        }

        public ActionResult BigQuotaForm()
        {
            return View();
        }

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
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(string IPAddress,string IPType, string IsMac)
        {
            var result = _ipApp.SubmitIpForm(IPAddress, IPType, IsMac);
            if (result > 0)
            {
                return Success("操作成功。");
            }
            return Error("操作失败，请重试。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMacJson(Pagination pagination,string queryJson)
        {
            var data = _ipApp.GetBigQuotaList(pagination, queryJson);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitMacForm(string UserAccount, string IPAddress)
        {
            var result = _ipApp.SubmitMacForm(UserAccount, IPAddress);
            if (result > 0)
            {
                return Success("操作成功。");
            }
            return Error("操作失败，请重试。");
        }
        #endregion
    }
}