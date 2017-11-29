using CQ.Core;
using System;
using System.Collections.Generic;
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
    }
}