using CQ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CQ.WebSite.API
{
    public class BaseApiController : ApiController
    {
        // GET: BaseApi

        public Log Log
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }

    }
}