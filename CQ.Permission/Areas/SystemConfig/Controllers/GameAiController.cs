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
    public class GameAiController : BaseController
    {
        #region 属性

        private readonly RobotApp _robotApp = new RobotApp();
        private readonly GameApp _gameApp = new GameApp();

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
                rows = _robotApp.GetGameAiList(pagination, keyValue),
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
            var data = _robotApp.GetGameAiForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RobotGameAI robotGame, string keyValue)
        {
            _robotApp.SubmitGameAiForm(robotGame, keyValue.ToInt());
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _robotApp.DeleteGameAiForm(keyValue.ToInt());
            return Success("删除成功。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGameList()
        {
            var data = _gameApp.GetGameList();
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetRoomList(string game)
        {
            var data = _gameApp.GetRoomList(game);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGameAiList()
        {
            var data = _robotApp.GetGameAiList();
            return Content(data.ToJson());
        }
        #endregion
    }
}