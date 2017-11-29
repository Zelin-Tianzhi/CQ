/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CQ.Repository.EntityFramework
{
    public class DbHelper
    {
        private string connstring = string.Empty;
        public DbHelper()
        {
            connstring = ConfigurationManager.ConnectionStrings["CqDbContext"].ConnectionString;
        }
        public DbHelper(string connectionStr)
        {
            connstring = ConfigurationManager.ConnectionStrings[connectionStr].ConnectionString;
        }
        public int ExecuteSqlCommand(string cmdText)
        {
            using (DbConnection conn = new SqlConnection(connstring))
            {
                DbCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                return cmd.ExecuteNonQuery();
            }
        }
        private void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (isOpenTrans != null)
                cmd.Transaction = isOpenTrans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

         /// <summary>
         ///  执行查询SQL语句，返回离线记录集
         /// </summary>
         /// <param name="strSQL">SQL语句</param>
         /// <returns>离线记录DataSet</returns>
         public DataSet GetDataTablebySQL(string strSQL)
         {
             using (SqlConnection conn = new SqlConnection(connstring))
             {
                 using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                 {
                     try
                     {
                         conn.Open();//打开数据源连接
                         DataSet ds = new DataSet();
                         SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                         myAdapter.Fill(ds);
                         return ds;
                     }
                     catch (System.Data.SqlClient.SqlException ex)
                     {
                         conn.Close();//出异常,关闭数据源连接
                         throw new Exception(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                     }
                 }
             }
         }
    }
}
