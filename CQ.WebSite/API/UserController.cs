using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using CQ.Core;
using CQ.Plugin.Cache;
using CQ.Core.Security;
using System.Text;

namespace CQ.WebSite.API
{
    public class UserController : BaseApiController
    {
        //[System.Web.Http.HttpGet, System.Web.Http.HttpPost]
        //public IHttpActionResult MemberRegister(string username, string userpwd, string verify, string yzm, string useryz, string fw)
        //{
        //    Log.Debug("这个是用户注册方法");
        //    Regex accountRex = new Regex("^[A-Za-z0-9_][A-Za-z0-9_]*$");
        //    if (!accountRex.IsMatch(username))
        //    {
        //        return Json(3);
        //    }
        //    string code = Cache.Get(fw)?.ToString();
        //    string yzmMd5 = Md5.md5(yzm.ToLower(), 16);
        //    if (code != yzmMd5)
        //    {
        //        return Json(2);
        //    }

        //    DbHelper helper = new DbHelper("QpAccount");

        //    string sql = string.Format("select * from account where account='{0}'", username);
        //    DataSet ds = helper.GetDataTablebySQL(sql);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return Json(4);
        //    }

        //    string sqlMaxNum = "select MAX(AccountNum) from Account";
        //    DataSet dsMaxNum = helper.GetDataTablebySQL(sqlMaxNum);
        //    int num = 100000;
        //    if (dsMaxNum.Tables[0].Rows.Count > 0)
        //    {
        //        num = dsMaxNum.Tables[0].Rows[0][0].ToInt() + 1;
        //    }

        //    int len;
        //    len = verify.Length / 2;
        //    byte[] inputByteArray = new byte[len];
        //    int x, i;
        //    for (x = 0; x < len; x++)
        //    {
        //        i = Convert.ToInt32(verify.Substring(x * 2, 2), 16);
        //        inputByteArray[x] = (byte)i;
        //    }

        //    byte[] mingwen = YSEncrypt.DecryptData(inputByteArray);

        //    string str = Encoding.ASCII.GetString(mingwen);


        //    string account = username;
        //    string password = userpwd;
        //    string accounttype = "11";
        //    string accountsecondtype = "0";
        //    string sex = "2";
        //    string nickname = "新手" + num;
        //    string accountnum = num.ToString();
        //    string ipaddress = Net.Ip;
        //    string mac = str;// string.Join(" , ", CQ.Core.Net.GetMacByNetworkInterface());
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
        //    string Url = GetUrlStr() + string.Format("ysfunction={0}&account={1}&password={2}&accounttype={3}&accountsecondtype={4}&sex={5}&nickname={6}&accountnum={7}&ipaddress={8}&mac={9}&details={10}",
        //        "register", account, password, accounttype, accountsecondtype, sex, nickname, accountnum, ipaddress, mac, details);
        //    string msg = HttpMethods.HttpGet(Url);
        //    Regex rex = new Regex(@"(-\d+|\d+)<");
        //    int result = 0;
        //    string respson = rex.Match(msg).Groups[1].Value;
        //    Log.Debug(msg);
        //    if (respson != "-1" && respson != "-3" && respson != "-999" && respson != "-404")
        //    {
        //        result = 0;
        //    }
        //    else
        //    {
        //        result = respson.ToInt();
        //    }
        //    return Json(result);

        //}
        //[System.Web.Http.HttpGet, System.Web.Http.HttpPost]
        //public IHttpActionResult TouristLogin(string verify)
        //{
        //    Log.Debug("---------------------------------------------------------------------------------");
        //    string result = "0";
        //    try
        //    {
        //        string upwd = "c8c8e2585e7555ee27396f4645b415ff";
                
        //        DbHelper helper = new DbHelper("QpAccount");

        //        int len;
        //        len = verify.Length / 2;
        //        byte[] inputByteArray = new byte[len];
        //        int x, y;
        //        for (x = 0; x < len; x++)
        //        {
        //            y = Convert.ToInt32(verify.Substring(x * 2, 2), 16);
        //            inputByteArray[x] = (byte)y;
        //        }

        //        byte[] mingwen = YSEncrypt.DecryptData(inputByteArray);

        //        string str = Encoding.ASCII.GetString(mingwen);

