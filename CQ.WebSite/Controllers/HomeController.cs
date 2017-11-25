using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using CQ.Core;
using CQ.Core.Security;
using CQ.Plugin.Cache;

namespace CQ.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAuthCode()
        {

            VerifyCode verifyCode = new VerifyCode();
            byte[] bytes = verifyCode.CreateImage();
            string code = verifyCode.ValidationCode;
            string token = Md5.md5(Guid.NewGuid().ToString(), 16);
            Log.Debug("生成token：" + token);
            Cache.Insert(token, (object)Md5.md5(code.ToLower(), 16), 10);
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(token);
            byte[] result = YSEncrypt.EncryptFishFile(bytes);

            byte[] arr = byteArray.Concat(result).ToArray();
            return File(arr, @"image/Gif");
        }
    }
}