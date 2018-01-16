using System.Collections.Generic;
using System.Data;
using CQ.Repository.EntityFramework;

namespace CQ.Application.SystemConfig
{
    public class GameApp
    {
        #region 属性
        
        private readonly DbHelper _qpGameName = new DbHelper("QPGameAddRoomName");

        #endregion

        #region 公共方法

        public List<object> GetGameList()
        {
            string sql = $"select GameID,GameName from GameRoomName group by GameID,GameName";
            DataSet ds = _qpGameName.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            var list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["GameID"],
                    GameName = dr["GameName"]
                });
            }
            return list;
        }

        public List<object> GetRoomList(string gameid)
        {
            string sql = $"select RoomID,RoomName from GameRoomName where GameID = {gameid}";
            DataSet ds = _qpGameName.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            var list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["RoomID"],
                    RoomName = dr["RoomName"]
                });
            }
            return list;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}