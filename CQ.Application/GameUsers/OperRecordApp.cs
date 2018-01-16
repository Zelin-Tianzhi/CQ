using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class OperRecordApp:BaseApp
    {
        #region 属性

        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpWebLog = new DbHelper("QPWebLog");

        #endregion

        #region 公共方法

        public List<object> GetExtractList(Pagination pagination, string queryJson)
        {
            const string sysTable = @"UserGoldOperLog";
            const string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 and OperType=2 ";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                sysWhere += $" and AccountID={GetIdByNum(queryParam["keyword"].ToString(), 0)} ";
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
            var dataTable = _qpWebLog.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    Account = GetIdByNum(dr["AccountID"].ToString(),1),
                    Gold = dr["Gold"],
                    BankGold = dr["BankGold"],
                    ChangeGold = dr["ChangeGold"],
                    CreateTime = dr["CreateTime"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        #endregion

        #region 私有方法


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