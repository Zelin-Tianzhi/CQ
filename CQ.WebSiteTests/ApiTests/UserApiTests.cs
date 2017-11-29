using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CQ.WebApi.Api;
using CQ.Core;
using CQ.Core.Log;
using CQ.Core.Security;

namespace CQ.WebSiteTests.ApiTests
{
    [TestClass]
    public class UserApiTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string url = "http://192.168.1.99:8080/api/user/MemberRegister?username=test0101&userpwd=123123&yzm=1234&useryz=1234&fw=123";
            //url = "http://192.168.1.99:8080/api/user/GetAuthCode";//?username=test0101&userpwd=123123&yzm=1234&useryz=1234&fw=123";
            //url = "http://192.168.1.99:8080/api/Utilitys/GetAuthCode";
            //url = "http://192.168.1.10:11003/YLWebToServerInterface/test";
            

            int accountnum = 103061;
            int account = 4061;
            string pwd = "c8c8e2585e7555ee27396f4645b415ff";
            int sex = 1;
            int accounttype = 1;
            string ip = Net.Ip;
            string mac = string.Join(";", Net.GetMacByNetworkInterface());
            string details = "|||0|0|||||||";
            for (int i = 0; i < 10000; i++)
            {
                account += 1;
                accountnum += 1;
                url = "http://115.238.87.114:11004/flop.cpp?" + string.Format("ysfunction={0}&account={1}&password={2}&accounttype={3}&accountsecondtype={4}&sex={5}&nickname={6}&accountnum={7}&ipaddress={8}&mac={9}&details={10}",
                    "register", "w" + account, pwd, 0, accounttype, sex, "新手" + accountnum, accountnum, ip, mac, details);
                try
                {
                    string msg = CQ.Core.HttpMethods.HttpGet(url);
                }
                catch (Exception ex)
                {
                    Log log = LogFactory.GetLogger(GetType().ToString());
                    log.Error(ex.Message);
                }
            }

        }

        [TestMethod]
        public void TestReg()
        {

            string zifuchuan = "UlCk7cAbhAIlhDlw+avnCj/z/KAzJcgO7PBxfq3iX7ECamYbikjcVbbmYARZSUk=";

            Log log = LogFactory.GetLogger();
            log.Info("获取当前运行的方法完整路径。");

            //string url = "http://192.168.1.10:11005/api/user/MemberRegister?username=w11111&userpwd=123123&yzm=1231&useryz=1231&fw=1231";
            //UserController user = new UserController();
            //user.MemberRegister("w21111", "123123", "1231", "1231", "1231");
            //string msg = CQ.Core.HttpMethods.HttpGet(url);
        }
    }
}
