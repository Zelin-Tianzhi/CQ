using System.Web;
using System.Web.Mvc;
using CQ.Core;

namespace CQ.Permission
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                //WebHelper.WriteCookie("tz_login_error", "overdue");
                //filterContext.Result = new RedirectToRouteResult("default",
                //    new System.Web.Routing.RouteValueDictionary {
                //        { "controller","Login"},
                //        { "action","Index"},
                //        { "returnurl",HttpUtility.HtmlEncode(filterContext.HttpContext.Request.RawUrl)}
                //    });
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Login/Index?returnurl=" +
                                                         HttpUtility.HtmlEncode(
                                                             filterContext.HttpContext.Request.RawUrl) + "';</script>");
            }
        }
    }
}