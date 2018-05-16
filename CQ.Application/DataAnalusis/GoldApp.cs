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

        public List<object> UserGoldStatis(DateTime begintime, DateTime endtime, string gameid,string utype, string account)
        {
            string tableName = "LogDayGame" + begintime.ToString("yyyyMM");

            string sql =
                $"select * from (select AccountID,sum(Gold) gold,SUM(AllGold) AllGold, sum(GoldWin) GoldWin from {tableName} ";
            sql += $" where CurrentDay >= '{begintime}' and CurrentDay <= '{endtime}' ";
            if (!string.IsNullOrEmpty(gameid))
            {
                sql += $" and GameId={gameid} ";
            }

            if (!string.IsNullOrEmpty(account))
            {
                sql += $" and AccountID={account}";
            }
            sql += " group by AccountID) as wintable order by gold desc ";
            DataTable dt = _qpLogStatis.GetDataTablebySql(sql).Tables[0];
            string uids = string.Join(",",(from r in dt.AsEnumerable() select r.Field<int>("AccountID")).ToArray());
            DataTable userDt = new DataTable();
            if (uids.Length > 1)
            {
                string userSql = $"select AccountID,Account from Account where AccountID in ({uids}) ";
                if (!string.IsNullOrEmpty(utype) && string.IsNullOrEmpty(account))
                {
                    userSql += $" and AccountSecondType={utype} ";
                }
                userDt = _qpAccount.GetDataTablebySql(userSql).Tables[0];
            }
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                var row = userDt.Select($"AccountID={dr["AccountID"]}");
                if (row.Length <= 0)
                {
                    continue;
                }
                string userName = row[0]["Account"].ToString();
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

        public List<object> GetUserGameGold(Pagination pagination, string keyValue, string begintime, string engtime, string account)
        {
            if (begintime.IsEmpty())
            {
                begintime = DateTime.Today.AddDays(-1).ToString();
            }
            if ( engtime.IsEmpty())
            {
                engtime = DateTime.Today.ToString();
            }
            //1：黑名单； 0：白名单
            string tableName = " LogDayGame" + begintime.ToDate().ToString("yyyyMM");
            string sourceWhere = " 1=1 ";
            string sourceGroup = " GameID,AccountID ";
            if (keyValue.IsEmpty())
            {
                keyValue = "0";
            }
            if (!account.IsEmpty())
            {
                sourceWhere += $" and AccountID ={account}";
            }
            if (!begintime.IsEmpty())
            {
                sourceWhere += $" and CurrentDay>='{begintime}' ";
            }
            if (!engtime.IsEmpty())
            {
                sourceWhere += $" and CurrentDay<'{engtime}' ";
            }
            if (keyValue !="0" && keyValue != "")
            {
                sourceWhere += $" and GameID={keyValue} ";
            }
            string sourceTable =
                $"(select Row_Number() over ( order by getdate() ) as Id, GameID,sum(Gold) as GoldWin,AccountID from {tableName} where {sourceWhere} group by {sourceGroup}) ";

            string sysTable = sourceTable;
            string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            
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
            var dataTable = _qpLogStatis.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            var gameList = GetGameList();
            string userSql = $"select AccountID,Account from Account where AccountID={account}";
            var userDt = _qpAccount.GetDataTablebySql(userSql).Tables[0];
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    Account = userDt.Rows[0]["Account"].ToString(),
                    GoldWin = dr["GoldWin"],
                    GameName = gameList.Select($"GameID={dr["GameID"].ToString()}")[0]["GameName"].ToString(),
                    GameID = dr["GameID"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public List<object> GetGameGoldList(Pagination pagination, string keyValue, string begintime, string engtime,
            string account)
        {
            if (begintime.IsEmpty())
            {
                begintime = DateTime.Today.AddDays(-1).ToString();
            }
            if (engtime.IsEmpty())
            {
                engtime = DateTime.Today.ToString();
            }
            string dbName = Enum.Parse(typeof(TableNameEnum), keyValue).ToString();
            var helper = new DbHelper("QPLog" + dbName);
            string yearMon = begintime.ToDate().ToString("yyyyMM");
            string sourceTableName = "Item_" + yearMon;
            string isExists = $"select * from sysobjects where name='{sourceTableName}' and xtype='U'";
            var obj = helper.GetObject(isExists, null);
            if (obj == null)
            {
                return new List<object>();
            }
            string sourceWhere = " 1=1 ";
            if (!account.IsEmpty())
            {
                sourceWhere += $" and AccountID ={account}";
            }
            if (!begintime.IsEmpty())
            {
                sourceWhere += $" and CreationDate>='{begintime}' ";
            }
            if (!engtime.IsEmpty())
            {
                sourceWhere += $" and CreationDate<'{engtime}' ";
            }
            string sysTable = sourceTableName;
            string sysKey = @"Id";
            const string sysFields = @"*";
            const string sysOrder = "Id desc";
            const int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = sourceWhere;

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
            var dataTable = helper.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            var gameList = GetGameList();
            string userSql = $"select AccountID,Account from Account where AccountID={account}";
            var userDt = _qpAccount.GetDataTablebySql(userSql).Tables[0];
            var userName = userDt.Rows[0]["Account"].ToString();
            
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["Id"],
                    Account = userName,
                    GameName = gameList.Select($"GameID={keyValue}")[0]["GameName"].ToString(),
                    GameID = keyValue,
                    RoomID = dr["RoomID"],
                    RoomName = gameList.Select($"RoomID={dr["RoomID"].ToString()}")[0]["RoomName"].ToString(),
                    GroupId = keyValue + "_" + yearMon + "_" + dr["GroupID"],
                    GoldTax = dr["GoldTax"],
                    GoldWin = dr["GoldWin"],
                    GoldCurrent = dr["GoldCurrent"],
                    GoldBring = dr["GoldBring"],
                    GoldBank = dr["GoldBank"],
                    GoldTotal = dr["GoldTotal"],
                    CreateTime = dr["CreationDate"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public DataTable GetIdenticalRoundGridJson(long gameid, string yearmonth, string roundflag)
        {
            string connectionStr = Enum.Parse(typeof(TableNameEnum), gameid+"").ToString();
            var helper = new DbHelper("QPLog" + connectionStr);
            string tableName ="Item_" + yearmonth;
            string isExists = $"select * from sysobjects where name='{tableName}' and xtype='U'";
            var obj = helper.GetObject(isExists, null);
            if (obj == null)
            {
                return new DataTable();
            }

            string sql =
                $"SELECT [ID],[GroupID],[RoomID],[CreationDate],[AccountID],[AccountIP],[GoldTax],[GoldWin],[GoldCurrent],[GoldBring],[GoldBank],[GoldTotal],[Bean],[XinYunPoint],[Score] FROM [{tableName}] WHERE GroupID={roundflag}";
            DataTable dt = helper.GetDataTablebySql(sql).Tables[0];
            return dt;
            
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
            string sql = "select GameID,GameName,RoomID,RoomName from GameRoomName";
            DataSet ds = _qpGameName.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        #endregion

    }
}