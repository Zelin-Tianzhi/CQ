using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class PropApp : BaseApp
    {
        #region 属性

        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpLogTotal = new DbHelper("QPLogTotal");
        private readonly DbHelper _qpProp = new DbHelper("QPProp");

        #endregion

        #region 公共方法

        public List<object> GetList(Pagination pagination, string queryJson)
        {
            const string sysTable = @"PropTrade";
            const string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                string accountId = GetIdByNum(keyword, 0);
                sysWhere += $" and (ReceiverAccountID={accountId} or ProviderAccountID={accountId}) ";
            }
            if (!queryParam["begintime"].IsEmpty())
            {
                var begintime = queryParam["begintime"].ToString();
                sysWhere += $" and Date >= '{begintime}' ";
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                var endtime = queryParam["endtime"].ToString();
                sysWhere += $" and Date < '{endtime}' ";
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
                new SqlParameter("@PCount", SqlDbType.Int),
                new SqlParameter("@RCount", SqlDbType.Int),
            };
            parameters[8].Direction = ParameterDirection.Output;
            parameters[9].Direction = ParameterDirection.Output;
            var dataTable = _qpLogTotal.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["Id"].ToInt64(),
                    F_Manage = dr["ProviderAccountID"].ToInt64(),
                    F_Account = GetIdByNum(dr["ReceiverAccountID"].ToString(), 1),
                    F_PropName = GetProNameById(dr["PropID"].ToString()),
                    F_Count = dr["Amount"].ToInt64(),
                    F_Price = dr["Price"].ToInt64(),
                    F_Type = EnumHelper.GetEnumDescription((PropType)Enum.ToObject(typeof(PropType),dr["Operate"].ToInt())),
                    F_OperTime = dr["Date"].ToString()
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
            }
            var obj = _qpAccount.GetObject(sql, null);
            return obj?.ToString() ?? "0";
        }

        private string GetProNameById(string propid)
        {
            var sql = string.Empty;// 
            sql = $"select [name] from Prop where PropID={propid}";
            var obj = _qpProp.GetObject(sql, null);
            return obj?.ToString() ?? "";
        }



        #endregion
    }
}