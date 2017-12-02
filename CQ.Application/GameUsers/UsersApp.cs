using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using CQ.Core;
using CQ.Core.Security;
using CQ.Domain.Entity.QPAccount;
using CQ.Domain.IRepository.QPAccount;
using CQ.Plugin.Cache;
using CQ.Repository.EntityFramework;
using CQ.Repository.QPAccount;

namespace CQ.Application.GameUsers
{
    public class UsersApp : BaseApp
    {
        #region 属性

        private DbHelper _helper = new DbHelper("QpAccount");
        private IAccountRepository service = new AccountRepository();

        #endregion

        #region 公共方法

        public List<object> GetList(Pagination pagination, string keyword)
        {
            const string sysTable = @"View_UserInfo";
            const string sysKey = @"AccountID";
            const string sysFields = @"*";
            const string sysOrder = "AccountID desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(keyword))
            {
                sysWhere += " and ";
                sysWhere += " ( ";
                sysWhere += $" AccountNum like '%{keyword}%' ";
                sysWhere += $" or Account like '%{keyword}%' ";
                sysWhere += $" or NickName like '%{keyword}%' ";
                sysWhere += " ) ";
            }
            SqlParameter[] parameters =
            {
                new SqlParameter("@sys_Table", sysTable),
                new SqlParameter("@sys_Key", sysKey),
                new SqlParameter("@sys_Fields", sysFields),
                new SqlParameter("@sys_Where", sysWhere),
                new SqlParameter("@sys_Order", sysOrder),
                new SqlParameter("@sys_Begin", sysBegin),
                new SqlParameter("@sys_PageIndex", sysPageIndex),
                new SqlParameter("@sys_PageSize", sysPageSize),
                new SqlParameter("@PCount",SqlDbType.Int),
                new SqlParameter("@RCount",SqlDbType.Int),
            };
            parameters[8].Direction = ParameterDirection.Output;
            parameters[9].Direction = ParameterDirection.Output;
            var dataTable = _helper.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    AccountID = dr["AccountID"],
                    AccountNum = dr["AccountNum"],
                    AccountName = dr["Account"],
                    NickName = dr["NickName"],
                    Sex = dr["Sex"],
                    AccountType = dr["AccountType"],
                    AccountSecondType = dr["AccountSecondType"],
                    Gold = dr["Gold"],
                    GoldBank = dr["GoldBank"],
                    TotalExp = dr["TotalExp"],
                    LastLoginIP = dr["LastLoginIP"],
                    LastLoginTime =dr["LastLoginTime"],
                    RegisterAddress = dr["RegisterAddress"],
                    RegisterDate = dr["RegisterDate"],
                    RealName = dr["RealName"],
                    Telephone = dr["Telephone"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            //var expression = ExtLinq.True<Account>();
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    expression = expression.And(t => t.AccountName.Contains(keyword));
            //    expression = expression.Or(t => t.NickName.Contains(keyword));
            //}
            return list;
        }
        
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userpwd"></param>
        /// <param name="macaddress"></param>
        /// <returns></returns>
        public string MemberRegister(string username, string userpwd, string macaddress)
        {
            //解密获得mac地址
            var len = macaddress.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(macaddress.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            byte[] mingwen = YSEncrypt.DecryptData(inputByteArray);
            string str = Encoding.ASCII.GetString(mingwen);

            var respson = SendRegisterRequest(username, userpwd, str,"11","0");
            return respson;
        }
        /// <summary>
        /// 游客登录
        /// </summary>
        /// <returns></returns>
        public string TouristLogin(string mac)
        {
            //网卡mac地址
            var len = mac.Length / 2;
            var inputByteArray = new byte[len];
            int x, y;
            for (x = 0; x < len; x++)
            {
                y = Convert.ToInt32(mac.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)y;
            }

            byte[] mingwen = YSEncrypt.DecryptData(inputByteArray);

            var str = Encoding.ASCII.GetString(mingwen);

            string sql = "select * from AccountRegInfo where ";
            int m = 0;
            foreach (string item in str.Split('|'))
            {
                if (item.Length >= 12)
                {
                    if (m > 0)
                    {
                        sql += " or ";
                    }
                    sql += $" RegisterMac like '%{item.Trim()}%' ";
                    m++;
                }
            }
            DataSet ds = _helper.GetDataTablebySql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sql = $"select * from account where AccountID={ds.Tables[0].Rows[0]["AccountID"]}";
                DataSet accountDs = _helper.GetDataTablebySql(sql);
                string username = accountDs.Tables[0].Rows[0]["Account"].ToString();
                string pwd = accountDs.Tables[0].Rows[0]["Password"].ToString();
                string result = "99&" + username + "&" + pwd;
                byte[] bytes1 = Encoding.ASCII.GetBytes(result);
                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
                return string.Join(",", bytes2);
            }
            int rows = 0;
            string uname = string.Empty;
            do
            {
                uname = BuildAccount();
                sql = $"select * from account where account='{uname}'";
                ds = _helper.GetDataTablebySql(sql);
                rows = ds.Tables[0].Rows.Count;
            } while (rows > 0);
            string upwd = "c8c8e2585e7555ee27396f4645b415ff";
            var respson = SendRegisterRequest(uname, upwd, str, "7", "2");
            if (respson != "-1" && respson != "-3" && respson != "-999" && respson != "-404")
            {
                string result = $"0&{uname}&{upwd}";// uname + "&" + upwd;
                byte[] bytes1 = Encoding.ASCII.GetBytes(result);
                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
                return string.Join(",", bytes2);
            }
            else
            {
                string result = respson;
                byte[] bytes1 = Encoding.ASCII.GetBytes(result);
                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
                return string.Join(",", bytes2);
            }
        }
        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UserNameIsExist(string username)
        {
            string sql = string.Format("select * from account where account='{0}'", username);
            DataSet ds = _helper.GetDataTablebySql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public 

        #endregion

        #region 私有方法
        
        /// <summary>
        /// 获取新的用户编号
        /// </summary>
        /// <returns></returns>
        long GetMaxUserNum()
        {
            bool flag = false;
            Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
            long Id = 100001;
            try
            {
                var maxNum = Cache.Get("MaxUserId");
                if (maxNum == null)
                {
                    string maxUserIdSql = "select max(accountnum) from account";
                    DataSet dsMaxNum = _helper.GetDataTablebySql(maxUserIdSql);
                    if (dsMaxNum.Tables[0].Rows.Count > 0)
                    {
                        maxNum = dsMaxNum.Tables[0].Rows[0][0].ToInt() + 1;
                    }
                }
                do
                {
                    maxNum = maxNum.ToInt64() + ran.Next(2, 10);
                    if (IsSpecialNum(maxNum.ToInt64()))
                    {
                        continue;
                    }
                    //用户num是否存在
                    string sql = $"select accountid from account where AccountNum='{maxNum}'";
                    DataSet dsIsExist = _helper.GetDataTablebySql(sql);
                    if (dsIsExist.Tables[0].Rows.Count > 0)
                    {
                        flag = true;
                    }

                } while (flag);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return Id;
        }
        /// <summary>
        /// 匹配五连号或者五个连续相同数字的编号
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool IsSpecialNum(long num)
        {
            string c = num.ToString();
            //是否五位相同数字
            Regex reg = new Regex(@"(\d)\1{4,}");
            MatchCollection matchResult = reg.Matches(c);
            if (matchResult.Count > 0)
            {
                return true;
            }
            char[] cArr = c.ToCharArray();
            int n = 0, m = 0;
            //是否五连号
            for (int i = 0; i < cArr.Length - 1; i++)
            {
                if (cArr[i] - cArr[i + 1] == 1)
                {
                    n++;
                }
                else if (n < 4)
                {
                    n = 0;
                }
             }
            if (n >= 4) return true;
            for (int i = 0; i < cArr.Length - 1; i++)
            {
                if (cArr[i] - cArr[i + 1] == -1)
                {
                    m++;
                }
                else if (m < 4)
                {
                    m = 0;
                }
            }
            if (m >= 4) return true;

            return false;
        }
        /// <summary>
        /// 获取配置文件变量
        /// </summary>
        /// <returns></returns>
        string GetUrlStr()
        {
            //string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Configs\\GlobConfig.xml";
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/Configs/GlobConfig.xml");
            string url = XmlHelper.Read(filePath, "configuration/aqiuUrl", "url");
            return url;
        }
        /// <summary>
        /// 游客登录生成登录帐号
        /// </summary>
        /// <returns></returns>
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
        private string SendRegisterRequest(string username, string userpwd, string str, string usertype, string usersecondtype)
        {
            long maxNum = GetMaxUserNum();

            string account = username;
            string password = userpwd;
            string accounttype = usertype;
            string accountsecondtype = usersecondtype;
            string sex = "2";
            string nickname = "新手" + maxNum;
            string accountnum = maxNum.ToString();
            string ipaddress = Net.Ip;
            string mac = str;
            string details = "|||0|0|||||||";
            //|密保问题|密保答案|年龄|身高cm|学历|生肖|星座|职业|省|市|
            //string[] userInfo = details.Split('|');
            //string mbwt = userInfo[1];
            //string mbda = userInfo[2];
            //string age = userInfo[3];
            //string sg = userInfo[4];
            //string xl = userInfo[5];
            //string sx = userInfo[6];
            //string xz = userInfo[7];
            //string zy = userInfo[8];
            //string sheng = userInfo[9];
            //string shi = userInfo[10];

            string realname = "";
            string idntirycard = "";
            string telephone = "";
            string parentid = "";
            string Url = GetUrlStr() +
                         $"ysfunction=register&account={account}&password={password}&accounttype={accounttype}&accountsecondtype={accountsecondtype}&sex={sex}&nickname={nickname}&accountnum={accountnum}&ipaddress={ipaddress}&mac={mac}&details={details}";
            string msg = HttpMethods.HttpGet(Url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            int result = 0;
            string respson = rex.Match(msg).Groups[1].Value;
            return respson;
        }
        #endregion
    }
}