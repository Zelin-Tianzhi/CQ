using System;
using CQ.Application.SystemConfig;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQ.WebSiteTests.ApplicationTests
{
    [TestClass]
    public class RobotTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            RobotApp app = new RobotApp();
            app.GetRoomAiForm("8201");
        }
    }
}
