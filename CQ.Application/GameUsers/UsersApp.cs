using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CQ.Core;
using CQ.Core.Security;
using CQ.Domain.Entity.QPAccount;
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
        private readonly DbHelper _qpRobot = new DbHelper("QPRobot");

        #endregion

        #region 公共方法

        public bool IpConfig(string ip)
        {
            string urlIndex = "~/Configs/GlobConfig.xml";
            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Configs\\GlobConfig.xml";
            //string FileName = System.Web.HttpContext.Current.Server.MapPath(urlIndex);
            XDocument doc = XDocument.Load(filePath);
            var rel = from p in doc.Descendants("item") where p.Attribute("ip").Value.ToLower() == ip select p;
            return rel != null && rel.Count() > 0 ? true : false;
        }
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
            return msg;
        }
        /// <summary>
        /// 增减用户金币
        /// </summary>
        /// <param name="gold"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public string ModifyGold(long gold, string keyValue)
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
        public string MemberRegister(string username, string userpwd, string macaddress,string mbid, string telphone)
        {
            string str = null;
            if (!string.IsNullOrEmpty(macaddress))
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
                str = Encoding.ASCII.GetString(mingwen); 
            }

            Random ran = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var uuid = ran.Next(1, 37);
            var response = SendRegisterRequest(username, userpwd, str, "11", "0", uuid+"", null, mbid, telphone);
            return response;
        }

        public dynamic Register(string username, string userpwd, string pid)
        {
            string telphone = null;
            //Regex accountRex = new Regex("^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\\d{8}$");
            //if (accountRex.IsMatch(username))
            //{
            //    telphone = username;
            //}

            if (username.Length == 11)
            {
                telphone = username;
            }
            Random ran = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var uuid = ran.Next(1, 37);
            var response = SendRegisterRequest(username, userpwd, "", "11", "0", uuid+"", null, pid, telphone);
            return response;
        }
        /// <summary>
        /// 游客登录
        /// </summary>
        /// <returns></returns>
        public string TouristLogin(string mac, int type)
        {
            string sql = string.Empty;
            string macAddress = "";
            string pid = "";
            if (type == 0)
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

                macAddress = Encoding.ASCII.GetString(mingwen);

                sql = "select * from TouristMac a ";
                sql += "left join Account b on a.AccountID = b.AccountID ";
                sql += " where 1=1 and ";
                sql += " ( ";
                int m = 0;
                foreach (string item in macAddress.Split('|'))
                {
                    if (item.Length >= 12)
                    {
                        if (m > 0)
                        {
                            sql += " or ";
                        }
                        sql += $" MacAddress like '%{item.Trim()}%' ";
                        m++;
                    }
                }

                sql += " ) ";
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
            }
            else
            {
                pid = mac;
                sql = $"select * from account where MobilePhoneID like '%{mac}%'";
                DataSet ds = _qpAccount.GetDataTablebySql(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string username = ds.Tables[0].Rows[0]["Account"].ToString();
                    string pwd = ds.Tables[0].Rows[0]["Password"].ToString();
                    string result = "99&" + username + "&" + pwd;
                    return result;
                }
            }
            int rows = 0;
            string uname = string.Empty;
            DataSet tempDs = new DataSet();
            do
            {
                uname = BuildAccount();
                sql = $"select * from account where account='{uname}'";
                tempDs = _qpAccount.GetDataTablebySql(sql);
                rows = tempDs.Tables[0].Rows.Count;
            } while (rows > 0);
            string upwd = "e10adc3949ba59abbe56e057f20f883e";
            Random ran = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var uuid = ran.Next(1, 37);
            var response = SendRegisterRequest(uname, upwd, macAddress, "0", "7", uuid+"", null, pid, null);
            if (response != "-1" && response != "-3" && response != "-999" && response != "-404")
            {
                string result = $"0&{uname}&{Md5.Md5Hash(upwd+ "hydra")}";// uname + "&" + upwd;
                byte[] bytes1 = Encoding.ASCII.GetBytes(result);
                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
                return type == 0? string.Join(",", bytes2) : result;
            }
            else
            {
                string result = response;
                byte[] bytes1 = Encoding.ASCII.GetBytes(result);
                byte[] bytes2 = YSEncrypt.EncryptFishFile(bytes1);
                return type == 0 ? string.Join(",", bytes2) : result;
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
            var finishCount = 0;
            DateTime start = DateTime.Now;
            for (var i = 0; i < num; i++)
            {
                var account = BuildAccount().ToLower();
                var accountId = GetIdByNum(account, 3).ToInt64();
                if (accountId > 0)
                {
                    continue;
                }

                string pwd = "e10adc3949ba59abbe56e057f20f883e";
                
                string mac = Net.GetMacAddress();

                Random rd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                Random rr = new Random(Environment.TickCount);
                string uuid = rd.Next(1, 37).ToString();
                string utype = "0";
                string secondtype = "1";

                string nickSql =
                    "select top 1 name from RobotNickName where Usering=0 ";
                object nick = _qpRobot.GetObject(nickSql, null);

                string useSql = $"update RobotNickName set Usering=1 where [Name]='{nick + ""}'";
                _qpRobot.ExecuteSql(useSql, null);

                long maxNum = GetMaxUserNum();
                string nickname = nick+"";
                
                string password = Md5.Md5Hash(pwd + "hydra");
                string details = "|||0|0|||||||";

                accountId = -999;
                SqlParameter[] sqlPara = new SqlParameter[]
                {
                new SqlParameter("@Account", account),
                new SqlParameter("@Password", password),
                new SqlParameter("@AccountType", utype),
                new SqlParameter("@Sex", rr.Next(0,2)),
                new SqlParameter("@NickName", nickname),
                new SqlParameter("@AccountSecondType", secondtype),
                new SqlParameter("@AccountNum", maxNum),
                new SqlParameter("@RegisterAddress", Net.Ip),
                new SqlParameter("@Details", details),
                new SqlParameter("@RegisterMac", SqlNull(mac)),
                new SqlParameter("@PhoneID",SqlNull(null)),
                new SqlParameter("@RealName", SqlNull(null)),
                new SqlParameter("@IdentityCard", SqlNull(null)),
                new SqlParameter("@Telephone", SqlNull(null)),
                new SqlParameter("@ParentID", SqlNull(null)),
                new SqlParameter("@PhotoUUID", uuid),
                new SqlParameter("@AccountID", SqlDbType.Int),
                };
                sqlPara[16].Direction = ParameterDirection.Output;

                try
                {
                    _qpAccount.ExecuteNonQueryOutPut("csp_Account_register", sqlPara);
                    accountId = sqlPara[16].Value.ToInt();


                    if (accountId > 0)
                    {
                        var sql = $"insert into SpareRobot(Account,NickName,PassWord,AccountNum,AccountID) values('{account}','{nickname}','{pwd}',{maxNum},{accountId})";
                        int result = _qpRobot.ExecuteSqlCommand(sql);
                        if (result > 0)
                        {
                            finishCount++;
                        }
                        else
                        {
                            List<string> deleteSqlList = new List<string>
                            {
                                $"delete AccountRegInfo where AccountID={accountId}",
                                $"delete UserAccountInfo where AccountID={accountId}",
                                $"delete Account where AccountID={accountId}"
                            };
                            _qpAccount.ExecuteSqlTran(deleteSqlList);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            Log.Debug($"生成{finishCount}条，总时间：" + (DateTime.Now - start).TotalMilliseconds);
            return finishCount+"";
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

        public int ModifyUserInfo(string accountid, string phonenum, string idcardno, string realname,
            string isenablephone)
        {
            List<string> sqlArr = new List<string>();
            sqlArr.Add($"update Account set SafeWay={isenablephone} where accountid={accountid}");
            sqlArr.Add(
                $"update AccountRegInfo set IdentityCard='{idcardno}', RealName='{realname}',Telephone='{phonenum}' where accountid={accountid}");

            int rows = _qpAccount.ExecuteSqlTran(sqlArr);
            return rows;
        }

        public dynamic BindInfo(string pid, string account, string pwd, string nickname, string type)
        {
            string password = Md5.Md5Hash(pwd + "hydra");
            string aidSql = $"select AccountNum from Account where AccountSecondType=7 and  MobilePhoneID='{pid}'";
            object aid = _qpAccount.GetObject(aidSql, null);
            if (aid == null)
            {
                return "-1";
            }
            string accountid = GetIdByNum(aid + "", 0);
            int rows = 0;
            if (type == "1")
            {
                List<string> sqlArr = new List<string>();
                sqlArr.Add(
                    $"update Account set Account='{account}',Password='{password}',AccountSecondType=0 where AccountNum={aid}");
                string sql = $"select AccountID from UserAccountInfo where AccountNum={aid}";
                object obj = _qpAccount.GetObject(sql, null);
                if (obj != null)
                {
                    sqlArr.Add($"update UserAccountInfo set NickName='{nickname}' where AccountNum={aid}");
                }
                else
                {
                    sqlArr.Add(
                        $"insert into UserAccountInfo(AccountID,AccountNum,NickName,CreateTime) values({accountid},{aid},'{nickname}',GETDATE())");
                }

                rows = _qpAccount.ExecuteSqlTran(sqlArr);

            }
            else
            {
                List<string> sqlArr = new List<string>();
                sqlArr.Add($"update Account set Account='{account}',Password='{password}' where AccountNum={aid} ");
                sqlArr.Add($"update AccountRegInfo set Telephone='{account}' where AccountID={accountid}");
                string sql = $"select AccountID from UserAccountInfo where AccountNum={aid}";
                object obj = _qpAccount.GetObject(sql, null);
                if (obj != null)
                {
                    sqlArr.Add($"update UserAccountInfo set NickName='{nickname}' where AccountNum={aid}");
                }
                else
                {
                    sqlArr.Add(
                        $"insert into UserAccountInfo(AccountID,AccountNum,NickName,CreateTime) values({accountid},{aid},'{nickname}',GETDATE())");
                }
                rows = _qpAccount.ExecuteSqlTran(sqlArr);

            }

            if (rows > 0)
            {
                string infoSql = $"select a.*,b.LastLoginTime,c.NickName as Nick from Account a left join AccountLastLogin b on a.AccountID = b.AccountID left join UserAccountInfo c on a.AccountID = c.AccountID  where a.AccountID='{accountid}'";
                DataTable dtTable = _qpAccount.GetDataTablebySql(infoSql).Tables[0];
                string userInfo =
                    $"0|{dtTable.Rows[0]["AccountNum"]}|{dtTable.Rows[0]["Gold"]}|{dtTable.Rows[0]["YuanBao"]}|{dtTable.Rows[0]["GoldBank"]}";
                //object data = new
                //{
                //    AID = dtTable.Rows[0]["AccountNum"],
                //    AccountType = dtTable.Rows[0]["AccountType"],
                //    AccountSecondType = dtTable.Rows[0]["AccountSecondType"],
                //    PhotoUUID = dtTable.Rows[0]["PhotoUUID"],
                //    YuanBao = dtTable.Rows[0]["YuanBao"],
                //    OpenFunctionFlag = dtTable.Rows[0]["OpenFunctionFlag"],
                //    SafeWay = dtTable.Rows[0]["SafeWay"],
                //    OnLineType = dtTable.Rows[0]["OnLineType"],
                //    PID = dtTable.Rows[0]["MobilePhoneID"],
                //    Glod = dtTable.Rows[0]["Gold"],
                //    GoldBank = dtTable.Rows[0]["GoldBank"],
                //    VIP = dtTable.Rows[0]["VipExp"],
                //    LastLoginTime = dtTable.Rows[0]["LastLoginTime"],
                //    NickName = dtTable.Rows[0]["Nick"],
                //    Sex = dtTable.Rows[0]["Sex"]

                //};
                return userInfo;
            }
            else
            {
                return "-1";
            }

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
            Random ran = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            long id = 100001;
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
                    else
                    {
                        maxNum = 100001;
                    }
                }

                long newNum = maxNum.ToInt64();
                do
                {
                    newNum += ran.Next(2, 15);
                    if (IsSpecialNum(newNum))
                    {
                        continue;
                    }
                    Cache.Insert("MaxUserId", (object)newNum);

                } while (false);
                id = newNum;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return id;
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

        private string SendRegisterRequest(string username, string userpwd, string macaddress, string usertype,
            string usersecondtype, string uuid, string nick, string mbid, string telphone)
        {
            long maxNum = GetMaxUserNum();

            string account = username;
            string password = userpwd;
            string accounttype = usertype;
            string accountsecondtype = usersecondtype;
            string sex = "2";
            string nickname = string.IsNullOrEmpty(nick) ? "新手" + maxNum : nick;
            string accountnum = maxNum.ToString();
            string ipaddress = Net.Ip;
            string mac = macaddress;
            string details = "|||0|0|||||||";
            string photouuid = uuid;
            string pid = mbid;
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
            string telephone = telphone;
            string parentid = "";
            string Url = GetUrlStr() +
                         $"ysfunction=register&account={account}&password={password}&accounttype={accounttype}&accountsecondtype={accountsecondtype}&sex={sex}&nickname={nickname}&accountnum={accountnum}&ipaddress={ipaddress}&mac={mac}&details={details}&photouuid={photouuid}&pid={pid}&telephone={telephone}";
            string msg = HttpMethods.HttpGet(Url);
            Regex rex = new Regex(@"(-\d+|\d+)");
            int result = 0;
            string response = rex.Match(msg).Groups[1].Value;
            return msg;
        }

        public string GetIdByNum(string account, int type)
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

        private static object SqlNull(object obj)
        {
            if (obj == null || obj.ToString() == "")
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }
        }
        #endregion
    }
}