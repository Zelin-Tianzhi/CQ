using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.DataAnalusis;
using CQ.Core;

namespace CQ.Permission.Areas.DataAnalysis.Controllers
{
    public class OnlineStatisticsController : BaseController
    {
        #region 属性

        OnlineUserApp _userApp = new OnlineUserApp();

        #endregion

        #region 视图

        public ActionResult RoomOnlineUser()
        {
            return View();
        }

        public ActionResult OnlineUser()
        {
            return View();
        }

        #endregion

        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetRealTimeUserJson()
        {
            var list = _userApp.GetOnlineUserCount();
            var data = new
            {
                rows = list,
                total = 1,
                page = 1,
                records = list.Count
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetRoomUserCount()
        {
            var list = _userApp.GetRoomOnlineUser();
            var data = new
            {
                rows = list,
                total = 1,
                page = 1,
                records = list.Count
            };
            return Content(data.ToJson());
        }

        #endregion
    }
}