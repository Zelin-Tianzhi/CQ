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
    public class RoleAuthorizeController : BaseController
    {
        private readonly RoleAuthorizeApp _roleAuthorizeApp = new RoleAuthorizeApp();
        private readonly ModuleApp _moduleApp = new ModuleApp();
        private readonly ModuleButtonApp _moduleButtonApp = new ModuleButtonApp();

        public ActionResult GetPermissionTree(string roleId)
        {
            var moduledata = _moduleApp.GetList().Where(t=> t.F_EnabledMark == true).ToList();
            var buttondata = _moduleButtonApp.GetList();
            var authorizedata = new List<RoleAuthorizeEntity>();
            if (!string.IsNullOrEmpty(roleId))
            {
                authorizedata = _roleAuthorizeApp.GetList(roleId.ToInt());
            }
            var treeList = new List<TreeViewModel>();
            foreach (ModuleEntity item in moduledata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = moduledata.Count(t => t.F_ParentId == item.F_Id) != 0;
                tree.id = item.F_Id.ToString();
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId.ToString();
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = authorizedata.Count(t => t.F_ItemId == item.F_Id);
                tree.hasChildren = true;
                tree.img = item.F_Icon == "" ? "" : item.F_Icon;
                treeList.Add(tree);
            }
            foreach (ModuleButtonEntity item in buttondata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = buttondata.Count(t => t.F_ParentId == item.F_Id) != 0;
                tree.id = "Btn_"+ item.F_Id.ToString();
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = (item.F_ParentId == 0 ? item.F_ModuleId : item.F_ParentId).ToString();
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = authorizedata.Count(t => t.F_ItemId == item.F_Id);
                tree.hasChildren = hasChildren;
                tree.img = item.F_Icon == "" ? "" : item.F_Icon;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
    }
}