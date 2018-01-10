using System;
using CQ.Core;
using CQ.Core.Log;

namespace CQ.Application
{
    public class BaseApp
    {
        public Log Log => LogFactory.GetLogger(this.GetType().ToString());

        #region 私有方法

        protected string GetUrlStr()
        {
            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Configs\\GlobConfig.xml";
            //string filePath = System.Web.HttpContext.Current.Server.MapPath("/Configs/GlobConfig.xml");
            string url = XmlHelper.Read(filePath, "configuration/aqiuUrl", "url");
            return url;
        }

        #endregion
    }
}