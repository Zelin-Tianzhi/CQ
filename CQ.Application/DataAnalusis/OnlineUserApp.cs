using System;
using System.Collections.Generic;
using System.Data;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.DataAnalusis
{
    public class OnlineUserApp : BaseApp
    {
        #region 属性

        private readonly DbHelper _logTotal = new DbHelper("QPLogTotal");
        private readonly DbHelper _gameList = new DbHelper("QPGameAddRoomName");

        #endregion

        #region 公共方法

        public List<object> GetOnlineUserCount()
        {
            string url = GetUrlStr() + $"ysfunction=getusercount";
            string Mess = HttpMethods.HttpGet(url);
            string[] counts = Mess.Split(',');
            List<object> list = new List<object>();
            if (counts.Length > 0)
            {
                list.Add(new
                {
                    F_Id = 1,
                    CurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ActiveUser = counts[0],
                    InsideUser = counts[3],
                    OrdinaryUser = counts[2],
                    TotalUser = int.Parse(counts[0]) + int.Parse(counts[1]) + int.Parse(counts[2]) + int.Parse(counts[3])
                });
            }

            return list;
        }

        public List<object> GetRoomOnlineUser()
        {
            string sql = string.Empty;
            sql +=
                "select gameid,sum(RobotCount) as RobotCount,sum(AgentCount) as AgentCount,sum(InternalCount) as InternalCount,sum(NormalCount) as NormalCount,sum(FamilyCount) as FamilyCount, sum(Count) as Total ";
            sql += " from OnlineCountGame where CreationDate > dateadd(n, -5, getdate()) group by gameid ";
            DataTable dt = _logTotal.GetDataTablebySql(sql).Tables[0];
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["gameid"],
                    F_GameName = dr["gameid"].ToString() == "0" ? "平台总人数" : GetGameList(dr["gameid"].ToString()),
                    RobotCount = dr["RobotCount"],
                    AgentCount = dr["AgentCount"],
                    InternalCount = dr["InternalCount"],
                    NormalCount = dr["NormalCount"],
                    FamilyCount = dr["FamilyCount"],
                    Total = dr["Total"]
                });
            }

            return list;
        }


        #endregion

        #region 私有方法

        public string GetGameList(string gameid)
        {
            string sql = $"select GameName from GameRoomName where GameID={gameid}";
            var obj = _gameList.GetObject(sql,null);
            return obj?.ToString();
        }

        #endregion
    }
}