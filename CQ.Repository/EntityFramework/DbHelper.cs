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
         /// <param name="strSql">SQL语句</param>
         /// <returns>离线记录DataSet</returns>
         public DataSet GetDataTablebySql(string strSql)
         {
             using (SqlConnection conn = new SqlConnection(connstring))
             {
                 using (SqlCommand cmd = new SqlCommand(strSql, conn))
                 {
                     try
                     {
                         conn.Open();//打开数据源连接
                         DataSet ds = new DataSet();
                         SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                         myAdapter.Fill(ds);
                         return ds;
                     }
                     catch (SqlException ex)
                     {
                         conn.Close();//出异常,关闭数据源连接
                         throw new Exception($"执行{strSql}失败:{ex.Message}");
                     }
                 }
             }
         }
        /// <summary>
        /// 执行存储过程，返回结果集
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteNonQuery(string sqlText, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText,conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (SqlParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                        
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sqlDataAdapter.Fill(ds);
                        DataTable dt = ds.Tables[0];
                        return dt;
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();

                        throw new Exception($"执行{sqlText}失败:{ex.Message}");
                    }
                }
            }
            //SqlConnection sqlconn = new SqlConnection(conn);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = sqlconn;
            //cmd.CommandText = "Categoriestest5";
            //cmd.CommandType = CommandType.StoredProcedure;
            //// 创建参数
            //IDataParameter[] parameters = {
            //    new SqlParameter("@Id", SqlDbType.Int,4) ,
            //    new SqlParameter("@CategoryName", SqlDbType.NVarChar,15) ,
            //    new SqlParameter("rval", SqlDbType.Int,4)
            //};
            //// 设置参数类型
            //parameters[0].Direction = ParameterDirection.Output;    // 设置为输出参数
            //parameters[1].Value = "testCategoryName";         // 给输入参数赋值
            //parameters[2].Direction = ParameterDirection.ReturnValue; // 设置为返回值
            //// 添加参数
            //cmd.Parameters.Add(parameters[0]);
            //cmd.Parameters.Add(parameters[1]);
            //cmd.Parameters.Add(parameters[2]);
            //sqlconn.Open();
            //// 执行存储过程并返回影响的行数
            //Label1.Text = cmd.ExecuteNonQuery().ToString();
            //sqlconn.Close();
            //// 显示影响的行数，输出参数和返回值
            //Label1.Text += "-" + parameters[0].Value.ToString() + "-" + parameters[2].Value.ToString();
        }
    }
}
