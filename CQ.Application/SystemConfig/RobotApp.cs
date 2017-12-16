using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using CQ.Core;
using CQ.Domain.Entity.QPRobot;
using CQ.Repository.EntityFramework;

namespace CQ.Application.SystemConfig
{
    public class RobotApp : BaseApp
    {
        #region 属性

        private readonly DbHelper _qpRobot = new DbHelper("QPRobot");
        private readonly  DbHelper _qpGameName = new DbHelper("QPGameAddRoomName");
        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");

        #endregion

        #region 公共方法
        /// <summary>
        /// 房间配置列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<object> GetRoomAiList(Pagination pagination, string keyValue)
        {
            string sysTable = "RobotRoomAI";
            string sysKey = @"ID";
            string sysFields = @"*";
            string sysOrder = "ID desc";
            int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(keyValue))
            {
                
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
            var dataTable = _qpRobot.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["ID"],
                    RoomName = dr["RoomName"],
                    LoginRoomRate = dr["LoginRoomRate"],
                    LeaveRoomRate = dr["LeaveRoomRate"],
                    InRoomMinTime = dr["InRoomMinTime"],
                    InRoomMaxTime = dr["InRoomMaxTime"],
                    AIText = dr["AIText"],
                    PriorityTable = dr["PriorityTable"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public RobotRoomAI GetRoomAiForm(string keyValue)
        {
            string sql = $"select  * from RobotRoomAI where ID={keyValue}";
            DataSet ds = _qpRobot.GetDataTablebySql(sql);
            if (!(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
            {
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            RobotRoomAI robotRoom = new RobotRoomAI();
            robotRoom = Serialize.TableRowToModel(robotRoom, dr);
            return robotRoom;
        }

        public void SubmitRoomAiForm(RobotRoomAI robotRoom, int keyValue = 0)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@LoginRoomRate",robotRoom.LoginRoomRate),
                new SqlParameter("@LeaveRoomRate",robotRoom.LeaveRoomRate),
                new SqlParameter("@InRoomMinTime",robotRoom.InRoomMinTime),
                new SqlParameter("@InRoomMaxTime",robotRoom.InRoomMaxTime),
                new SqlParameter("@AIText",robotRoom.AIText),
                new SqlParameter("@PriorityTable",robotRoom.PriorityTable),
                new SqlParameter("@RoomName",robotRoom.RoomName),
                new SqlParameter("@Config",robotRoom.Config),
                new SqlParameter("@ID",keyValue),
            };
            if (keyValue > 0)
            {
                string sql =
                    $"update RobotRoomAI set LoginRoomRate=@LoginRoomRate,LeaveRoomRate=@LeaveRoomRate,InRoomMinTime=@InRoomMinTime,InRoomMaxTime=@InRoomMaxTime,AIText=@AIText,PriorityTable=@PriorityTable,RoomName=@RoomName,Config=@Config where ID=@ID";
                _qpRobot.ExecuteSql(sql, parameters);
            }
            else
            {
                string sql =
                    $"insert into RobotRoomAI(LoginRoomRate,LeaveRoomRate,InRoomMinTime,InRoomMaxTime,AIText,PriorityTable,RoomName,Config) values(@LoginRoomRate,@LeaveRoomRate,@InRoomMinTime,@InRoomMaxTime,@AIText,@PriorityTable,@RoomName,@Config)";
                _qpRobot.ExecuteSql(sql, parameters);
            }
        }

        public void DeleteRoomAiForm(int keyValue)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ID", keyValue),
            };
            string sql = $"delete RobotRoomAI where ID=@ID";
            _qpRobot.ExecuteSql(sql, parameters);
        }

        /***游戏配置*************************************************************************/
        /// <summary>
        /// 游戏配置列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<object> GetGameAiList(Pagination pagination, string keyValue)
        {
            string sysTable = "RobotGameAI";
            string sysKey = @"ID";
            string sysFields = @"*";
            string sysOrder = "ID desc";
            int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(keyValue))
            {

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
            var dataTable = _qpRobot.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["ID"],
                    Name = dr["Name"],
                    GameName = dr["GameName"],
                    AIText = dr["AIText"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public RobotGameAI GetGameAiForm(string keyValue)
        {
            string sql = $"select  * from RobotGameAI where ID={keyValue}";
            DataSet ds = _qpRobot.GetDataTablebySql(sql);
            if (!(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
            {
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            RobotGameAI robotRoom = new RobotGameAI();
            robotRoom = Serialize.TableRowToModel(robotRoom, dr);
            return robotRoom;
        }

        public void SubmitGameAiForm(RobotGameAI robotGame, int keyValue = 0)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Name",robotGame.Name),
                new SqlParameter("@GameName",robotGame.GameName),
                new SqlParameter("@AIText",robotGame.AIText),
                new SqlParameter("@ID",keyValue),
            };
            if (keyValue > 0)
            {
                string sql =
                    $"update RobotGameAI set Name=@Name,GameName=@GameName,AIText=@AIText where ID=@ID";
                _qpRobot.ExecuteSql(sql, parameters);
            }
            else
            {
                string sql =
                    $"insert into RobotGameAI(Name,GameName,AIText) values(@Name,@GameName,@AIText)";
                _qpRobot.ExecuteSql(sql, parameters);
            }
        }

        public void DeleteGameAiForm(int keyValue)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ID", keyValue),
            };
            string sql = $"delete RobotGameAI where ID=@ID";
            _qpRobot.ExecuteSql(sql, parameters);
        }


        public List<object> GetTimeAiList(Pagination pagination, string keyValue)
        {
            string sysTable = "RobotGameRoomConfig";
            string sysKey = @"ID";
            string sysFields = @"*";
            string sysOrder = "ID desc";
            int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(keyValue))
            {

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
            var dataTable = _qpRobot.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["ID"],
                    TimeText = dr["TimeText"],
                    roomname = dr["roomname"],
                    roomid = dr["roomid"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public RobotRoomAI GetTimeAiForm(string keyValue)
        {
            string sql = $"select  * from RobotGameRoomConfig where ID={keyValue}";
            DataSet ds = _qpRobot.GetDataTablebySql(sql);
            if (!(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
            {
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            RobotRoomAI robotRoom = new RobotRoomAI();
            robotRoom = Serialize.TableRowToModel(robotRoom, dr);
            return robotRoom;
        }

        public void SubmitTimeAiForm(RobotGameRoomConfig robotGameRoom, int keyValue = 0)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TimeText",robotGameRoom.TimeText),
                new SqlParameter("@roomname",robotGameRoom.roomname),
                new SqlParameter("@roomid",robotGameRoom.roomid),
                new SqlParameter("@ID",keyValue),
            };
            if (keyValue > 0)
            {
                string sql =
                    $"update RobotGameRoomConfig set TimeText=@TimeText,roomname=@roomname,roomid=@roomid where ID=@ID";
                _qpRobot.ExecuteSql(sql, parameters);
            }
            else
            {
                string sql =
                    $"insert into RobotGameRoomConfig(TimeText,roomname,roomid) values(@TimeText,@roomname,@roomid)";
                _qpRobot.ExecuteSql(sql, parameters);
            }
        }

        public void DeleteTimeAiForm(int keyValue)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ID", keyValue),
            };
            string sql = $"delete RobotGameRoomConfig where ID=@ID";
            _qpRobot.ExecuteSql(sql, parameters);
        }

        public List<object> GetGroupList(Pagination pagination, string keyValue)
        {
            string sysTable = "View_GroupConfig";
            string sysKey = @"ID";
            string sysFields = @"*";
            string sysOrder = "ID desc";
            int sysBegin = 1;
            var sysPageIndex = pagination.page;
            var sysPageSize = pagination.rows;
            var sysWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(keyValue))
            {

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
            var dataTable = _qpRobot.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_Id = dr["Id"],
                    GroupName = dr["GroupName"],
                    RoomAIID = dr["RoomAIID"],
                    GameAIID = dr["GameAIID"],
                    gameroomconfigid = dr["gameroomconfigid"],
                    UserCount = dr["UserCount"]
                });
            }
            pagination.records = parameters[9].Value.ToInt();
            return list;
        }

        public string BuildRobot(string account, string password, string nickname, string accountnum, string accountid, string uids)
        {
            string sql =
                $"select a.account,b.NickName,[password],a.accountnum,a.accountid from account a left join UserAccountInfo b on a.AccountID = b.AccountID where a.accountid in ({uids})";
            DataSet ds = _qpAccount.GetDataTablebySql(sql);
            DataTable dt = ds.Tables[0];
            string Url = GetUrlStr() +
                         $"ysfunction=createrobot&account={account}&password={password}&nickname={nickname}&accountnum={accountnum}&accountid={accountid}";
            string msg = HttpMethods.HttpGet(Url);
            Regex rex = new Regex(@"(-\d+|\d+)<");
            int result = 0;
            string response = rex.Match(msg).Groups[1].Value;
            return response;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}