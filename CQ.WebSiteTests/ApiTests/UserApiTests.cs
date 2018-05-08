using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CQ.WebSite.API;
using CQ.Core;
using CQ.Core.Log;
using CQ.Core.Security;
using CQ.WebApi.Application;
using CQ.WebApi.Models;
using System.Xml.Linq;

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
            

            int accountnum = 203062;
            int account = 4061;
            string pwd = "2396926147ee02b652877fb5ca551f9b";
            int sex = 1;
            int accounttype = 1;
            string ip = Net.Ip;
            string mac = string.Join(";", Net.GetMacByNetworkInterface());
            string details = "|||0|0|||||||";
            string pid = "123sjkhfaiueyi123123";
            
                account += 1;
                accountnum += 1;
            url = "http://183.131.69.236:10101/api/factory?" + string.Format(
                      "ysfunction={0}&account={1}&password={2}&accounttype={3}&accountsecondtype={4}&sex={5}&nickname={6}&accountnum={7}&ipaddress={8}&mac={9}&details={10}&pid={11}&photouuid={12}",
                      "register", "z222211123321", pwd, 0, accounttype, sex, "新手" + accountnum, accountnum, ip, mac,
                      details, pid,0);
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

        [TestMethod]
        public void TestReg()
        {
            ///123456 MD5之后的值：E10ADC3949BA59ABBE56E057F20F883E
            string pwd = "123qwe";

            string ss = Md5.md5(pwd.ToLower(), 32);

            OperationApp app = new OperationApp();

            Parameters par = new Parameters();
            par.account = "master520";
            par.password = ss;
            object data = app.UserLoginVerify(par);
            string result = data.ToJson();

            //string zifuchuan = "UlCk7cAbhAIlhDlw+avnCj/z/KAzJcgO7PBxfq3iX7ECamYbikjcVbbmYARZSUk=";

            ////Log log = LogFactory.GetLogger();
            ////log.Info("获取当前运行的方法完整路径。");
            //string verify = "30d75532541d732be881cc89f04bc26f24";
            //string url = "http://192.168.1.10:11005/api/user/MemberRegister?username=w11111&userpwd=123123&yzm=1231&useryz=1231&fw=1231";
            //UserController user = new UserController();
            //user.TouristLogin(verify);
            //string msg = CQ.Core.HttpMethods.HttpGet(url);
        }

        [TestMethod]
        public void MobileRegister()
        {
            object data = new
            {
               AID=1,PID=2,CID=3
            };
            var aa = "0|" + data.ToJson();

            //123qwe
            string username = "master001";
            string pwd = "46f94c8de14fb36680850768ff1b7f2a";
            string md5 = GetMd5Hash(pwd.ToLower() + "hydra");
            string pid = "2396926147ee02b652877fb5ca551f9b";
            string url = "http://183.131.69.236:11001/api/user/Register?" +
                         $"username={username}&userpwd={pwd}&pid={pid}";
            url = $"http://183.131.69.236:10101/api/Factory?ysfunction=mblogin&account={username}&password={pwd}";
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

        private string GetMd5Hash(string input)
        {
            if (input == null)
            {
                return null;
            }
            

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串 
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串 
            return sBuilder.ToString();
        }
    }
}
