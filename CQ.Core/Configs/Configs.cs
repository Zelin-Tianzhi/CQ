using System.Configuration;
using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace CQ.Core
{
    public class Configs
    {
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }

        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(HttpContext.Current.Server.MapPath("~/Configs/system.config"));
            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(HttpContext.Current.Server.MapPath("~/Configs/system.config"));
        }
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static T GetConfig<T>(string configPath) where T : class
        {
            T config;
            using (var fs = new FileStream(configPath,FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
            {
                var xs = new XmlSerializer(typeof(T));
                config = (T)xs.Deserialize(fs);
            }
            return config;
        }
    }
}