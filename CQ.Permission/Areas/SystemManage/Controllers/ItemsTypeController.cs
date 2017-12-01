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
    public class ItemsTypeController : BaseController
    {
        private readonly ItemsApp _itemsApp = new ItemsApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = _itemsApp.GetList();
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
        public ActionResult GetTreeJson()
        {
            var data = _itemsApp.GetList();
            var treeList = (from item in data
                let hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0
                select new TreeViewModel
                {
                    id = item.F_Id.ToString(),
                    text = item.F_FullName,
                    value = item.F_EnCode,
                    parentId = item.F_ParentId.ToString(),
                    isexpand = true,
                    complete = true,
                    hasChildren = hasChildren
                }).ToList();
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson()
        {
            var data = _itemsApp.GetList();
            var treeList = (from item in data
                let hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0
                select new TreeGridModel
                {
                    id = item.F_Id.ToString(),
                    isLeaf = hasChildren,
                    parentId = item.F_ParentId.ToString(),
                    expanded = hasChildren,
                    entityJson = item.ToJson()
                }).ToList();
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _itemsApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            _itemsApp.SubmitForm(itemsEntity, keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _itemsApp.DeleteForm(keyValue.ToInt());
            return Success("删除成功。");
        }
    }
}