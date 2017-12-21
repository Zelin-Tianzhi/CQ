using System;
using System.Collections.Generic;
using System.Data;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.DataAnalusis
{
    public class GoldApp :BaseApp
    {
        #region 属性
        
        private readonly DbHelper _qpGameName = new DbHelper("QPGameAddRoomName");

        #endregion

        #region 公共方法

        public void UserGoldRealTime(DateTime begintime, DateTime endtime)
        {
            var gameList = GetGameList();
            foreach (DataRow dr in gameList.Rows)
            {
                string dbName = Enum.Parse(typeof(TableNameEnum), dr["GameID"].ToString()).ToString();
            }
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