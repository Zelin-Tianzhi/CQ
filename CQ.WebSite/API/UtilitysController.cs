using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CQ.Core;
using CQ.Plugin.Cache;

namespace CQ.WebSite.API
{
    public class UtilitysController : BaseApiController
    {

        [HttpGet]
        public IHttpActionResult GetAuthCode()
        {
            VerifyCode verifyCode = new VerifyCode();
            byte[] bytes = verifyCode.CreateImage();
            string code = verifyCode.ValidationCode;
            return Json((object)code);
        }
    }
}