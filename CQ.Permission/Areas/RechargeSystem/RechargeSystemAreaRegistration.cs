using System.Web.Mvc;

namespace CQ.Permission.Areas.RechargeSystem
{
    public class RechargeSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RechargeSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RechargeSystem_default",
                "RechargeSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}