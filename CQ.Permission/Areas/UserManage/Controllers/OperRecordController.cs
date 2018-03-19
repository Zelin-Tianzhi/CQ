using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Application.SystemSecurity;
using CQ.Core;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class OperRecordController : BaseController
    {
        #region 属性

        private readonly OperLogApp _operLogApp = new OperLogApp();
        private readonly UsersApp _userApp = new UsersApp();
        private readonly OperRecordApp _recordApp = new OperRecordApp();

        #endregion


        #region 视图

        public ActionResult TUserLog()
        {
            return View();
        }

        public ActionResult Complaint()
        {
            return View();
        }

        public ActionResult Broad()
        {
            return View();
        }

        public ActionResult Extract()
        {
            return View();
        }

        public ActionResult UserGoldOper()
        {
            return View();
        }

        #endregion


        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTUserJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _operLogApp.GetList(pagination, queryJson, 2),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetComplaintJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _userApp.ComplaintRecord(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBroadJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _operLogApp.GetList(pagination, queryJson, 3),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetExtractJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _recordApp.GetExtractList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        #endregion
    }
}