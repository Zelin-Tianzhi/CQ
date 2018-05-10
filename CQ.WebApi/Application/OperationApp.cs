using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CQ.Core;
using CQ.Core.Log;
using CQ.WebApi.Models;
using CQ.Repository.EntityFramework;

namespace CQ.WebApi.Application
{
    public class OperationApp
    {

        #region 属性

        public Log Log
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpWebLog = new DbHelper("QPWebLog");
        private readonly DbHelper _qpProp = new DbHelper("QPProp");
        private readonly DbHelper _qpRobot = new DbHelper("QPRobot");

        #endregion

        #region 公共方法

        #region 用户操作

        public dynamic ModifyInfo(Parameters parameters)
        {
            long accountnum = parameters.accountnum;
            string nickname = parameters.nickname;
            string uuid = parameters.photouuid;
            if (accountnum<=0 || string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(uuid))
            {
                return 1;
            }
            string sql = $"select * from account where accountnum={accountnum}";
            DataSet ds = _qpAccount.GetDataTablebySql(sql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return 1;
            }

            sql = $"select * from UserAccountInfo where nickname = '{nickname}' and AccountNum={accountnum}";
            DataSet dsNick = _qpAccount.GetDataTablebySql(sql);
            if (dsNick.Tables[0].Rows.Count > 0)
            {
                return 11;
            }
            List<string> sqlList = new List<string>();
            sqlList.Add($"update Account set PhotoUUID={uuid}, OpenFunctionFlag=3 where AccountNum={accountnum}");
            sql = $"select * from UserAccountInfo where AccountNum={accountnum}";
            DataSet nick = _qpAccount.GetDataTablebySql(sql);
            if (nick.Tables[0].Rows.Count > 0)
            {
                sqlList.Add($"update UserAccountInfo set NickName='{nickname}' where AccountNum={accountnum}");
            }
            else
            {
                sqlList.Add($"insert into UserAccountInfo(AccountID,AccountNum,NickName,CreateTime) values({ds.Tables[0].Rows[0]["AccountID"]},{accountnum},'{nickname}',GETDATE())");
            }

            int rows = _qpAccount.ExecuteSqlTran(sqlList);
            if (rows >= 0)
            {
                return 0;
            }

            return 17;
        }

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic GetAllUserCount(Parameters parameters)
        {
            int type = parameters.type;
            string url = GetUrlStr() + $"ysfunction=getusercount&type={type}";
            string result = HttpMethods.HttpGet(url);
            if (result == null)
            {
                return -5;
            }
            return result;
        }

        public dynamic GetOnlineUser(Parameters parameters)
        {
            int type = parameters.type;
            string url = GetUrlStr() + $"ysfunction=getusercount&type={type}";
            string result = HttpMethods.HttpGet(url);
            if (result == null)
            {
                return -5;
            }
            return result;
        }

        public dynamic UserLoginVerify(Parameters parameters)
        {
            string username = parameters.account;
            string pwd = Md5.md5(parameters.password.ToLower() + "hydra", 32);
            string sql = $"select a.*,b.LastLoginTime,c.NickName as Nick from Account a left join AccountLastLogin b on a.AccountID = b.AccountID left join UserAccountInfo c on a.AccountID = c.AccountID  where a.Account='{username}'";
            DataTable dtTable = _qpAccount.GetDataTablebySql(sql).Tables[0];
            if (dtTable == null || dtTable.Rows.Count <=0 || pwd.ToLower() != dtTable.Rows[0]["Password"].ToString().ToLower())
            {
                return "9"; //帐号密码错误
            }

            string token = Guid.NewGuid().ToString();

            if (dtTable.Rows[0]["OnLineType"].ToString() == "0")
            {
                string updataSql = $"update Account set OnLineType=2,Token='{token}' where Account='{username}'";

                int rows = _qpAccount.ExecuteSql(updataSql, null);
                if (rows > 0)
                {
                    object data = new
                    {
                        AID = dtTable.Rows[0]["AccountNum"],
                        AccountType = dtTable.Rows[0]["AccountType"],
                        AccountSecondType = dtTable.Rows[0]["AccountSecondType"],
                        PhotoUUID = dtTable.Rows[0]["PhotoUUID"],
                        YuanBao = dtTable.Rows[0]["YuanBao"],
                        OpenFunctionFlag = dtTable.Rows[0]["OpenFunctionFlag"],
                        SafeWay = dtTable.Rows[0]["SafeWay"],
                        OnLineType = dtTable.Rows[0]["OnLineType"],
                        PID = dtTable.Rows[0]["MobilePhoneID"],
                        Glod = dtTable.Rows[0]["Gold"],
                        GoldBank = dtTable.Rows[0]["GoldBank"],
                        VIP = dtTable.Rows[0]["VipExp"],
                        LastLoginTime = dtTable.Rows[0]["LastLoginTime"],
                        NickName = dtTable.Rows[0]["Nick"],
                        Sex = dtTable.Rows[0]["Sex"],
                        Token = token
                    };
                    return "0|" + data.ToJson();
                }
                return "8"; // 登录错误
            }
            return dtTable.Rows[0]["OnLineType"];
        }

        public dynamic UserLogin(Parameters parameters)
        {
            string username = parameters.account;
            string token = parameters.token;
            string sql = $"select a.*,b.LastLoginTime,c.NickName as Nick from Account a left join AccountLastLogin b on a.AccountID = b.AccountID left join UserAccountInfo c on a.AccountID = c.AccountID  where a.Account='{username}'";
            DataTable dtTable = _qpAccount.GetDataTablebySql(sql).Tables[0];
            if (dtTable == null || dtTable.Rows.Count <= 0 || token.ToLower() != dtTable.Rows[0]["ToKen"].ToString().ToLower())
            {
                return 9; //帐号密码错误
            }
            if (dtTable.Rows[0]["OnLineType"].ToString() == "0")
            {
                string updataSql = $"update Account set OnLineType=2,Token='{token}' where Account='{username}'";

                int rows = _qpAccount.ExecuteSql(updataSql, null);
                if (rows > 0)
                {
                    object data = new
                    {
                        AID = dtTable.Rows[0]["AccountNum"],
                        AccountType = dtTable.Rows[0]["AccountType"],
                        AccountSecondType = dtTable.Rows[0]["AccountSecondType"],
                        PhotoUUID = dtTable.Rows[0]["PhotoUUID"],
                        YuanBao = dtTable.Rows[0]["YuanBao"],
                        OpenFunctionFlag = dtTable.Rows[0]["OpenFunctionFlag"],
                        SafeWay = dtTable.Rows[0]["SafeWay"],
                        OnLineType = dtTable.Rows[0]["OnLineType"],
                        PID = dtTable.Rows[0]["MobilePhoneID"],
                        Glod = dtTable.Rows[0]["Gold"],
                        GoldBank = dtTable.Rows[0]["GoldBank"],
                        VIP = dtTable.Rows[0]["VipExp"],
                        LastLoginTime = dtTable.Rows[0]["LastLoginTime"],
                        NickName = dtTable.Rows[0]["Nick"],
                        Sex = dtTable.Rows[0]["Sex"],
                        Token = token
                    };
                    return "0|" + data.ToJson();
                }
                return 8; // 登录错误
            }
            return dtTable.Rows[0]["OnLineType"];
        }

