using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.AutoService
{
    public class GoldStatisticsApp : BaseApp
    {
        #region 属性
        
        private readonly DbHelper _qpGameName = new DbHelper("QPGameAddRoomName");
        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");
        private readonly DbHelper _qpLogStatis = new DbHelper("QPLogStatistics");


        private readonly DbHelper _logstatis = new DbHelper("QPLogStatistics");

        #endregion

        #region 公共方法

        public object GetTop1LogDay()
        {
            string sql = $"select top 1 CurrentDay from LogDay order by ID desc";
            var obj = _logstatis.GetObject(sql, null);
            return obj;
        }

        public void GoldStaStart(DateTime time)
        {
            var gameList = GetGameList();
            foreach (DataRow dr in gameList.Rows)
            {
                string dbName = Enum.Parse(typeof(TableNameEnum), dr["GameID"].ToString()).ToString();
                AddLogDayGameXx(dbName, time);
            }

            int monthDays = DateTime.DaysInMonth(time.Year, time.Month);

            AddLogDay(time);

            if (time.Day == monthDays)
            {
                AddLogMonth(time);
            }

        }

        #endregion

        #region 私有方法

        private DataTable GetGameList()
        {
            string sql = "select GameID,GameName from GameRoomName group by GameID,GameName";
            DataSet ds = _qpGameName.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        private void AddLogDayGameXx(string dbname,DateTime time)
        {
            string yearMon = time.ToString("yyyyMM");
            var helper = new DbHelper("QPLog" + dbname);
            string sourceTableName = "Item_" + yearMon;
            string isExists = $"select * from sysobjects where name='{sourceTableName}' and xtype='U'";
            var obj = helper.GetObject(isExists, null);
            if (obj == null)
            {
                return;
            }
            string sql =
                $"SELECT AccountID,RoomID,SUM(GoldWin) AS win,SUM(ABS(GoldWin)) AS allgold, SUM(GoldTax) AS Tax,SUM(CASE WHEN GoldWin>0 THEN GoldWin ELSE 0 END) AS goldWin FROM {sourceTableName} WHERE CreationDate >= '{time}' AND CreationDate< '{time.AddDays(1)}' GROUP BY RoomID,AccountID ";
            DataTable oneGameList = helper.GetDataTablebySql(sql).Tables[0];

            //sql = $"select AccountID from Account where AccountSecondType=1";

            //DataTable robotList = _qpAccount.GetDataTablebySql(sql).Tables[0];

            string targetTableName = "LogDayGame" + yearMon;

            List<string> sqlList = new List<string>();

            foreach (DataRow dr in oneGameList.Rows)
            {
                string insertSql = $"insert into {targetTableName}(GameID,Gold,AllGold,AccountID,RoomID,CurrentDay,GoldWin,Tax) ";
                insertSql +=
                    $" values({dr["RoomID"].ToInt() / 100},{dr["win"]},{dr["allgold"]},{dr["AccountID"]},{dr["RoomID"]},'{time}',{dr["goldWin"]},{dr["Tax"]})";

                sqlList.Add(insertSql);
            }
            if (sqlList.Count > 0)
            {
                _qpLogStatis.ExecuteSqlTran(sqlList);
            }
            else
            {
                Log.Info("【QPLog_"+dbname+"】数据库暂无数据。");
            }
        }

        private void AddLogDay(DateTime time)
        {
            string yearMon = time.ToString("yyyyMM");

            string sql =
                $"select AccountID,SUM(ABS(Gold)) as AllGold,SUM(Gold) as Win,SUM(Tax) as Tax from LogDayGame{yearMon} where CurrentDay >= '{time}' and CurrentDay < '{time.AddDays(1)}' group by AccountID";
            DataTable dt = _qpLogStatis.GetDataTablebySql(sql).Tables[0];

            List<string> sqlList = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                var insertSql = "insert into LogDay(CurrentDay,GoldGame,AllGold,Tax,AccountID) ";
                insertSql += $" values('{time}',{dr["Win"]},{dr["AllGold"]},{dr["Tax"]},{dr["AccountID"]})";
                sqlList.Add(insertSql);
            }
            if (sqlList.Count > 0)
            {
                _qpLogStatis.ExecuteSqlTran(sqlList);
            }
            else
            {
                Log.Info("无人进行游戏。");
            }
        }

        private void AddLogMonth(DateTime time)
        {
            string yearMon = time.ToString("yyyyMM");

            string sql =
                $"select AccountID,SUM(GoldGame) as Win,SUM(ABS(GoldGame)) as AllGold from LogDay where CurrentDay>='{time}' and CurrentDay<'{time.AddDays(1)}' group by AccountID";
            DataTable oneGameTable = _qpLogStatis.GetDataTablebySql(sql).Tables[0];

            List<string> sqlList = new List<string>();

            foreach (DataRow dr in oneGameTable.Rows)
            {
                string insertSql = "insert into LogMonth(CurrentMonth,GoldGame,AccountID,AllGold,Tax) ";
                insertSql += $" values({yearMon.ToInt()},{dr["Win"]},{dr["AccountID"]},{dr["AllGold"]},{dr["Tax"]})";
                sqlList.Add(insertSql);
            }
            if (sqlList.Count > 0)
            {
                _qpLogStatis.ExecuteSqlTran(sqlList);
            }
        }

        #endregion
    }
}