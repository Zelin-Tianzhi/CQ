using System;
using System.Collections.Generic;
using System.Data;
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

        #endregion

        #region 公共方法

        public List<object> UserGoldStatis(DateTime begintime, DateTime endtime, string gameid)
        {
            string tableName = "LogDayGame" + begintime.ToString("yyyyMM");

            string sql =
                $"select AccountID,sum(Gold) gold,SUM(AllGold) AllGold, sum(GoldWin) GoldWin from {tableName} ";
            sql += $" where CurrentDay >= '{begintime}' and CurrentDay < '{endtime}' ";
            if (!string.IsNullOrEmpty(gameid))
            {
                sql += $" and GameId={gameid} ";
            }
            sql += " group by AccountID ";
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