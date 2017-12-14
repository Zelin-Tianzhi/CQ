using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public ActionResult DisableUser()
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

        public ActionResult GetFormJson(string keyValue)
        {
            Regex accountRex = new Regex("^[A-Za-z0-9_][A-Za-z0-9_]*$");
            if (!accountRex.IsMatch(keyValue))
            {
                return Content((new {Result = "Error", Msg="帐号错误", data = new { }}).ToJson());
            }
            var data = _userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitModifyNickName(string nickName, string keyValue)
        {
            string result = _userApp.ModifyNickName(nickName, keyValue);
            return Success("");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string keyValue)
        {
            string result = _userApp.RevisePassword(keyValue, "", "");
            return Success("");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitModifyJinBi(string keyValue, string num = "0")
        {
            string result = _userApp.ModifyGold(num.ToInt(), keyValue);
            return Success("");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReviseBankPassword(string keyValue)
        {
            string result = _userApp.ReviseBankPassword(keyValue);
            return Success("");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitGetOut(string keyValue)
        {
            string result = _userApp.GetOutGame(keyValue, 1);
            return Success("");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitEnabled(string keyValue)
        {
            string result = _userApp.LockUser(keyValue, 0, "");
            return Success("");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitDisabled(string keyValue, string hj, string message)
        {
            string result = _userApp.LockUser(keyValue, hj.ToInt64(), message);
            return Success("");
        }
        #endregion
    }
}