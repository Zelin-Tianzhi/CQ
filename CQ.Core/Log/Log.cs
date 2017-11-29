/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;
using log4net;

namespace CQ.Core.Log
{
    public class Log
    {
        private ILog logger;
        public Log(ILog log)
        {
            this.logger = log;
        }
        public void Debug(object message)
        {
            this.logger.Debug(message);
        }
        public void Debug(object message,Exception ex)
        {
            this.logger.Debug(message,ex);
        }
        public void Error(object message)
        {
            this.logger.Error(message);
        }
        public void Error(object message, Exception ex)
        {
            this.logger.Error(message,ex);
        }
        public void Info(object message)
        {
            this.logger.Info(message);
        }
        public void Info(object message, Exception ex)
        {
            this.logger.Info(message,ex);
        }
        public void Warn(object message)
        {
            this.logger.Warn(message);
        }
        public void Warn(object message, Exception ex)
        {
            this.logger.Warn(message,ex);
        }
    }
}
