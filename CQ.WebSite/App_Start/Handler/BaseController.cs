using CQ.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQ.Core.Log;

namespace CQ.WebSite
{
    public class BaseController : Controller
    {
        public Log Log
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }

        public BaseController()
        {
            ViewBag.Version = Version;
        }

        public string Version => string.IsNullOrEmpty(ConfigurationManager.AppSettings["Version"])
            ? "1.0"
            : ConfigurationManager.AppSettings["Version"].ToLower();

        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }
    }
}