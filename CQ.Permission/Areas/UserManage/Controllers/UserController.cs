using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CQ.Application;
using CQ.Application.GameUsers;
using CQ.Application.SystemSecurity;
using CQ.Core;
using CQ.Domain.Entity.SystemSecurity;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class UserController : BaseController
    {
        #region 属性

        private readonly UsersApp _userApp = new UsersApp();
        private readonly OperLogApp _operLogApp = new OperLogApp();

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

        public ActionResult SendMessage()
        {
            return View();
        }

        public ActionResult SendBroad()
        {
            return View();
        }

        #endregion

        #region Ajax请求

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
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMacGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = _userApp.GetMacList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetLgoinLogGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = _userApp.GetLoginLogList(pagination, keyword),
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
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitModifyNickName(string nickName, string keyValue)
        {
            string result = _userApp.ModifyNickName(nickName, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string keyValue)
        {
            string result = _userApp.RevisePassword(keyValue, "", "");
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitModifyJinBi(string keyValue, string keyword, string num = "0")
        {
            string result = _userApp.ModifyGold(num.ToInt(), keyValue);
            //记录操作日志
            OperLogEntity entity = new OperLogEntity
            {
                F_Account = keyValue,
                F_TextValue = num,
                F_Type = (int)OperLogType.Gold,
                F_Description = "管理员金币操作。操作值：[" + num + "]"
            };
            _operLogApp.WriteLog(entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReviseBankPassword(string keyValue)
        {
            string result = _userApp.ReviseBankPassword(keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitGetOut(string keyValue)
        {
            string result = _userApp.GetOutGame(keyValue, 0);
            //记录操作日志
            OperLogEntity entity = new OperLogEntity
            {
                F_Account = keyValue,
                F_TextValue = "0",
                F_Type = (int)OperLogType.Tuser,
                F_Description = "管理员踢人操作。"
            };
            _operLogApp.WriteLog(entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitEnabled(string keyValue)
        {
            string result = _userApp.LockUser(keyValue, 0, "");
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitDisabled(string keyValue, string hj, string message)
        {
            string result = _userApp.LockUser(keyValue, hj.ToInt64(), message);
            return Success("操作成功。");
        }

        public ActionResult SubmitRechange()
        {

            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitSendMsg(string F_Account,string F_Type, string F_Message)
        {
            if (string.IsNullOrEmpty(F_Message))
            {
                return Error("发送内容不能为空。");
            }
            if (string.IsNullOrEmpty(F_Type))
            {
                return Error("消息类型不能为空。");
            }
            _userApp.SendMessage(F_Account, F_Type, F_Message);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitSendBroad(string opendlg, string opengo, string serverid, string broadcast)
        {
            if (string.IsNullOrEmpty(broadcast))
            {
                return Error("发送内容不能为空。");
            }
            var result = _userApp.SendBroad(opendlg, opengo, serverid, broadcast);
            //记录操作日志
            OperLogEntity entity = new OperLogEntity
            {
                F_Account = serverid,
                F_TextValue = "",
                F_Type = (int)OperLogType.Broad,
                F_Description = $"发送系统广播。内容：【{broadcast}】"
            };
            _operLogApp.WriteLog(entity);
            return Success("操作成功。");
        }
        #endregion
    }
}