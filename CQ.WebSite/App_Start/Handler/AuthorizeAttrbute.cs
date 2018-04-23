using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;

namespace CQ.WebSite
{
    public class AuthorizeAttrbute: System.Web.Http.Filters.AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var ipaddress = GetIpaddress(); //用户的ip
            object obj = null;
            obj = GetCache(ipaddress + "api"); //获取请求api的ip列表
            {
                if (obj == null)
                {
                    if (!IpConfig(ipaddress))
                    {
                        //返回401错误
                        actionContext.Response = new HttpResponseMessage
                        {
                            Content = new StringContent("当前ip地址" + ipaddress + "无访问权限",
                                Encoding.GetEncoding("UTF-8"), "application/json"),
                            StatusCode = HttpStatusCode.Unauthorized
                        };
                        return;
                    }
                    else
                    {
                        SetCache(ipaddress + "api", 1, 6);
                    }
                }
            }
            base.OnAuthorization(actionContext);
        }
        public static bool IpConfig(string ip)
        {
            string urlIndex = "~/Configs/GlobConfig.xml";
            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Configs\\GlobConfig.xml";
            //string FileName = System.Web.HttpContext.Current.Server.MapPath(urlIndex);
            XDocument doc = XDocument.Load(filePath);
            var rel = from p in doc.Descendants("item") where p.Attribute("ip").Value.ToLower() == ip select p;
            return rel != null && rel.Count() > 0 ? true : false;
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }
        /// <summary>
        ///  设置缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="expires_in"></param>
        public static void SetCache(string CacheKey, object objObject, double expires_in)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.Now.AddHours(expires_in), Cache.NoSlidingExpiration);
        }
        /// <summary>
        ///   获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpaddress()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !IsIP(result))
                return "127.0.0.1";

            return result;
        }
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }
    }
}