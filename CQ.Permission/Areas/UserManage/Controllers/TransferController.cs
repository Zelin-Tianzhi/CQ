using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Core;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class TransferController : BaseController
    {
        #region 属性

        private readonly GoldOperApp _goldApp = new GoldOperApp();

        #endregion

        #region 视图



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

        #endregion
    }
}