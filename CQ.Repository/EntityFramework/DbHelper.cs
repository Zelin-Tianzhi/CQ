/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/

using System;
using System.Collections.Generic;
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
        /// 执行查询语句返回首行首列的值
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <param name="cmdtype"></param>
        /// <returns></returns>
        public object GetObject(string strSql, SqlParameter[] paras, CommandType cmdtype = CommandType.Text)
        {
            object o = null;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.CommandType = cmdtype;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);

                }

                conn.Open();
                o = cmd.ExecuteScalar();
                conn.Close();
            }
            return o;

        }
        /// <summary>
        /// 执行带参数的sql语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSql(string cmdText, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();//返回受影响行数
                }
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
        }
        /// <summary>
        /// 执行多条sql语句， 实现数据库事务
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public int ExecuteSqlTran(List<string> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }

        }
    }
}
