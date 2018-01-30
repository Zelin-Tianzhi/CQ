using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.DataAnalusis
{
    public class GoldApp :BaseApp
    {
        #region 属性
        
        private readonly DbHelper _qpGameName = new DbHelper("QPGameAddRoomName");
        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpLogStatis = new DbHelper("QPLogStatistics");
        private readonly DbHelper _qpLogTotal = new DbHelper("QPLogTotal");

        #endregion

        #region 公共方法

        public List<object> UserGoldStatis(DateTime begintime, DateTime endtime, string gameid)
        {
            string tableName = "LogDayGame" + begintime.ToString("yyyyMM");

            string sql =
                $"select * from (select AccountID,sum(Gold) gold,SUM(AllGold) AllGold, sum(GoldWin) GoldWin from {tableName} ";
            sql += $" where CurrentDay >= '{begintime}' and CurrentDay < '{endtime}' ";
            if (!string.IsNullOrEmpty(gameid))
            {
                sql += $" and GameId={gameid} ";
            }
            sql += " group by AccountID) as wintable order by GoldWin desc ";
            DataTable dt = _qpLogStatis.GetDataTablebySql(sql).Tables[0];
            string uids = string.Join(",",(from r in dt.AsEnumerable() select r.Field<int>("AccountID")).ToArray());
            DataTable userDt = new DataTable();
            if (uids.Length > 1)
            {
                string userSql = $"select AccountID,Account from Account where AccountID in ({uids})";
                userDt = _qpAccount.GetDataTablebySql(userSql).Tables[0];
            }
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                string userName = userDt.Select($"AccountID={dr["AccountID"].ToString()}")[0]["Account"].ToString();
                list.Add(new
                {
                    Account = userName,
                    AccountId = dr["AccountID"].ToString(),
                    WinGold = dr["gold"].ToInt64(),
                    AllGold = dr["AllGold"].ToInt64(),
                    Profit = dr["GoldWin"].ToInt64()
                });
            }
            //pagination.records = dt.Rows.Count;
            //pagination.rows = dt.Rows.Count;
            
            return list;
        }

        public List<object> GetTaxList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //1：黑名单； 0：白名单
            string sysTable = "View_Tax";
            string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!queryParam["keyword"].IsEmpty())
            {
                sysWhere += $" and AccountID ={queryParam["keyword"].ToString()}";
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
            var dataTable = _qpLogTotal.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    AccountId = dr["AccountID"],
                    CreateTime = dr["CreateTime"],
                    Tax = dr["Tax"],
                    TaxType = dr["TaxType"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        #endregion

        #region 私有方法


        private DataTable AddLogDayGameXx(string dbname, DateTime time)
        {
            string yearMon = time.ToString("yyyyMM");
            var helper = new DbHelper("QPLog" + dbname);
            string sql =
                $"SELECT AccountID,RoomID,SUM(GoldWin) AS win,SUM(ABS(GoldWin)) AS allgold, SUM(GoldTax) AS Tax,SUM(CASE WHEN GoldWin>0 THEN GoldWin ELSE 0 END) AS goldWin FROM Item_{yearMon} WHERE CreationDate >= '{time}' AND CreationDate< '{time.AddDays(1)}' GROUP BY RoomID,AccountID ";
            DataTable oneGameList = helper.GetDataTablebySql(sql).Tables[0];
            return oneGameList;
        }




















        private DataTable GetGameList()
        {
            string sql = "select GameID,GameName from GameRoomName group by GameID,GameName";
            DataSet ds = _qpGameName.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        #endregion

    }
}