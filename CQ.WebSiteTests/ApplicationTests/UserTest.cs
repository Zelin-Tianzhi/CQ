using CQ.Application.GameUsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQ.WebSiteTests.ApplicationTests
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void UsersGetUrlTest()
        {
            //RechargeOrderApp app = new RechargeOrderApp();
            //app.Chongzhi();
            UsersApp app = new UsersApp();
            //app.tousurecord();
        }
        [TestMethod]
        public void TouristLoginTest()
        {
            UsersApp app = new UsersApp();
            app.IpConfig("127.0.0.2");
        }
    }
}