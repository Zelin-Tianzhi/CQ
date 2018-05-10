using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using CQ.Application.GameUsers;
using CQ.Core;
using CQ.Plugin.Cache.Redis;
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
            app.ModifyUserInfo("204243", "15696963698", "330101199602121254", "阿萨德", "1");
            //app.BindInfo(HttpUtility.UrlDecode("8edf96b8-6ff0-b768-5d7b-5366efdf2220%40dwc"), "15996969696", "46f94c8de14fb36680850768ff1b7f2a",
            //    "159ZZH", "2");
        }

        [TestMethod]
        public void xmloper()
        {
            List<RedisConfig>  aa = new List<RedisConfig>();
            List<RedisConfig> config = new List<RedisConfig>();
            config.Add(new RedisConfig
            {
                Host = "127.0.0.1",
                Prot = 6379,
                Password = "123"
            });
            string configPath = FileHelper.MapPath("/Configs/Redis.config");
            var xs = new XmlSerializer(typeof(List<RedisConfig>));
            Stream st = File.OpenWrite(configPath);

            //config = (RedisConfig)xs.Deserialize(fs);
            xs.Serialize(st, config);
            st.Close();
            Stream ss = File.OpenRead(configPath);
            aa = (List<RedisConfig>)xs.Deserialize(ss);
            ss.Close();
        }
    }
}