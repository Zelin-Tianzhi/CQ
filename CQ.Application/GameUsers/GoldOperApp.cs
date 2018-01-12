using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CQ.Core;
using CQ.Domain.Entity.SystemSecurity;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class GoldOperApp
    {
        #region 属性

        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpLogTotal = new DbHelper("QPLogTotal");

        #endregion

        #region 公共方法

        public List<object> GetTransferList(Pagination pagination, string begintime, string endtime, string outusernum,
            string inusernum)
        {
            const string sysTable = @"Transfer";
            const string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(begintime))
            {
                sysWhere += $" and Date>='{begintime}' ";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sysWhere += $" and Date<='{endtime}' ";
            }
            if (!string.IsNullOrEmpty(outusernum))
            {
                sysWhere += $" and SrcAccountID={GetIdByNum(outusernum, 0)} ";
            }
            if (!string.IsNullOrEmpty(inusernum))
            {
                sysWhere += $" and DstAccountID={GetIdByNum(outusernum, 0)} ";
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
                    OutUser = GetIdByNum(dr["SrcAccountID"].ToString(), 1),
                    ReceiveUser = GetIdByNum(dr["DstAccountID"].ToString(), 1),
                    OutGold = dr["SrcGold"].ToInt64(),
                    ReceiveGold = dr["DstGold"].ToInt64(),
                    Tax = dr["SrcGold"].ToInt64() - dr["DstGold"].ToInt64(),
                    TransferType = GetTransferType(dr["OperateType"].ToString()),
                    OperTime = dr["Date"].ToString()
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
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
            }
            var obj = _qpAccount.GetObject(sql, null);
            return obj?.ToString() ?? "0";
        }

        #endregion

        #region 私有方法



        private string GetTransferType(string type)
        {
            string temp = "";
            switch (type)
            {
                case "10":
                    temp = "五子棋转账";
                    break;
                case "11":
                    temp = "银行转账";
                    break;
                case "100":
                    temp = "管理员操作";
                    break;
            }
            return temp;
        }

        #endregion



    }
}