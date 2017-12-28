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
    public class TransferController : BaseController
    {
        #region 属性

        private readonly GoldOperApp _goldApp = new GoldOperApp();
        private readonly OperLogApp _operLogApp = new OperLogApp();

        #endregion

        #region 视图

        public ActionResult SysTransfer()
        {
            return View();
        }

        #endregion

        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTransferJson(Pagination pagination,string begintime, string endtime, string outaccount, string receiveaccount)
        {
            var data = new
            {
                rows = _goldApp.GetTransferList(pagination,begintime,endtime,outaccount,receiveaccount),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _operLogApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        #endregion
    }
}