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
    public class AreaController : BaseController
    {
        private readonly AreaApp _areaApp = new AreaApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = _areaApp.GetList();
            var treeList = data.Select(item => new TreeSelectModel
                {
                    id = item.F_Id.ToString(),
                    text = item.F_FullName,
                    parentId = item.F_ParentId.ToString()
                })
                .ToList();
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = _areaApp.GetList();
            var treeList = (from item in data
                let hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0
                select new TreeGridModel
                {
                    id = item.F_Id.ToString(),
                    text = item.F_FullName,
                    isLeaf = hasChildren,
                    parentId = item.F_ParentId.ToString(),
                    expanded = true,
                    entityJson = item.ToJson()
                }).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _areaApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(AreaEntity areaEntity, string keyValue)
        {
            _areaApp.SubmitForm(areaEntity, keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _areaApp.DeleteForm(keyValue.ToInt());
            return Success("删除成功。");
        }
    }
}