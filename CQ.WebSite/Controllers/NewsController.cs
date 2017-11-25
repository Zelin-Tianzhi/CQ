using CQ.Application.BusinessData;
using CQ.Core;
using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.WebSite.Controllers
{
    public class NewsController : BaseController
    {
        ArticleApp articleApp = new ArticleApp();
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetArticleEntitys(int keyValue = 0)
        {
            List<ArticleEntity> articleEntitys = articleApp.GetList(keyValue);
            for (int i = 0; i < articleEntitys.Count; i++)
            {
                articleEntitys[i].F_PublishTime = articleEntitys[i].F_PublishTime.ToDate();
            }
            return Content(articleEntitys.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(long keyValue)
        {
            long key = keyValue;
            var data = articleApp.GetForm(key);
            return Content(data.ToJson());
        }
    }
}