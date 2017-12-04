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
    public class ModuleButtonController : BaseController
    {
        private readonly ModuleApp _moduleApp = new ModuleApp();
        private readonly ModuleButtonApp _moduleButtonApp = new ModuleButtonApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson(string moduleId)
        {
            var data = _moduleButtonApp.GetList(moduleId.ToInt());
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleButtonEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel
                {
                    id = item.F_Id.ToString(),
                    text = item.F_FullName,
                    parentId = item.F_ParentId.ToString()
                };
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string moduleId)
        {
            var data = _moduleButtonApp.GetList(moduleId.ToInt());
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
            var data = _moduleButtonApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            _moduleButtonApp.SubmitForm(moduleButtonEntity, keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _moduleButtonApp.DeleteForm(keyValue.ToInt());
            return Success("删除成功。");
        }
        [HttpGet]
        public ActionResult CloneButton()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCloneButtonTreeJson()
        {
            var moduledata = _moduleApp.GetList().Where(t=> t.F_EnabledMark == true).ToList();
            var buttondata = _moduleButtonApp.GetList();
            var treeList = (from item in moduledata
                let hasChildren = moduledata.Count(t => t.F_ParentId == item.F_Id) != 0
                select new TreeViewModel
                {
                    id = item.F_Id.ToString(),
                    text = item.F_FullName,
                    value = item.F_EnCode,
                    parentId = item.F_ParentId.ToString(),
                    isexpand = true,
                    complete = true,
                    hasChildren = true
                }).ToList();
            foreach (ModuleButtonEntity item in buttondata)
            {
                bool hasChildren = buttondata.Count(t => t.F_ParentId == item.F_Id) != 0;
                TreeViewModel tree = new TreeViewModel
                {
                    id = "Btn_"+ item.F_Id.ToString(),
                    text = item.F_FullName,
                    value = item.F_EnCode,
                    parentId = item.F_ParentId == 0 ? item.F_ModuleId.ToString() : item.F_ParentId.ToString(),
                    isexpand = true,
                    complete = true,
                    showcheck = true,
                    hasChildren = hasChildren
                };
                if (item.F_Icon != "")
                {
                    tree.img = item.F_Icon;
                }
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitCloneButton(string moduleId, string Ids)
        {
            _moduleButtonApp.SubmitCloneButton(moduleId.ToInt(), Ids);
            return Success("克隆成功。");
        }
    }
}