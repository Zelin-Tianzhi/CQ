using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using CQ.Core;

namespace CQ.WebApi.Api
{
    public class UserController : ApiController
    {
        //[HttpGet,HttpPost]
        //public IHttpActionResult MemberRegister(string username, string userpwd, string yzm, string useryz, string fw)
        //{
        //    Regex accountRex = new Regex("^[A-Za-z0-9_][A-Za-z0-9_]*$");
        //    if (!accountRex.IsMatch(username))
        //    {
        //        return Json(new { code = 0, Msg = "用户名包含非法字符" });
        //    }
        //    DbHelper helper = new DbHelper("QpAccount");

        //    string sql = string.Format("select * from account where account='{0}'", username);
        //    DataSet ds = helper.GetDataTablebySQL(sql);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return Json(new { code = 0, Msg = "用户名已经存在" });
        //    }

        //    string sqlMaxNum = "select MAX(AccountNum) from Account";
        //    DataSet dsMaxNum = helper.GetDataTablebySQL(sqlMaxNum);
        //    int num = 100000;
        //    if (dsMaxNum.Tables[0].Rows.Count > 0)
        //    {
        //        num = dsMaxNum.Tables[0].Rows[0][0].ToInt() + 1;
        //    }


        //    string account = username;
        //    string password = userpwd;
        //    string accounttype = "11";
        //    string accountsecondtype = "0";
        //    string sex = "2";
        //    string nickname = "新手" + num;
        //    string accountnum = num.ToString();
        //    string ipaddress = Net.Ip;
        //    string mac = string.Join(" , ", CQ.Core.Net.GetMacByNetworkInterface());
        //    string details = "|||0|0|||||||";
        //    //|密保问题|密保答案|年龄|身高cm|学历|生肖|星座|职业|省|市|
        //    //string[] userInfo = details.Split('|');
        //    //string mbwt = userInfo[1];
        //    //string mbda = userInfo[2];
        //    //string age = userInfo[3];
        //    //string sg = userInfo[4];
        //    //string xl = userInfo[5];
        //    //string sx = userInfo[6];
        //    //string xz = userInfo[7];
        //    //string zy = userInfo[8];
        //    //string sheng = userInfo[9];
        //    //string shi = userInfo[10];

        //    string realname = "";
        //    string idntirycard = "";
        //    string telephone = "";
        //    string parentid = "";
        //    string Url = GetUrlStr() + string.Format("function={0}&account={1}&password={2}&accounttype={3}&accountsecondtype={4}&sex={5}&nickname={6}&accountnum={7}&ipaddress={8}&mac={9}&details={10}",
        //        "register", account, password, accounttype, accountsecondtype, sex, nickname, accountnum, ipaddress, mac, details);
        //    string msg = HttpMethods.HttpGet(Url);
        //    return Json(new { code = 0, Msg = "" });

        //}

        string GetUrlStr()
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/Configs/GlobConfig.xml");
            string url = XmlHelper.Read(filePath, "configuration/aqiuUrl", "url");
            return url;
        }
    }
}
