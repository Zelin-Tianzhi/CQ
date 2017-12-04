﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Core;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class UserController : BaseController
    {
        #region 属性

        private readonly UsersApp _userApp = new UsersApp();

        #endregion

        #region 视图

        public ActionResult ModifyNickName()
        {
            return View();
        }

        public ActionResult RevisePassword()
        {
            return View();
        }

        public ActionResult ModifyUserGold()
        {
            return View();
        }

        #endregion

        #region Ajac请求

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = _userApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        public ActionResult SubmitModifyNickName(string nickname, string keyValue)
        {

            return Success("");
        }

        #endregion
    }
}