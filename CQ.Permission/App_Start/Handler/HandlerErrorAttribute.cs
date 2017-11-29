using System.Web.Mvc;
using CQ.Core.Log;

namespace CQ.Permission
{
    public class HandlerErrorAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            //错误日志
            WriteLog(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new ViewResult() { ViewName = "/Views/Shared/Error.cshtml"};
        }
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
            {
                return;
            }
            var log = LogFactory.GetLogger(context.Controller.ToString());
            log.Error(context.Exception);
        }
    }
}