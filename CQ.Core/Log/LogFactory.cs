/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using log4net;

namespace CQ.Core.Log
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

        public static Log GetLogger()
        {
            return new Log(LogManager.GetLogger(GetCurrentMethodFullName()));
        }

        private static string GetCurrentMethodFullName()
        {
            try
            {
                StackFrame frame;
                string className;
                int num = 2;
                StackTrace trace = new StackTrace();
                int len = trace.GetFrames().Length;
                do
                {
                    frame = trace.GetFrame(num++);
                    className = frame.GetMethod().DeclaringType.FullName;
                } while (className.EndsWith("Excption") && num < len);
                string methodName = frame.GetMethod().Name;
                return className + "." + methodName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            }
        }
    }
}