        //        try
        //        {
        //            string sql = string.Format("select * from AccountRegInfo where ");
        //            int m = 0;
        //            foreach (string item in str.Split('|'))
        //            {
        //                if (item.Length >= 12)
        //                {
        //                    if (m > 0)
        //                    {
        //                        sql += " or ";
        //                    }
        //                    sql += string.Format(" RegisterMac like '%{0}%' ", item.Trim());
        //                    m++;
        //                }
        //            }
        //            DataSet ds = helper.GetDataTablebySQL(sql);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                sql = string.Format("select * from account where AccountID={0}", ds.Tables[0].Rows[0]["AccountID"]);
        //                DataSet accountDs = helper.GetDataTablebySQL(sql);
        //                string username = accountDs.Tables[0].Rows[0]["Account"].ToString();
        //                string pwd = accountDs.Tables[0].Rows[0]["Password"].ToString();
        //                string str1 = "99&" + username + "&" + pwd;
        //                //if (str1.Length < 64)
        //                //{
        //                //    int l = 64 - str1.Length - 1;
        //                //    string lastStr = string.Empty;
        //                //    for (int i = 0; i < l; i++)
        //                //    {
        //                //        lastStr += "0";
        //                //    }
        //                //    str1 += "&";
        //                //    str1 = str1.Insert(str.Length - 1, lastStr);
        //                //}
        //                byte[] bytes1 = Encoding.ASCII.GetBytes(str1);
        //                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
        //                Log.Debug(bytes2);
        //                char[] str2 = Encoding.ASCII.GetChars(bytes2);
        //                Log.Debug(str2);
        //                string str3 = Encoding.ASCII.GetString(bytes2);
        //                Log.Debug(str3);
        //                byte[] b = new byte[bytes2.Length];
        //                b[0] = 82;
        //                Log.Debug(b);
        //                return Json(string.Join(",", bytes2));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex);
        //        }
        //        int rows = 0;
        //        string uname = string.Empty;
        //        do
        //        {
        //            uname = BuildAccount();
        //            string sql = string.Format("select * from account where account='{0}'", uname);
        //            DataSet ds = helper.GetDataTablebySQL(sql);
        //            rows = ds.Tables[0].Rows.Count;
        //        } while (rows > 0);


        //        string sqlMaxNum = "select MAX(AccountNum) from Account";
        //        DataSet dsMaxNum = helper.GetDataTablebySQL(sqlMaxNum);
        //        int num = 100000;
        //        if (dsMaxNum.Tables[0].Rows.Count > 0)
        //        {
        //            num = dsMaxNum.Tables[0].Rows[0][0].ToInt() + 1;
        //        }


        //        string account = uname;
        //        string password = upwd;
        //        string accounttype = "0";
        //        string accountsecondtype = "7";
        //        string sex = "2";
        //        string nickname = uname;
        //        string accountnum = num.ToString();
        //        string ipaddress = Net.Ip;
        //        string mac = str;// string.Join(" , ", Tz.Core.Net.GetMacByNetworkInterface());
        //        string details = "|||0|0|||||||";
        //        //|密保问题|密保答案|年龄|身高cm|学历|生肖|星座|职业|省|市|
        //        //string[] userInfo = details.Split('|');
        //        //string mbwt = userInfo[1];
        //        //string mbda = userInfo[2];
        //        //string age = userInfo[3];
        //        //string sg = userInfo[4];
        //        //string xl = userInfo[5];
        //        //string sx = userInfo[6];
        //        //string xz = userInfo[7];
        //        //string zy = userInfo[8];
        //        //string sheng = userInfo[9];
        //        //string shi = userInfo[10];

        //        string realname = "";
        //        string idntirycard = "";
        //        string telephone = "";
        //        string parentid = "";
        //        string Url = GetUrlStr() + string.Format("ysfunction={0}&account={1}&password={2}&accounttype={3}&accountsecondtype={4}&sex={5}&nickname={6}&accountnum={7}&ipaddress={8}&mac={9}&details={10}",
        //            "register", account, password, accounttype, accountsecondtype, sex, nickname, accountnum, ipaddress, mac, details);
        //        string msg = HttpMethods.HttpGet(Url);
        //        Regex rex = new Regex(@"(-\d+|\d+)<");
                
        //        string respson = rex.Match(msg).Groups[1].Value;
        //        Log.Debug(msg);
        //        if (respson != "-1" && respson != "-3" && respson != "-999" && respson != "-404")
        //        {

        //            result = Encoding.ASCII.GetString(
        //                YSEncrypt.EncryptFishFile(Encoding.ASCII.GetBytes("0&" + uname + "&" + upwd)));
        //            return Json(YSEncrypt.EncryptFishFile(Encoding.ASCII.GetBytes("0&" + uname + "&" + upwd)));

        //        }
        //        else
        //        {
        //            result = respson;
        //            return Json(YSEncrypt.EncryptFishFile(Encoding.ASCII.GetBytes(respson)));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return Json(YSEncrypt.EncryptFishFile(Encoding.ASCII.GetBytes(ex.Message)));
        //    }
        //}

        private string BuildAccount()
        {
            string uname = string.Empty;
            string sFirst = string.Empty;
            string sLast = string.Empty;
            char[] uFirst = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] uLast = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'g', 'h', 'j', 'k', 'm', 'n', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'p', 'q', 'r', 's', 't', 'u', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            Random ran = new Random();
            for (int i = 0; i < 2; i++)
            {
                sFirst += uFirst[ran.Next(uFirst.Length)];
            }
            for (int i = 0; i < 8; i++)
            {
                sLast += uLast[ran.Next(uLast.Length)];
            }

            uname = sFirst + "_" + sLast;
            return uname;
        }

        string GetUrlStr()
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/Configs/GlobConfig.xml");
            string url = XmlHelper.Read(filePath, "configuration/aqiuUrl", "url");
            return url;
        }

    }
}