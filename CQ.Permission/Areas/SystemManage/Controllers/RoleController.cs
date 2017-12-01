using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.SystemManage;
using CQ.Core;
using CQ.Domain.Entity.SystemManage;

namespace CQ.Permission.Areas.SystemManage.Controllers
{
    public class RoleController : BaseController
    {
        private readonly RoleApp _roleApp = new RoleApp();
        private RoleAuthorizeApp _roleAuthorizeApp = new RoleAuthorizeApp();
        private ModuleApp _moduleApp = new ModuleApp();
        private ModuleButtonApp _moduleButtonApp = new ModuleButtonApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _roleApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _roleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RoleEntity roleEntity, string permissionIds, string keyValue)
        {
            _roleApp.SubmitForm(roleEntity, permissionIds.Split(','), keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _roleApp.DeleteForm(keyValue.ToInt());
            return Success("删除成功。");
        }
    }
}