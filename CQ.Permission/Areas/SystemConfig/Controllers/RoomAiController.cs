using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.SystemConfig;
using CQ.Core;

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

        public ActionResult GetGridJson(Pagination pagination, string keyValue)
        {
            var data = new
            {
                rows = _robotApp.GetRoomList(pagination, keyValue),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        #endregion
    }
}