using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.SystemConfig;
using CQ.Core;
using CQ.Domain.Entity.QPRobot;

namespace CQ.Permission.Areas.SystemConfig.Controllers
{
    public class RoomAiController : BaseController
    {
        #region 属性

        private readonly RobotApp _robotApp = new RobotApp();

        #endregion

        #region 视图



        #endregion

        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyValue)
        {
            var data = new
            {
                rows = _robotApp.GetRoomAiList(pagination, keyValue),
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
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RobotRoomAI robotRoom, string keyValue)
        {
            _robotApp.SubmitRoomAiForm(robotRoom, keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _robotApp.DeleteRoomAiForm(keyValue.ToInt());
            return Success("删除成功。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetRoomAiList()
        {
            var data = _robotApp.GetRoomAiList();
            return Content(data.ToJson());
        }
        #endregion
    }
}