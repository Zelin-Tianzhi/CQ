using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.SystemSecurity;
using CQ.Core;
using CQ.Domain.Entity.SystemSecurity;

namespace CQ.Permission.Areas.SystemSecurity.Controllers
{
    public class FilterIPController : BaseController
    {
        private readonly FilterIPApp _filterIpApp = new FilterIPApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _filterIpApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _filterIpApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FilterIPEntity filterIPEntity, string keyValue)
        {
            _filterIpApp.SubmitForm(filterIPEntity, keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _filterIpApp.DeleteForm(keyValue.ToInt());
            return Success("删除成功。");
        }
    }
}