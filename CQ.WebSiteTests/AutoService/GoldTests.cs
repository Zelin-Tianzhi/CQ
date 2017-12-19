using System;
using CQ.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQ.WebSiteTests.AutoService
{
    [TestClass]
    public class GoldTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            DateTime dt = DateTime.Today;
            string dbName = Enum.Parse(typeof(TableNameEnum), "21").ToString();
        }
    }
}