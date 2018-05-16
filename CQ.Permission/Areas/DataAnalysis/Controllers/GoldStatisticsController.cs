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
        public ActionResult GetGridJson(string queryJson)
        {

            var queryParam = queryJson.ToJObject();
            var btime = queryParam["begintime"].IsEmpty()? DateTime.Today.AddDays(-1) : queryParam["begintime"].ToDate();
            var etime = queryParam["endtime"].IsEmpty() ? DateTime.Today.AddSeconds(-1) : queryParam["endtime"].ToDate();
            var gameid = queryParam["keyword"] + "";
            var utype = queryParam["usertype"] + "";
            var account = queryParam["account"] + "";
            if (etime.Month != btime.Month)
            {
                return Content("暂时不支持跨月度查询。");
            }

            var list = _app.UserGoldStatis(btime, etime, gameid, utype, account);
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
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetIdenticalRoundGridJson(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Error("请输入查询条件。");
            }

            var arr = keyword.Split('_');
            if (arr.Length != 3)
            {
                return Error("参数错误。");
            }
            var gameid = arr[0].ToInt64();
            var yearmonth = arr[1];
            var round = arr[2];
            var list = _app.GetIdenticalRoundGridJson(gameid, yearmonth, round);
            var data = new
            {
                rows = list,
                total = 1,
                page = 1,
                records = list.Rows.Count
            };
            return Content(data.ToJson());
        }

        #endregion


    }
}