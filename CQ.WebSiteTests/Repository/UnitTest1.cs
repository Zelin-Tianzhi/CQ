using System.Data;
using System.Data.SqlClient;
using CQ.Application;
using CQ.Repository.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQ.WebSiteTests.Repository
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DbHelper helper = new DbHelper("QpAccount");
            SqlParameter[] parameters =
            {
                new SqlParameter("@sys_Table", "Account"),
                new SqlParameter("@sys_Key", "AccountId"),
                new SqlParameter("@sys_Fields", "*"),
                new SqlParameter("@sys_Where", "1=1"),
                new SqlParameter("@sys_Order", "AccountId DESC"),
                new SqlParameter("@sys_Begin", 1),
                new SqlParameter("@sys_PageIndex", 1),
                new SqlParameter("@sys_PageSize", 30),
                new SqlParameter("@PCount",SqlDbType.Int),
                new SqlParameter("@RCount",SqlDbType.Int), 
            };
            parameters[8].Direction = ParameterDirection.Output;
            parameters[9].Direction = ParameterDirection.Output;
            var data = helper.ExecuteNonQuery(ProcedureConfig.SysPageV2, parameters);
        }
    }
}
