using System;
using CQ.Application.AutoService;
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
    }
}
