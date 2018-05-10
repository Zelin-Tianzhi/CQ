using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.GameUsers;
using CQ.Application.SystemConfig;
using CQ.Core;
using CQ.Domain.Entity.QPRobot;

namespace CQ.Permission.Areas.SystemConfig.Controllers
{
    public class RobotGroupController : BaseController
    {
        #region 属性

        private readonly RobotApp _robotApp = new RobotApp();
        private readonly UsersApp _usersApp = new UsersApp();

        #endregion

        #region 视图
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult Robot()
        {
            return View();
        }

        #endregion

        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyValue)
        {
            var data = new
            {
                rows = _robotApp.GetGroupList(pagination, keyValue),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _robotApp.GetRoomAiForm(keyValue);
            return Content(data.ToJson());
        }

        
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitCreateRobot(string count)
        {
            int num = 500;
            num = count.ToInt();
            if (num <= 0) num = 500;
            string result = _usersApp.CreateRobotAccount(num);
            return Success(result + "个操作成功。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSpareCount()
        {
            int num = _robotApp.GetSpareRobotList(0)?.Rows.Count ?? 0;
            return Success(num.ToString());
        }

        public ActionResult SubmitFormGroup(string gameAi, string roomAi,string timeAi, string groupName,string num)
        {
            string rows = _robotApp.CreateGroup(gameAi, roomAi, num.ToInt(), groupName, timeAi);
            return Success("创建成功。");
        }
        #endregion
    }
}