/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;
using System.IO;
using System.Web;
using log4net;

namespace CQ.Core
{
    public class LogFactory
    {
        static LogFactory()
        {
            string logConfigPath = HttpContext.Current == null
                ? AppDomain.CurrentDomain.BaseDirectory + "Configs\\log4net.config"
                : HttpContext.Current.Server.MapPath("/Configs/log4net.config");
            FileInfo configFile = new FileInfo(logConfigPath);
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        public static Log GetLogger(Type type)
        {
            return new Log(LogManager.GetLogger(type));
        }
        public static Log GetLogger(string str)
        {
            return new Log(LogManager.GetLogger(str));
        }
    }
}
