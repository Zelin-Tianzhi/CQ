using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.AutoService
{
    public class GoldStatisticsApp
    {
        #region 属性

        private readonly DbHelper CardNine = new DbHelper("QPLogCardNine");
        private readonly DbHelper DiamondTiger = new DbHelper("QPLogDiamondTiger");
        private readonly DbHelper HundredOx = new DbHelper("QPLogHundredOx");
        private readonly DbHelper OxForTwo = new DbHelper("QPLogOxForTwo");
        private readonly DbHelper ShenShou = new DbHelper("QPLogShenShou");
        private readonly DbHelper ShowHand = new DbHelper("QPLogShowHand");
        private readonly DbHelper _qpGameName = new DbHelper("QPGameAddRoomName");


        private readonly DbHelper _logstatis = new DbHelper("QPLogStatistics");

        #endregion

        #region 公共方法

        public object GetTop1LogDay()
        {
            string sql = $"select top 1 CurrentDay from LogDay order by ID desc";
            var obj = _logstatis.GetObject(sql, null);
            return obj;
        }

        public void GoldStaStart()
        {
            var gameList = GetGameList();
            foreach (DataRow dr in gameList.Rows)
            {
                string dbName = Enum.Parse(typeof(TableNameEnum), dr["GameID"].ToString()).ToString();

            }
        }

        #endregion

        #region 私有方法

        private DataTable GetGameList()
        {
            string sql = $"select GameID,GameName from GameRoomName group by GameID,GameName";
            DataSet ds = _qpGameName.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        private void AddLogDayGameXX(string dbname,DateTime time)
        {
            string yearMon = time.ToString("yyyyMM");
            var helper = new DbHelper("QPLog" + dbname);
            string sql =
                $"SELECT AccountID,RoomID,SUM(GoldWin) AS win,SUM(ABS(GoldWin)) AS allwin, SUM(GoldTax) AS Tax,SUM(CASE WHEN GoldWin>0 THEN GoldWin ELSE 0 END) AS goldWin FROM Item_{yearMon} WHERE CreationDate >= '{time}' AND CreationDate< '{time.AddDays(1)}' GROUP BY RoomID,AccountID ";
            DataTable oneGameList = helper.GetDataTablebySql(sql).Tables[0];
            
        }

        #endregion
    }
}