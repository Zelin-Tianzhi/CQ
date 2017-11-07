using System.Web.Mvc;

namespace CQ.Permission.Areas.SystemManage
{
    public class SystemManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SystemManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               this.AreaName + "_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new {area= this.AreaName, controller="Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "CQ.Permission.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}