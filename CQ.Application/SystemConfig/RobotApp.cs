using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CQ.Core;
using CQ.Repository.EntityFramework;

namespace CQ.Application.SystemConfig
{
    public class RobotApp
    {
        #region 属性

        private readonly DbHelper _qpAccount = new DbHelper("QPRobot");

        #endregion

        #region 公共方法

        public List<object> GetRoomList(Pagination pagination, string keyValue)
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
            var dataTable = _qpAccount.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
            var list = new List<object>();
            foreach (DataRow dr in dataTable.Rows)
            {
                list.Add(new
                {
                    F_ID = dr["ID"],
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

        #endregion

        #region 私有方法



        #endregion
    }
}