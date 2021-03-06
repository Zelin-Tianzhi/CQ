﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Core;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class RechargeController : BaseController
    {
        #region 属性

        private readonly RechargeOrderApp _orderApp = new RechargeOrderApp();

        #endregion

        #region 视图



        #endregion

        #region Ajax请求

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = _orderApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        #endregion

    }
}