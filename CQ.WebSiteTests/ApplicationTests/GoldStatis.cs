using System;
using System.Data;
using CQ.Application.AutoService;
using CQ.Repository.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQ.WebSiteTests.ApplicationTests
{
    [TestClass]
    public class GoldStatis
    {
        [TestMethod]
        public void TestMethod1()
        {
            GoldStatisticsApp app = new GoldStatisticsApp();
            app.GoldStaStart(DateTime.Today.AddDays(-1));
        }

        [TestMethod]
        public void DataTableTest()
        {
            DbHelper _qpAccount = new DbHelper("QpAccount");
            DataTable userDt = new DataTable();
            string userSql = $"select AccountID, Account from Account ";
            userDt = _qpAccount.GetDataTablebySql(userSql).Tables[0];
            string userName = userDt.Select($"AccountID=7")[0]["Account"].ToString();
        }
    }
}
