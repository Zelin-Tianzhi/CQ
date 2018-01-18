using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class IpConfigApp : BaseApp
    {
        #region 属性

        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");

        #endregion

        #region 公共方法

        public List<object> GetIpConfigList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //1：黑名单； 0：白名单
            string sysTable = queryParam["iptype"].IsEmpty()? "IPWhiteList" : queryParam["iptype"].ToString() == "1" ? "IPBlackList" : "IPWhiteList";
            string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!queryParam["keyword"].IsEmpty())
            {
                sysWhere += $" and IP like '%{queryParam["keyword"].ToString()}%'";
            }
            if (!queryParam["begintime"].IsEmpty())
            {
                sysWhere += $" and Time>='{queryParam["begintime"]}' ";
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                sysWhere += $" and Time<='{queryParam["endtime"]}' ";
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
                    F_ID = dr["Id"],
                    Ip = dr["IP"],
                    CreateTime = dr["Time"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public int SubmitIpForm(string IpAddress, string IpType, string IsMac)
        {
            var mac = IsMac == "true" ? 1 : 0;
            string sql = string.Empty;
            if (IpType == "0")
            {
                sql = $"insert into IPWhiteList(IP) values('{IpAddress}')";
            }
            else
            {
                sql = $"insert into IPBlackList(IP,IsMac) values('{IpAddress}','{mac}')";
            }
            var result = _qpAccount.ExecuteSqlCommand(sql);
            return result;
        }

        public List<object> GetBigQuotaList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //1：黑名单； 0：白名单
            string sysTable = "UserCheckMacList";
            string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!queryParam["keyword"].IsEmpty())
            {
                sysWhere += $" and MacAddressList like '%{queryParam["keyword"].ToString()}%'";
            }
            if (!queryParam["begintime"].IsEmpty())
            {
                sysWhere += $" and CreateTime>='{queryParam["begintime"]}' ";
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                sysWhere += $" and CreateTime<='{queryParam["endtime"]}' ";
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
                    F_ID = dr["Id"],
                    User = GetIdByNum(dr["AccountID"].ToString(),1),
                    MacAddressList = dr["MacAddressList"],
                    CreateTime = dr["CreateTime"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public int SubmitMacForm(string accountid, string mac)
        {
            string sql = $"insert into UserCheckMacList(AccountID,MacAddressList) values({accountid},'{mac}')";
            var result = _qpAccount.ExecuteSqlCommand(sql);
            return result;
        }

        #endregion

        #region 私有方法

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
            }
            var obj = _qpAccount.GetObject(sql, null);
            return obj?.ToString() ?? "0";
        }

        #endregion
    }
}