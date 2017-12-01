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
            UsersApp app = new UsersApp();
            var num = app.IsSpecialNum(111111);
        }
    }
}