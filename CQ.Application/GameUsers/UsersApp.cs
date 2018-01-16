using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using CQ.Core;
using CQ.Core.Security;
using CQ.Plugin.Cache;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class UsersApp : BaseApp
    {
        #region 属性

        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpLogTotal = new DbHelper("QPLogTotal");
        private readonly DbHelper _qpWebLog = new DbHelper("QPWebLog");

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
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
            var dataTable = _qpAccount.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["AccountID"],
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
                    Telephone = dr["Telephone"],
                    UnfreezeDate = dr["UnfreezeDate"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public object GetForm(string keyValue)
        {
            
            string sql = $"select * from View_UserInfo where account='{keyValue}'";
            DataSet ds = _qpAccount.GetDataTablebySql(sql);
            if (!(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
            {
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            string url = GetUrlStr() + $"ysfunction=getuserdata&account={keyValue}";
            string msg = HttpMethods.HttpGet(url, Encoding.Default);
            object data = new
            {
                F_ID = dr["AccountID"],
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
                LastLoginTime = dr["LastLoginTime"],
                RegisterAddress = dr["RegisterAddress"],
                RegisterDate = dr["RegisterDate"],
                RealName = dr["RealName"],
                Telephone = dr["Telephone"],
                UnfreezeDate = dr["UnfreezeDate"],
                Response = msg
            };
            if (msg != "用户不在线" && msg != "帐号不存在")
            {
                string[] userInfo = msg.Split(',');
                //TODO:查询用户详细信息
            }
            else
            {
                
            }
            return data;
        }


        public List<object> GetMacList(Pagination pagination, string keyValue)
        {
            const string sysTable = @"BindMachine";
            const string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            sysWhere += $" and  AccountID={GetIdByNum(keyValue,3)} ";
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
            var dataTable = _qpLogTotal.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    OperateType = dr["OperateType"],
                    MacAddress = dr["MACAddress"].ToString().Split(',')[0],
                    CreateTime = dr["CreateTime"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }
        public List<object> GetLoginLogList(Pagination pagination, string keyValue)
        {
            const string sysTable = @"LoginLog";
            const string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            sysWhere += $" and  AccountID={GetIdByNum(keyValue, 3)} ";
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
            var dataTable = _qpLogTotal.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    LoginIp = dr["LoginIP"],
                    LoginMac = dr["LoginMac"],
                    LoginType = dr["LoginType"],
                    LoginTime = dr["LoginTime"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public string ModifyNickName(string nickname, string keyValue)
        {
            nickname = ConvertToGb2312(nickname);
            string url = GetUrlStr()+ $"ysfunction=changenicheng&account={keyValue}&nicheng={nickname}";
            string msg = HttpMethods.HttpGet(url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            string response = rex.Match(msg).Groups[1].Value;
            return response;
        }
        /// <summary>
        /// 增减用户金币
        /// </summary>
        /// <param name="gold"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public string ModifyGold(int gold, string keyValue)
        {
            var func = gold > 0
                ? $"ysfunction=chongzhijinbi&account={keyValue}&values={gold}&nosendmail={1}"
                : $"ysfunction=kouchujinbi&account={keyValue}&values={gold}&nosendmail={1}";
            string url = GetUrlStr() + func;
            string msg = HttpMethods.HttpGet(url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            string response = rex.Match(msg).Groups[1].Value;
            return response;
        }
        /// <summary>
        /// 重置登录密码
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="pwd"></param>
        /// <param name="oldpwd"></param>
        /// <returns></returns>
        /// c8c8e2585e7555ee27396f4645b415ff （123456密文）
        public string RevisePassword(string keyValue,string pwd, string oldpwd)
        {
            string url = GetUrlStr() + $"ysfunction=changepwd&account={keyValue}&password={pwd}&oldpassword={oldpwd}";
            string msg = HttpMethods.HttpGet(url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            string response = rex.Match(msg).Groups[1].Value;
            return response;
        }
        /// <summary>
        /// 重置银行密码
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public string ReviseBankPassword(string keyValue)
        {
            string url = GetUrlStr() + $"ysfunction=resetbankpwd&account={keyValue}";
            string msg = HttpMethods.HttpGet(url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            string response = rex.Match(msg).Groups[1].Value;
            return response;
        }
        /// <summary>
        /// 踢出游戏
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="nonotify">0:发送通知，1：不发送通知</param>
        /// <returns></returns>
        public string GetOutGame(string keyValue, int nonotify)
        {
            string url = GetUrlStr() + $"ysfunction=tuser&account={keyValue}&nonotify={nonotify}";
            string msg = HttpMethods.HttpGet(url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            string response = rex.Match(msg).Groups[1].Value;
            return response;
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="minute"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string LockUser(string keyValue, long minute, string message)
        {
            string url = GetUrlStr() + $"ysfunction=lockuser&account={keyValue}&minutes={minute}&message={message}";
            string msg = HttpMethods.HttpGet(url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            string response = rex.Match(msg).Groups[1].Value;
            return response;
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

            var response = SendRegisterRequest(username, userpwd, str,"11","0","0");
            return response;
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
            DataSet ds = _qpAccount.GetDataTablebySql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sql = $"select * from account where AccountID={ds.Tables[0].Rows[0]["AccountID"]}";
                DataSet accountDs = _qpAccount.GetDataTablebySql(sql);
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
                ds = _qpAccount.GetDataTablebySql(sql);
                rows = ds.Tables[0].Rows.Count;
            } while (rows > 0);
            string upwd = "c8c8e2585e7555ee27396f4645b415ff";
            var response = SendRegisterRequest(uname, upwd, str, "7", "2","0");
            if (response != "-1" && response != "-3" && response != "-999" && response != "-404")
            {
                string result = $"0&{uname}&{upwd}";// uname + "&" + upwd;
                byte[] bytes1 = Encoding.ASCII.GetBytes(result);
                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
                return string.Join(",", bytes2);
            }
            else
            {
                string result = response;
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
            string sql = $"select * from account where account='{username}'";
            DataSet ds = _qpAccount.GetDataTablebySql(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建机器人登录帐号
        /// </summary>
        /// <returns></returns>
        public string CreateRobotAccount(int num)
        {
            string uids = string.Empty;
            for (int i = 0; i < num; i++)
            {
                string uname = string.Empty;
                string sql = string.Empty;
                int rows = 0;
                DataSet ds = new DataSet();
                do
                {
                    uname = BuildAccount();
                    sql = $"select * from account where account='{uname}'";
                    ds = _qpAccount.GetDataTablebySql(sql);
                    rows = ds.Tables[0].Rows.Count;
                } while (rows > 0);
                string pwd = "c8c8e2585e7555ee27396f4645b415ff";
                string mac = Net.GetMacAddress();
                Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
                string uuid = rd.Next(1, 37).ToString();
                string utype = "0";
                string secondtype = "1";
                string uid = SendRegisterRequest(uname, pwd, mac, utype, secondtype, uuid);
                uids += uid + ",";
            }
            return uids.TrimEnd(',');
        }
        /// <summary>
        /// 投诉记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<object> ComplaintRecord(Pagination pagination, string queryJson)
        {
            const string sysTable = @"View_Complaint";
            const string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["begintime"].IsEmpty())
            {
                sysWhere += $" and Date>='{queryParam["begintime"]}' ";
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                sysWhere += $" and Date<='{queryParam["endtime"]}' ";
            }
            if (!queryParam["outaccount"].IsEmpty())
            {
                sysWhere += $" and SrcAccountID={GetIdByNum(queryParam["outaccount"].ToString(),0)} ";
            }
            if (!queryParam["receiveaccount"].IsEmpty())
            {
                sysWhere += $" and DstAccountID={GetIdByNum(queryParam["receiveaccount"].ToString(),0)} ";
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
            var dataTable = _qpWebLog.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    SrcAccount = dr["Src"],
                    DstAccount = dr["Dst"],
                    GameName = dr["GameName"],
                    RoomName = dr["RoomName"],
                    CreateTime = dr["CreateTime"],
                    Reason = dr["Reason"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }
        /// <summary>
        /// 站内信
        /// </summary>
        /// <param name="account"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string SendMessage(string account, string type, string message)
        {
            message = ConvertToGb2312(message);
            string url = GetUrlStr() + $"ysfunction=sendzhanneixin&typedefine={type}&message={message}";
            if (!string.IsNullOrEmpty(account))
            {
                var accountId = GetIdByNum(account, 3);
                url = GetUrlStr() + $"ysfunction=sendzhanneixintouser&account={account}&message={message}&bonusid=0";
            }
            string msg = HttpMethods.HttpGet(url);

            return msg;
        }
        /// <summary>
        /// 系统广播
        /// </summary>
        /// <param name="opendlg">是否弹窗</param>
        /// <param name="opengo">是否开启跑马灯</param>
        /// <param name="serverid">房间id</param>
        /// <param name="broadcast">消息内容</param>
        /// <returns></returns>
        public string SendBroad(string opendlg, string opengo, string serverid, string broadcast)
        {
            var tc = opendlg == "true" ? 1: 0;
            var pmd = opengo == "true" ? 1 : 0;
            broadcast = ConvertToGb2312(broadcast);
            string url = GetUrlStr() + $"ysfunction=broadinfo&opendlg={tc}&opengo={pmd}&serverid={serverid}&broadcast={broadcast}";
            string msg = HttpMethods.HttpGet(url);

            return msg;
        }

        

        #endregion

        #region 私有方法

        public string ConvertToGb2312(string str)
        {
            String m_Start = str;
            //把unicode的转换为GB2312 
            UnicodeEncoding unicode = new UnicodeEncoding();
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            byte[] m = unicode.GetBytes(m_Start);

            byte[] s;
            s = Encoding.Convert(unicode, gb2312, m);
            return System.Web.HttpUtility.UrlEncode(s);
        }
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
                    DataSet dsMaxNum = _qpAccount.GetDataTablebySql(maxUserIdSql);
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
                    DataSet dsIsExist = _qpAccount.GetDataTablebySql(sql);
                    if (dsIsExist.Tables[0].Rows.Count > 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        Cache.Insert("MaxUserId",maxNum);
                    }

                } while (flag);
                Id = maxNum.ToInt64();
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
        private string SendRegisterRequest(string username, string userpwd, string macaddress, string usertype, string usersecondtype, string uuid)
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
            string mac = macaddress;
            string details = "|||0|0|||||||";
            string photouuid = uuid;
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
                         $"ysfunction=register&account={account}&password={password}&accounttype={accounttype}&accountsecondtype={accountsecondtype}&sex={sex}&nickname={nickname}&accountnum={accountnum}&ipaddress={ipaddress}&mac={mac}&details={details}&photouuid={photouuid}";
            string msg = HttpMethods.HttpGet(Url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            int result = 0;
            string response = rex.Match(msg).Groups[1].Value;
            return msg;
        }

        private string GetIdByNum(string account, int type)
        {
            var sql = string.Empty;// 
            switch (type)
            {
                case 0:
                    sql = $"select AccountID from Account where Accountnum={account}";
                    break;
                case 1:
                    sql = $"select Account from Account where AccountID={account}";
                    break;
                case 2:
                    sql = $"select Account from Account where AccountNum={account}";
                    break;
                case 3:
                    sql = $"select AccountID from Account where Account='{account}'";
                    break;
            }
            var obj = _qpAccount.GetObject(sql, null);
            return obj?.ToString() ?? "0";
        }
        #endregion
    }
}