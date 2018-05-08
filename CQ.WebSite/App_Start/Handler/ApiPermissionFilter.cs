using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQ.Core.Log;

namespace CQ.WebSite
{
    public class ApiPermissionFilter: System.Web.Http.Filters.ActionFilterAttribute
    {
        public Log Log => LogFactory.GetLogger(this.GetType().ToString());
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string str = actionContext.Request.RequestUri.ToString();
            Log.Debug(HttpUtility.UrlDecode(str));
        }
    }
}