        public dynamic Logout(Parameters parameters)
        {
            string username = parameters.account;
            string token = parameters.token;
            string pid = parameters.pid;
            string gold = parameters.gold;
            string bgold = parameters.bgold;
            string yb = parameters.yb;
            string updataSql = $"update Account set OnLineType=0,Gold={gold},GoldBank={bgold},YuanBao={yb} where Account='{username}'";
            int rows = _qpAccount.ExecuteSql(updataSql, null);
            if (rows > 0)
            {
                return 0;
            }

            return -1;
        }

        /// <summary>
        /// 注册帐号
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public long RegisterUser(Parameters parameters)
        {
            if (parameters.accountnum <= 100000 || parameters.account.Length <= 0 || parameters.password.Length <= 0)
            {
                return -1;
            }
            string nickname = string.IsNullOrEmpty(parameters.nickname)
                ? "新手" + parameters.accountnum
                : parameters.nickname;
            parameters.account = parameters.account.ToLower();
            parameters.password = parameters.password.ToLower();
            string password = Md5.md5(parameters.password.ToLower() + "hydra", 32);
            long accountId = GetAccountId(parameters.account);
            if (accountId > 0)
            {
                return -4;
            }
            accountId = -999;
            SqlParameter[] sqlPara = new SqlParameter[]
            {
                new SqlParameter("@Account", parameters.account),
                new SqlParameter("@Password", password),
                new SqlParameter("@AccountType", parameters.accounttype),
                new SqlParameter("@Sex", parameters.sex),
                new SqlParameter("@NickName", nickname),
                new SqlParameter("@AccountSecondType", parameters.accountsecondtype),
                new SqlParameter("@AccountNum", parameters.accountnum),
                new SqlParameter("@RegisterAddress", parameters.ipaddress),
                new SqlParameter("@Details", parameters.details),
                new SqlParameter("@RegisterMac", SqlNull(parameters.mac)),
                new SqlParameter("@PhoneID",SqlNull(parameters.pid)), 
                new SqlParameter("@RealName", SqlNull(parameters.realname)),
                new SqlParameter("@IdentityCard", SqlNull(parameters.idntirycard)),
                new SqlParameter("@Telephone", SqlNull(parameters.telephone)),
                new SqlParameter("@ParentID", SqlNull(parameters.parentid)),
                new SqlParameter("@PhotoUUID", parameters.photouuid),
                new SqlParameter("@AccountID", SqlDbType.Int),
            };
            sqlPara[16].Direction = ParameterDirection.Output;

            try
            {
                _qpAccount.ExecuteNonQueryOutPut("csp_Account_register", sqlPara);
                accountId = sqlPara[16].Value.ToInt();
                if (accountId > 0)
                {
                    string message =
                        $"{parameters.accounttype},{parameters.accountsecondtype},{parameters.details},{parameters.accountnum}";
                    WriteDbLogRegister(accountId + "", parameters.account, message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return accountId > 0 ? parameters.accountnum : 0;
        }

        /// <summary>
        /// 获取用户游戏数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic GetUserData(Parameters parameters)
        {
            string account = parameters.account;
            if (account.Length <= 0)
            {
                return -1;
            }
            string url = GetUrlStr() + $"ysfunction=getuserdata&account={account}";
            string result = HttpMethods.HttpGet(url);
            if (result == null)
            {
                return -5;
            }
            return result;
        }

        /// <summary>
        /// 锁定帐号
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string LockUser(Parameters parameters)
        {
            int min = parameters.minutes;
            string account = parameters.account;
            if (account.Length <= 0 || min <= 0)
            {
                return "-1";
            }
            min += 0;
            if (min < 0)
            {
                min = 0;
            }
            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }
            if (min == 0)
            {
                string sql = $"delete AccountFreeze where AccountID = {accountId}";
                var result = _qpAccount.ExecuteSql(sql, null);
                if (result > 0)
                {
                    WriteDbLogLockUser(accountId + "", account, min + "");
                    return "0";
                }
            }
            else
            {
                string sendMsg = parameters.message;
                List<string> sqlArr = new List<string>();
                sqlArr.Add($"delete AccountFreeze where AccountID = {accountId}");
                sqlArr.Add(
                    $"insert into AccountFreeze(AccountID, FreezeType, UnfreezeDate, LockMessage) values({accountId}, 1, dateadd(n, {min}, getdate()), '{sendMsg}')");
                int result = _qpAccount.ExecuteSqlTran(sqlArr);
                if (result > 0)
                {
                    WriteDbLogLockUser(accountId + "", account, min + "");
                    return "0";
                }
                else
                {
                    return "-3";
                }
            }
            return "0";
        }

        /// <summary>
        /// 踢出游戏
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string TUser(Parameters parameters)
        {
            string account = parameters.account;
            string nonotify = parameters.nonotify;
            if (account.Length <= 0)
            {
                return "-1";
            }
            string url = GetUrlStr() + $"ysfunction=tuser&account={account}&nonotify={nonotify}";
            string result = HttpMethods.HttpGet(url);
            if (result == null)
            {
                return "-5";
            }
            return result;
        }

        /// <summary>
        /// 充值或赠送的操作
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string Chongzhi(Parameters parameters)
        {
            string account = parameters.account;
            long values = parameters.values;
            int opertype = parameters.opertype;
            int type = parameters.type;

            if (account.Length <= 0 || values <=0 || opertype <=0 || type <=0)
            {
                return "-1";
            }
            string url = GetUrlStr() +
                         $"ysfunction=chongzhi&account={account}&values={values}&opertype={opertype}&type={type}";
            string result = HttpMethods.HttpGet(url);
            if (result == null)
            {
                if (opertype+0 != 1)
                {
                    return "-5";
                }
                if (type != 0 && type != 1)
                {
                    return "-5";
                }
                long accountId = GetAccountId(account);
                if (accountId <= 0)
                {
                    return "-2";
                }
                string sql = string.Empty;
                switch (type)
                {
                    case 0:
                        sql =
                            $"update Account set Bean = Bean + {values} where AccountID = {accountId} and Bean + {values} >= 0";
                        break;
                    case 1:
                        sql =
                            $"update Account set GoldBank = GoldBank + {values} where AccountID = {accountId} and GoldBank + {values} >= 0";
                        break;
                }
                int rows = _qpAccount.ExecuteSql(sql, null);
                if (rows > 0)
                {
                    WriteDbLogChongZhi(accountId + "", opertype + "", type + "", values+"");
                    return "0";
                }
                else
                {
                    return "-5";
                }
            }
            return result;
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic Chongzhifunc(Parameters parameters)
        {
            string account = parameters.account;
            long values = parameters.values;
            int opertype = parameters.opertype;
            int type = parameters.type;
            string nosendmail = parameters.nosendmail;

            if (account.Length <= 0 || values <= 0 || opertype <= 0 || type <= 0)
            {
                return -1;
            }
            if (opertype+0 <1 || opertype +0 >3)
            {
                return -5;
            }
            if (type != 0 && type != 1 && type != 3 && type != 5)
            {
                return -5;
            }
            long accountId = GetAccountIDNoRobot(account);
            if (accountId <= 0)
            {
                return -2;
            }
            string sql = string.Empty;
            switch (type)
            {
                case 0:
                    sql =
                        $"update Account set Bean = Bean + {values} where AccountID = {accountId} and Bean + {values} >= 0";
                    break;
                case 1:
                    sql =
                        $"update Account set GoldBank = GoldBank + {values} where AccountID = {accountId} and GoldBank + {values} >= 0";
                    break;
                case 3:
                    sql =
                        $"update Account set TotalExp = TotalExp + 100 * {values} where AccountID = {accountId} and TotalExp + 100 * {values} >= 0";
                    break;
                case 5:
                    sql =
                        $"update Account set YuanBao = YuanBao + {values} where AccountID = {accountId} and YuanBao + {values} >= 0";
                    break;

            }
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                if (opertype == 1 && type == 5)
                {
                    double VipExpAddValue = 0;
                    if (values < 1000)
                    {
                        VipExpAddValue = 2 * values;
                    }
                    else if (values < 10000)
                    {
                        VipExpAddValue = 2.4 * values;
                    }
                    else if (values < 20000)
                    {
                        VipExpAddValue = 2.8 * values;
                    }
                    else if (values < 50000)
                    {
                        VipExpAddValue = 3 * values;
                    }
                    else
                    {
                        VipExpAddValue = 3 * values;
                    }
                    if (VipExpAddValue > 0)
                    {
                        sql = $"update account set VIPExp = VIPExp + {VipExpAddValue} where AccountID = {accountId}";
                        _qpAccount.ExecuteSql(sql, null);
                    }
                }
                WriteDbLogChongZhi(accountId + "", opertype + "", type + "", values + "");

                string url = GetUrlStr() +
                             $"ysfunction=chongzhifuncnotify&account={account}&values={values}&opertype={opertype}&type={type}&nosendmail={nosendmail}";
                HttpMethods.HttpGet(url);
                return 0;
            }
            return -5;
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ChongzhifuncNotify(Parameters parameters)
        {
            string account = parameters.account;
            long values = parameters.values;
            int opertype = parameters.opertype;
            int type = parameters.type;
            string nosendmail = parameters.nosendmail;

            string issendmailforuser = nosendmail == "0" ? "1" : "0";

            string url = GetUrlStr() +
                         $"ysfunction=chongzhifuncnotify&account={account}&values={values}&opertype={opertype}&type={type}&sendmail={issendmailforuser}";
            HttpMethods.HttpGet(url);
            return "0";
        }
        /// <summary>
        /// 修改密码
        /// 对外密码4059673565acaf57fee24fe68f209007
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ChangePwd(Parameters parameters)
        {
            string account = parameters.account;
            string password = parameters.password;
            string oldpassword = parameters.oldpassword;
            if (account.Length <= 0)
            {
                return "-1";
            }
            password = Md5.md5(password.ToLower(),32);
            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }
            string sql = string.Empty;
            if (password.Length <= 0 && oldpassword.Length <= 0)
            {
                string pwd = GetDefaultPwd().ToLower();
                sql = $"update Account set Password = '{pwd}' where AccountID = {accountId}";
            }
            else
            {
                sql =
                    $"update Account set Password = '{password}' where AccountID = {accountId} and Password = '{oldpassword}'";
            }
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                WriteDbLogChangePwd(accountId + "", account, password);
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 防沉迷
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string FangChenMi(Parameters parameters)
        {
            string account = parameters.account;
            string boolvalue = parameters.isbool;
            if (account.Length <= 0 || boolvalue.Length <= 0)
            {
                return "-1";
            }
            boolvalue = boolvalue.ToLower();
            int isbool = 0;
            if (boolvalue == "true")
            {
                isbool = 1;
            }
            else
            {
                isbool = 0;
            }
            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }
            string sql = string.Empty;
            if (isbool == 1)
            {
                sql = $"delete from FangChengMi where AccountID = {accountId}";
            }
            else
            {
                sql = $"insert into FangChengMi(AccountID) values({accountId})";
            }
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                WriteDbLogFangChenMi(accountId + "", account, isbool + "");
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 添加道具到用户
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string AddPropToUser(Parameters parameters)
        {
            string account = parameters.account;
            string propid = parameters.propid;
            string propcountorday = parameters.propcountorday;
            string bind = parameters.bind;

            if (account.Length <= 0 || propid.Length <= 0 || propcountorday.Length <= 0)
            {
                return "-1";
            }
            bind = bind.ToLower();
            int isbind = 0;
            isbind = bind == "true" ? 1 : 0;
            long accountid = GetAccountId(account);
            string sql = $"select top 1 PropType, Name from Prop where PropID = {propid}";
            DataSet ds = new DataSet();
            try
            {
                ds = _qpProp.GetDataTablebySql(sql);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return "-6";
            }
            DataTable dt = ds.Tables[0];
            string propnamedefine = dt.Rows[0]["Name"].ToString();
            if (dt.Rows[0]["PropType"].ToString() == "2")
            {
                sql =
                    $"select top 1 ItemID from CommItem where PropID = {propid} and AccountID = {accountid} and IsBind = {isbind}";
                DataTable propDt = _qpProp.GetDataTablebySql(sql).Tables[0];
                string sqlStr = string.Empty;
                if (propDt.Rows.Count <= 0)
                {
                    sqlStr =
                        $"insert CommItem(AccountID, PropID, IsBind, [Count]) values({accountid}, {propid}, {isbind}, {propcountorday})";
                }
                else
                {
                    string itemId = propDt.Rows[0]["ItemID"].ToString();
                    sqlStr = $"update CommItem set Count = Count + {propcountorday} where ItemID = {itemId}";
                }
                int rows = _qpProp.ExecuteSql(sqlStr, null);
                if (rows > 0)
                {
                    string offlinemessage = $"系统赠送给您[{propcountorday}]个[{propnamedefine}]";
                    WriteOfflineMessage(accountid + "", offlinemessage, 0x03000300);

                    WriteDBLogAddProp(accountid + "", propid, propcountorday, isbind+"");
                    return "0";
                }
                return "-5";
            }
            else
            {
                string sqlStr = string.Empty;
                if (dt.Rows[0]["PropType"].ToString() == "3")
                {
                    sql =
                        $"select top 1 ItemID, DateDiff(day, getdate(), dateadd(day, DueDate, BuyDate)) as T from Item where PropID = {propid} and AccountID = {accountid}";
                    DataTable propDt = _qpProp.GetDataTablebySql(sql).Tables[0];
                    if (propDt.Rows.Count <= 0)
                    {
                        sqlStr =
                            $"insert Item( AccountID, PropID, IsBind, BuyDate, DueDate, Active, PricePerDay) values({accountid}, {propid}, {isbind}, getdate(), {propcountorday}, 0, 0)";
                    }
                    else
                    {
                        string itemId = propDt.Rows[0]["ItemID"].ToString();
                        string tValue = propDt.Rows[0]["T"].ToString();
                        if (tValue.ToInt() >= 0)
                        {
                            sqlStr = $"update Item set DueDate = DueDate + {propcountorday} where ItemID = {itemId}";
                        }
                        else
                        {
                            sqlStr =
                                $"update Item set DueDate = {propcountorday}, BuyDate = getdate() where ItemID = {itemId}";
                        }
                    }
                    int rows = _qpProp.ExecuteSql(sqlStr, null);
                    if (rows > 0)
                    {
                        string offlinemessage = $"系统赠送给您[{propcountorday}]天[{propnamedefine}]";
                        WriteOfflineMessage(accountid + "", offlinemessage, 0x03000300);

                        WriteDBLogAddProp(accountid + "", propid, propcountorday, isbind + "");
                        return "0";
                    }
                    return "-5";
                }
                else
                {
                    sqlStr =
                        $"insert Item( AccountID, PropID, IsBind, BuyDate, DueDate, Active, PricePerDay) values({accountid}, {propid}, {isbind}, getdate(), {propcountorday}, 0, 0)";
                    int rows = _qpProp.ExecuteSql(sqlStr, null);
                    if (rows > 0)
                    {
                        string offlinemessage = $"系统赠送给您[{propcountorday}]天[{propnamedefine}]";
                        WriteOfflineMessage(accountid + "", offlinemessage, 0x03000300);

                        WriteDBLogAddProp(accountid + "", propid, propcountorday, isbind + "");
                        return "0";
                    }
                    return "-5";
                }

            }
        }

        /// <summary>
        /// 变更用户类型
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ChangeAccountType(Parameters parameters)
        {
            string account = parameters.account;
            int type = parameters.type;

            if (account.Length <= 0 || type <=0)
            {
                return "-1";
            }

            type += 0;
            if (type < 0 || type > 9)
            {
                return "-1";
            }
            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }

            string sql = string.Empty;
            switch (type)
            {
                case 0:
                    sql = $"update Account set AccountType = 0, AccountSecondType = 0 where AccountID = {accountId}";
                    break;
                case 1:
                    sql = $"update Account set AccountType = 1, AccountSecondType = 0 where AccountID = {accountId}";
                    break;
                case 2:
                    sql = $"update Account set AccountType = 3 where AccountID = {accountId}";
                    break;
                case 3:
                    sql = $"update Account set AccountType = 4, AccountSecondType = 0 where AccountID = {accountId}";
                    break;
                case 4:
                    sql = $"update Account set AccountSecondType = 3 where AccountID = {accountId}";
                    break;
                case 5:
                    sql = $"update Account set AccountSecondType = 2 where AccountID = {accountId}";
                    break;
                case 6:
                    sql = $"update Account set AccountSecondType = 5 where AccountID = {accountId}";
                    break;
                case 7:
                    sql = $"update Account set AccountSecondType = 6 where AccountID = {accountId}";
                    break;
                case 8:
                    sql = $"update Account set AccountType = 12 where AccountID = {accountId}";
                    break;
                case 9:
                    sql = $"update Account set AccountSecondType = 17 where AccountID = {accountId}";
                    break;
            }
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0 )
            {
                WriteDbLogChangeUserType(accountId + "", account, type);
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 重置银行密码
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ResetBankPwd(Parameters parameters)
        {
            string account = parameters.account;

            if (account.Length <= 0)
            {
                return "-1";
            }

            long accountId = GetAccountId(account);
            if (accountId <=0 )
            {
                return "-2";
            }
            string sql =
                $"update Account set BankPassword = '{GetDefaultPwd()}' where AccountID = {accountId}";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows >0)
            {
                WriteDbLogChangeBankPassword(accountId + "", account);
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 设置身份证
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string SetShenFenZheng(Parameters parameters)
        {
            string account = parameters.account;
            string shenfenzheng = parameters.shenfenzheng;
            if (account.Length <= 0)
            {
                return "-1";
            }

            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }

            string sql = $"update AccountRegInfo set IdentityCard = '{shenfenzheng}' where AccountID = {accountId}";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                WriteDbLogShenfenzheng(accountId + "", account);
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ChangeNicheng(Parameters parameters)
        {
            string account = parameters.account;
            string nicheng = parameters.nicheng;
            if (account.Length <= 0 || nicheng.Length <= 0)
            {
                return "-1";
            }

            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }

            string sql = $"update UserAccountInfo set NickName = '{nicheng}' where AccountID = {accountId}";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                WriteDbLogChangeNicheng(accountId + "", account, nicheng);
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 充值金币
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ChongzhiJinbi(Parameters parameters)
        {
            string account = parameters.account;
            long values = parameters.values;

            if (account.Length <= 0 || values <= 0)
            {
                return "-1";
            }

            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }

            string sql = $"update Account set Gold = Gold + {values} where AccountID = {accountId} and Gold + {values} >= 0";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                WriteDbLogChongZhiJinbi(accountId + "", values+"");
                return "0";
            }
            return "-5";
        }

        /// <summary>
        /// 扣除金币
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string KouChuJinbi(Parameters parameters)
        {
            string account = parameters.account;
            long values = parameters.values;
            string nosendmail = parameters.nosendmail;

            if (account.Length <= 0 || values <= 0)
            {
                return "-1";
            }

            long accountId = GetAccountId(account);
            if (accountId <= 0)
            {
                return "-2";
            }

            int ErrorID = -1;

            SqlParameter[] sqlPara = new SqlParameter[]
            {
                new SqlParameter("@Account", account),
                new SqlParameter("@iGold", values),
                new SqlParameter("@ErrorID", SqlDbType.Int)
            };
            sqlPara[2].Direction = ParameterDirection.Output;

            try
            {
                _qpAccount.ExecuteNonQueryOutPut("csp_kouchujinbi", sqlPara);
                ErrorID = sqlPara[15].Value.ToInt();
                if (ErrorID == 0)
                {
                    WriteDbLogChongZhiJinbi(accountId + "", values + "");
                    string issendmailforuser = nosendmail == "0" ? "1" : "0";
                    string url = GetUrlStr() +
                                 $"ysfunction=chongzhifuncnotify&account={account}&values={values}&opertype=5&type=4&sendmail={issendmailforuser}";
                    HttpMethods.HttpGet(url);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return ErrorID + "";
        }

        /// <summary>
        /// 删除道具
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string DelPropFromUser(Parameters parameters)
        {
            string account = parameters.account;
            string propid = parameters.propid;
            string propcountorday = parameters.propcountorday;
            string bind = parameters.bind;

            if (account.Length <= 0 || propid.Length <= 0 | propcountorday.Length <= 0 )
            {
                return "-1";
            }

            int isbind = 0;
            bind = bind.ToLower();
            isbind = bind == "true" ? 1 : 0;

            long accountid = GetAccountIDNoRobot(account);
            if (accountid <= 0)
            {
                return "-2";
            }
            string sql = $"select top 1 PropType, Name from Prop where PropID = {propid}";
            DataSet ds = new DataSet();
            try
            {
                ds = _qpProp.GetDataTablebySql(sql);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return "-6";
            }
            DataTable dt = ds.Tables[0];
            string propnamedefine = dt.Rows[0]["Name"].ToString();
            if (dt.Rows[0]["PropType"].ToString() == "2")
            {
                sql =
                    $"select top 1 ItemID from CommItem where PropID = {propid} and AccountID = {accountid} and Count >= {propcountorday}";
                DataTable propDt = _qpProp.GetDataTablebySql(sql).Tables[0];
                string sqlStr = string.Empty;
                if (propDt.Rows.Count <= 0)
                {
                    return "-6";
                }

                string itemId = propDt.Rows[0]["ItemID"].ToString();
                sqlStr = $"update CommItem set Count = Count - {propcountorday} where ItemID = {itemId}";

                int rows = _qpProp.ExecuteSql(sqlStr, null);
                if (rows > 0)
                {
                    WriteDBLogAddProp(accountid + "", propid, propcountorday, isbind + "");

                    string url = GetUrlStr() +
                                 $"ysfunction=delpropnotify&account={account}&propid={propid}&propcountorday={propcountorday}&bind={isbind}";
                    HttpMethods.HttpGet(url);
                    return "0";
                }
                return "-5";
            }
            else
            {
                sql = $"select top 1 ItemID from Item where PropID = {propid} and AccountID = {accountid}";
                DataTable propDt = _qpProp.GetDataTablebySql(sql).Tables[0];
                string sqlStr = string.Empty;
                if (propDt.Rows.Count <= 0)
                {
                    return "-6";
                }
                string itemId = propDt.Rows[0]["ItemID"].ToString();
                sqlStr = $"delete Item where ItemID = {itemId}";
                int rows = _qpProp.ExecuteSql(sqlStr, null);
                if (rows > 0)
                {
                    WriteDBLogAddProp(accountid + "", propid, propcountorday, isbind + "");
                    string url = GetUrlStr() +
                                 $"ysfunction=delpropnotify&account={account}&propid={propid}&propcountorday={propcountorday}&bind={isbind}";
                    HttpMethods.HttpGet(url);
                    return "0";
                }
                return "-5";
            }

        }

        /// <summary>
        /// 删除道具
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string DelPropFromUserNotify(Parameters parameters)
        {
            string account = parameters.account;
            string propid = parameters.propid;
            string propcountorday = parameters.propcountorday;
            string bind = parameters.bind;

            string url = GetUrlStr() +
                         $"ysfunction=delprop&account={account}&propid={propid}&propcountorday={propcountorday}&bind={bind}";
            HttpMethods.HttpGet(url);
            return "0";
        }

        /// <summary>
        /// 删除mac绑定
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string DeleteMacBind(Parameters parameters)
        {
            string account = parameters.account;
            if (account.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountIDNoLock(account);
            if (accountid <= 0)
            {
                return "-2";
            }
            string sql = $"delete BindMachine where AccountID = {accountid}";
            _qpAccount.ExecuteSql(sql, null);
            sql = $"delete UserCheckMacList where AccountID = {accountid}";
            _qpAccount.ExecuteSql(sql, null);
            return "0";
        }


        /// <summary>
        /// 删除用户所有道具
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string DeleteUseAllProp(Parameters parameters)
        {
            string account = parameters.account;
            if (account.Length <= 0)
            {
                return "-1";
            }
            long accountid = GetAccountIDNoLock(account);
            if (accountid <= 0)
            {
                return "-2";
            }

            string sql = $"delete Item where AccountID = {accountid}";
            try
            {
                _qpProp.ExecuteSql(sql, null);
                return "0";
            }
            catch (Exception e)
            {
                Log.Error(e);
                return "-5";
            }
        }


        public string DeleteUsePropByID(Parameters parameters)
        {
            string account = parameters.account;
            string findpropid = parameters.propid;
            if (account.Length<=0 || findpropid.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountIDNoLock(account);
            if (accountid <= 0 )
            {
                return "-2";
            }
            string sql = $"delete Item where AccountID = {accountid} and propid = {findpropid}";
            try
            {
                _qpProp.ExecuteSql(sql, null);
                sql = $"delete CommItem where AccountID = {accountid} and propid = {findpropid}";
                _qpProp.ExecuteSql(sql, null);
                return "0";
            }
            catch (Exception e)
            {
                return "-5";
            }

        }

        public string DeleteUsePropByType(Parameters parameters)
        {
            string account = parameters.account;
            string findtypedefine = parameters.typedefine;
            if (account.Length <= 0 || findtypedefine.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountIDNoLock(account);
            if (accountid <= 0)
            {
                return "-2";
            }
            string sql = string.Empty;
            switch (findtypedefine)
            {
                case "10":
                    sql = $"delete CommItem where AccountID = {accountid} and propid >= 100000 and propid <= 200000";
                    try
                    {
                        _qpProp.ExecuteSql(sql, null);
                        return "0";
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        return "-5";
                    }
                case "11":
                    sql = $"delete CommItem where AccountID = {accountid} and propid >= 9000 and propid <= 9100";
                    try
                    {
                        _qpProp.ExecuteSql(sql, null);
                        return "0";
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        return "-5";
                    }
                case "12":
                    sql = $"delete CommItem where AccountID = {accountid} and propid > 7200 and propid < 9000";
                    try
                    {
                        _qpProp.ExecuteSql(sql, null);
                        return "0";
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        return "-5";
                    }
                    default:
                        return "-5";
            }
        }

        public string CheckUserAuth(Parameters parameters)
        {
            string authtype = parameters.authtype;
            string url = GetUrlStr() + $"ysfunction=checkuserauth&authtype={authtype}";
            string result = HttpMethods.HttpGet(url);
            if (string.IsNullOrEmpty(result))
            {
                return "-5";
            }
            return result;
        }

        #endregion

        #region 系统操作

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string BroadCastInfo(Parameters parameters)
        {
            string broadcast = parameters.broadcast;
            if (broadcast.Length <= 0)
            {
                return "-1";
            }
            string url = GetUrlStr() + $"ysfunction=broad&broadcast={broadcast}";
            string result = HttpMethods.HttpGet(url);
            if (result == null)
            {
                return "-5";
            }
            return result;
        }
        /// <summary>
        /// GM广播消息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GMBroadCastInfo(Parameters parameters)
        {
            string broadcast = parameters.broadcast;
            string serverid = parameters.serverid;
            string opengo = parameters.opengo;
            string opendlg = parameters.opendlg;
            if (broadcast.Length <= 0)
            {
                return "-1";
            }
            string sendinfo = broadcast;
            string url = GetUrlStr() +
                         $"ysfunction=broadinfo&serverid={serverid}&opengo={opengo}&opendlg={opendlg}&broadcast={sendinfo}";
            string result = HttpMethods.HttpGet(url);
            return "0";
        }


        public string StartFlopServer(Parameters parameters)
        {
            string serverTypeGet = parameters.serverstate;
            if (serverTypeGet.Length <= 0)
            {
                return "-1";
            }

            string url = GetUrlStr() + $"ysfunction=startflopserver&serverstate={serverTypeGet}";
            string result = HttpMethods.HttpGet(url);
            if (string.IsNullOrEmpty(result))
            {
                return "-5";
            }
            return result;
        }

        public string SetGameServer(Parameters parameters)
        {
            string servicestatus = parameters.servicestatus;
            string roomid = parameters.roomid;

            string url = GetUrlStr() + $"ysfunction=setgameserver&servicestatus={servicestatus}&roomid={roomid}";
            string result = HttpMethods.HttpGet(url);
            if (string.IsNullOrEmpty(result))
            {
                return "-5";
            }
            return result;
        }

        public string SendZhanNeiXin(Parameters parameters)
        {
            string typeDefine = parameters.typedefine;
            string msgInfo = parameters.message;
            if (msgInfo.Length <= 0 )
            {
                return "-1";
            }

            int InsertID = -999;
            int ErrorID = 0;
            int newtypeDefine = 0;
            if (typeDefine == "0")
            {
                newtypeDefine = 0x03000500;
            }
            else
            {
                newtypeDefine = 0x03000400;
            }

            int SendAccountID = 0;
            int TargetAccountID = 0;
            int bonusid = 0;

            SqlParameter[] sqlPara = new SqlParameter[]
            {
                new SqlParameter("@AccountID", SendAccountID),
                new SqlParameter("@TargetAccountID", TargetAccountID),
                new SqlParameter("@TypeDefine", newtypeDefine),
                new SqlParameter("@MsgInfo", msgInfo),
                new SqlParameter("@BonusID", bonusid),
                new SqlParameter("@nReturnValue", SqlDbType.Int),
                new SqlParameter("@ErrorID", SqlDbType.Int),
            };
            sqlPara[5].Direction = ParameterDirection.Output;
            sqlPara[6].Direction = ParameterDirection.Output;

            try
            {
                _qpAccount.ExecuteNonQueryOutPut("OfflineMsgInsertBonus", sqlPara);
                InsertID = sqlPara[5].Value.ToInt();
                if (InsertID > 0)
                {
                    string url = GetUrlStr() + $"ysfunction=sendzhanneixin&typedefine={typeDefine}&message={msgInfo}";
                    HttpMethods.HttpGet(url);
                    return "0";
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return "-5";
        }

        public string SendZhanNeiXinToUser(Parameters parameters)
        {
            string msgInfo = parameters.message;
            string account = parameters.account;
            string bonusID = parameters.bonusid;
            if (msgInfo.Length <= 0 || account.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountIDNoLock(account);
            if (accountid <= 0)
            {
                return "-2";
            }

            int InsertID = -999;
            int ErrorID = 0;
            int newtypeDefine = 0x03000100;

            int SendAccountID = 0;
            long TargetAccountID = accountid;
            int bonusid = bonusID.ToInt();
            SqlParameter[] sqlPara = new SqlParameter[]
            {
                new SqlParameter("@AccountID", SendAccountID),
                new SqlParameter("@TargetAccountID", TargetAccountID),
                new SqlParameter("@TypeDefine", newtypeDefine),
                new SqlParameter("@MsgInfo", msgInfo),
                new SqlParameter("@BonusID", bonusid),
                new SqlParameter("@nReturnValue", SqlDbType.Int),
                new SqlParameter("@ErrorID", SqlDbType.Int),
            };
            sqlPara[5].Direction = ParameterDirection.Output;
            sqlPara[6].Direction = ParameterDirection.Output;

            try
            {
                _qpAccount.ExecuteNonQueryOutPut("OfflineMsgInsertBonus", sqlPara);
                InsertID = sqlPara[5].Value.ToInt();
                if (InsertID > 0)
                {
                    string url = GetUrlStr() + $"ysfunction=sendzhanneixin&message={msgInfo}&accountid={accountid}&offlinemsgid={InsertID}";
                    HttpMethods.HttpGet(url);
                    return "0";
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return "-5";
        }

        public string ReloadConfig(Parameters parameters)
        {
            string mask = parameters.mask;
            string url = GetUrlStr() + $"ysfunction=reloadconfig&mask={mask}";
            string result = HttpMethods.HttpGet(url);
            if (string.IsNullOrEmpty(result))
            {
                return "-5";
            }
            return result;
        }

        public string ChangeRobotPhoto(Parameters parameters)
        {
            string mustbeempty = parameters.changeempty;
            int changebili = parameters.bili * 100;
            if (changebili > 5000)
            {
                return "一次不能改变超过50%";
            }
            string sql = $"select accountid from account where accountsecondtype = 1 and PhotoUUID = 0";
            if (mustbeempty == "0")
            {
                sql = $"select accountid from account where accountsecondtype = 1";
            }

            int rangemin = 1;
            int rangemax = 29;
            List<long>[] arr = new List<long>[rangemax];

            DataTable dt = _qpAccount.GetDataTablebySql(sql).Tables[0];
            int total = 0;
            int totalsuccess = 0;
            int countnumber = dt.Rows.Count;

            Random rm = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(),0));
            foreach (DataRow row in dt.Rows)
            {
                if (rm.Next(0,10000) < changebili)
                {
                    int randnumber = rm.Next(rangemin, rangemax);
                    arr[randnumber].Add(row["accountid"].ToInt64());
                    total++;
                }
            }

            for (int i = rangemin; i <= rangemax; i++)
            {
                List<long> myarray = arr[i];
                if (myarray.Count > 0)
                {
                    string mystr = string.Join(",", myarray);
                    string updateSql = $"update account set PhotoUUID = {i} where accountid in ({mystr})";
                    try
                    {
                        _qpProp.ExecuteSql(updateSql, null);
                        totalsuccess += myarray.Count;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            return $"总个数: {countnumber},改变个数: {total},改变成功个数: {totalsuccess}";
        }

        public string ChangeRobotMember(Parameters parameters)
        {
            string account = parameters.account;
            int vipexp = parameters.vipexp * 100;
            if (account.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountId(account);
            if (accountid <= 0 )
            {
                return "-2";
            }

            string sql = $"update account set vipexp = {vipexp} where accountid = {accountid}";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                return "0";
            }
            return "-5";
        }

        public string AddBlackList(Parameters parameters)
        {
            string black = parameters.black;
            string days = parameters.days;
            string ismac = parameters.ismac;
            string isdelete = parameters.isdelete;

            string sql = string.Empty;
            sql = isdelete == "1" ? $"delete from IPBlackList where IP = '{black}'" : $"insert IPBlackList(IP, Time, IsMac) values('{black}', dateadd(d, {days}, getdate()), {ismac})";
            try
            {
                _qpAccount.ExecuteSql(sql, null);
                string url = GetUrlStr() + $"ysfunction=reloadconfig&mask=8";
                string result = HttpMethods.HttpGet(url);
                return "0";
            }
            catch (Exception e)
            {
                Log.Error(e);
                return "-5";
            }
        }

        public string DoRobotChelueID(Parameters parameters)
        {
            string robotgroup = parameters.groupname;
            string chelueid = parameters.chelueid;

            string sql = $"select accountid from robotaccount where groupname = '{robotgroup}'";

            DataTable dt = _qpRobot.GetDataTablebySql(sql).Tables[0];
            string straccountid = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                straccountid += row["accountid"] + " ";
            }
            return straccountid;
        }

        public string ClearMoney(Parameters parameters)
        {
            string account = parameters.account;
            if (account.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountId(account);
            if (accountid <= 0)
            {
                return "-2";
            }

            string sql = $"update account set Gold = 0,GoldBank = 0,bean=0,yuanbao=0 where account = '{account}'";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                WriteDBLogClearMoney(accountid + "", account);
                return "0";
            }
            return "-5";
        }

        public string ChangeUserDetails(Parameters parameters)
        {
            string account = parameters.account;
            string userinfodata = parameters.userinfo;
            if (account.Length <= 0)
            {
                return "-1";
            }

            long accountid = GetAccountId(account);
            if (accountid <= 0)
            {
                return "-2";
            }
            string writeinfo = "|||0|0|||||||$userinfodata|";
            string sql = $"update AccountRegInfo set Details = '{writeinfo}' where accountid = {accountid}";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                return "0";
            }
            return "-5";
        }

        public string SetPhoneNumber(Parameters parameters)
        {
            string account = parameters.account;
            string userinfodata = parameters.phonenumber;
            if (account.Length <= 0 || (userinfodata.Length != 11 && userinfodata.Length != 0))
            {
                return "-1";
            }
            long accountid = GetAccountId(account);
            if (accountid <= 0)
            {
                return "-2";
            }
            int returnValues = -999;

            SqlParameter[] sqlPara = new SqlParameter[]
            {
                new SqlParameter("@AccountID", accountid),
                new SqlParameter("@PhoneNumber", userinfodata),
                new SqlParameter("@nReturnValue",SqlDbType.Int),
            };
            sqlPara[2].Direction = ParameterDirection.Output;

            try
            {
                _qpAccount.ExecuteNonQueryOutPut("csp_Account_BindPhone", sqlPara);
                returnValues = sqlPara[2].Value.ToInt();
                if (returnValues == 0)
                {
                    return "0";
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return "-5";
        }

        public string SetFunctionHYD(Parameters parameters)
        {
            string account = parameters.account;
            if (account.Length <= 0)
            {
                return "-1";
            }

            string sql = $"update account set OpenFunctionFlag = OpenFunctionFlag | 3 where account = '{account}'";
            int rows = _qpAccount.ExecuteSql(sql, null);
            if (rows > 0)
            {
                return "0";
            }
            return "-5";
        }

        public string SetCloseTime(Parameters parameters)
        {
            long portclosetime = parameters.closetime;
            long timenow = Common.GetCurrentTimeUnix();
            long bussinessclosetime = 0;
            long cacheclosetime = 0;
            if (portclosetime == 0)
            {
                bussinessclosetime = 0;
                cacheclosetime = 0;
            }
            else if (portclosetime - timenow <=3600 || portclosetime-timenow >= 3600 * 24)
            {
                return "设置的时间小于1小时或者大于24小时";
            }
            else
            {
                bussinessclosetime = portclosetime + 60;
                cacheclosetime = portclosetime + 60 * 3;
            }
            string url = GetUrlStr() +
                         $"ysfunction=setclosetime&cachetimeshutdown={cacheclosetime}&bussinesstimeshutdown={bussinessclosetime}&porttimeshutdown={portclosetime}";
            string result = HttpMethods.HttpGet(url);
            if (string.IsNullOrEmpty(result))
            {
                return "设置失败，请求没有返回";
            }
            if (result != "1")
            {
                return "设置失败，请求返回结果是0";
            }
            if (portclosetime == 0)
            {
                return "取消设置成功！";
            }
            return "设置成功，关闭的时间："+Common.ConvertStringToDateTime(portclosetime+"").ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string QuestionVerify(Parameters parameters)
        {
            string roomid = parameters.roomid;
            string accountid = parameters.accountid;
            if (roomid.ToInt() <= 0)
            {
                return "-1";
            }
            string url = GetUrlStr() + $"ysfunction=questionverify&roomid={roomid}&accountid={accountid}";
            string result = HttpMethods.HttpGet(url);
            return result;
        }

        #endregion

        #endregion

        #region 私有方法


        #region 日志记录


        public void WriteDBLogClearMoney(string accountId, string account)
        {
            string sql =
                $"insert into adminoperlog(AccountID, Account, OperType) values ({accountId}, '{account}', 10)";
            _qpWebLog.ExecuteSql(sql, null);
        }
        private void WriteDbLogChongZhiJinbi(string accountId, string values)
        {
            string sql =
                $"insert into adminaddmoney(AccountID, opertype, typedefine, value) values ({accountId}, 4, 1, {values})";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogChangeNicheng(string accountId, string account, string nicheng)
        {
            string sql = $"insert into adminoperlog(AccountID, Account, OperType, Message) values ({accountId}, '{account}', 9, '{nicheng}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogShenfenzheng(string accountId, string account)
        {
            string sql = $"insert into adminoperlog(AccountID, Account, OperType) values ({accountId}, '{account}', 8)";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogChangeBankPassword(string accountId, string account)
        {
            string sql = $"insert into adminoperlog(AccountID, Account, OperType) values ({accountId}, '{account}', 7)";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogChangeUserType(string accountId, string account, int type)
        {
            string sql =
                $"insert into adminoperlog(AccountID, Account, OperType, Message) values ({accountId}, '{account}', 6, '{type}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogRegister(string accountId, string account, string message)
        {
            string sql =
                $"insert into adminoperlog(AccountID, Account, OperType, Message) values ({accountId}, '{account}', 3, '{message}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogLockUser(string accountId, string account, string minutes)
        {
            string sql =
                $"insert into adminoperlog(AccountID, Account, OperType, Message) values ({accountId}, '{account}', 0, '{minutes}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogChongZhi(string accountId, string moneybeanOperType, string moneybeanType,
            string lldValues)
        {
            string sql =
                $"insert into adminaddmoney(AccountID, opertype, typedefine, value) values ({accountId}, {moneybeanOperType}, {moneybeanType}, {lldValues})";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogChangePwd(string accountId, string account, string password)
        {
            string sql =
                $"insert into adminoperlog(AccountID, Account, OperType, Message) values ({accountId}, '{account}', 4, '{password}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDbLogFangChenMi(string accountId, string account, string bfcm)
        {
            string sql =
                $"insert into adminoperlog(AccountID, Account, OperType, Message) values ({accountId}, '{account}', 5, '{bfcm}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteOfflineMessage(string accountId, string msg, int typedef)
        {
            string sql =
                $"insert into OfflineMessage(TargetAccountID, AccountID, OperType, Message) values({accountId}, 0, {typedef}, '{msg}')";
            _qpWebLog.ExecuteSql(sql, null);
        }

        private void WriteDBLogAddProp(string accountId, string propId, string countDay, string bBind)
        {
            string sql =
                $"insert into adminaddprop(AccountID, PropID, bind, countorday) values ({accountId}, {propId}, {bBind}, {countDay})";
            _qpWebLog.ExecuteSql(sql, null);
        }

        #endregion

        private string GetUrlStr()
        {
            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Configs\\GlobConfig.xml";
            //string filePath = System.Web.HttpContext.Current.Server.MapPath("/Configs/GlobConfig.xml");
            string url = XmlHelper.Read(filePath, "configuration/apiServiceUrl", "url");
            return url;
        }

        private string GetDefaultPwd()
        {
            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Configs\\GlobConfig.xml";
            //string filePath = System.Web.HttpContext.Current.Server.MapPath("/Configs/GlobConfig.xml");
            string value = XmlHelper.Read(filePath, "configuration/DefaultPassword", "value");
            return value;
        }

        private long GetAccountId(string account)
        {
            string sql = $"select top 1 AccountID from Account where Account = '{account}'";
            object obj = _qpAccount.GetObject(sql,null);
            if (obj != null)
            {
                return obj.ToInt64();
            }
            else
            {
                return -1;
            }
        }

        private long GetAccountIDNoRobot(string account)
        {
            string sql =
                $"select top 1 a.AccountID from Account as a where a.Account = '{account}' and a.accountsecondtype != 1";
            var data = _qpAccount.GetObject(sql, null);
            if (data != null)
            {
                return data.ToInt64();
            }
            return -1;
        }

        private long GetAccountIDNoLock(string account)
        {
            string sql = $"select top 1 a.AccountID from Account as a where a.Account = '{account}'";
            var data = _qpAccount.GetObject(sql, null);
            if (data != null)
            {
                return data.ToInt64();
            }
            return -1;
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