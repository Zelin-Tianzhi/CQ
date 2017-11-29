using CQ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using CQ.Core.Log;

namespace CQ.WebSite.API
{
    public class BaseApiController : ApiController
    {
        // GET: BaseApi

        public Log Log => LogFactory.GetLogger(this.GetType().ToString());
    }
}