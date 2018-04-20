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
            app.TouristLogin("30d75532541d732be881cc89f04bc26f24");
        }
    }
}