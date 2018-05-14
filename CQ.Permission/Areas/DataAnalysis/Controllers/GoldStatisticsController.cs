using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Application.DataAnalusis;
using CQ.Core;

namespace CQ.Permission.Areas.DataAnalysis.Controllers
{
    public class GoldStatisticsController : BaseController
    {
        #region 属性

        private GoldApp _app = new GoldApp();

        #endregion

        #region 视图

        public ActionResult UserGameGold()
        {
            return View();
        }

        public ActionResult TaxList()
        {
            return View();
        }

        public ActionResult DayDetails()
        {
            return View();
        }

        public ActionResult GoldDetails()
        {
            return View();
        }

        public ActionResult GameGoldDetails()
        {
            return View();
        }

        public ActionResult IdenticalRound()
        {
            return View();
        }

        #endregion

        #region Ajax请求
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string begintime, string endtime, string keyword, string usertype)
        {
            var btime = string.IsNullOrEmpty(begintime) ? DateTime.Today.AddDays(-1) : begintime.ToDate();
            var etime = string.IsNullOrEmpty(endtime) ? DateTime.Today : endtime.ToDate();
            if (etime.Month != btime.Month)
            {
                return Content("暂时不支持跨月度查询。");
            }
            var list = _app.UserGoldStatis(btime, etime, keyword);
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
        public ActionResult GetTaxGridJson(Pagination pagination, string queryJson)
        {
            var list = _app.GetTaxList(pagination, queryJson);
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
        public ActionResult GetUserGameGridJson(Pagination pagination, string keyValue,string begintime, string engtime,string account)
        {
            var list = _app.GetUserGameGold(pagination, keyValue, begintime, engtime, account);
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
        public ActionResult GetGameDetailsGridJson(Pagination pagination, string keyValue, string begintime,
            string engtime, string account)
        {
            var list = _app.GetGameGoldList(pagination, keyValue, begintime, engtime, account);
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