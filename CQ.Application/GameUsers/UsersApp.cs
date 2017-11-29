using System;
using System.Data;
using System.Text;
using CQ.Core;
using CQ.Core.Security;
using CQ.Plugin.Cache;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class UsersApp : BaseApp
    {
        public string MemberRegister(string username, string userpwd, string macaddress)
        {
            DbHelper helper = new DbHelper("QpAccount");

            //解密获得mac地址
            int len;
            len = macaddress.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(macaddress.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte) i;
            }

            byte[] mingwen = YSEncrypt.DecryptData(inputByteArray);

            //获取当前最大
            string sqlMaxNum = "select MAX(AccountNum) from Account";
            DataSet dsMaxNum = helper.GetDataTablebySQL(sqlMaxNum);
            int num = 100000;
            if (dsMaxNum.Tables[0].Rows.Count > 0)
            {
                num = dsMaxNum.Tables[0].Rows[0][0].ToInt() + 1;
            }

            string str = Encoding.ASCII.GetString(mingwen);
            string account = username;
            string password = userpwd;
            string accounttype = "11";
            string accountsecondtype = "0";
            string sex = "2";
            string nickname = "新手" + num;
            string accountnum = num.ToString();
            string ipaddress = Net.Ip;
            string mac = str;
            string details = "|||0|0|||||||";
            //|密保问题|密保答案|年龄|身高cm|学历|生肖|星座|职业|省|市|
            //string[] userInfo = details.Split('|');
            //string mbwt = userInfo[1];
            //string mbda = userInfo[2];
            //string age = userInfo[3];
            //string sg = userInfo[4];
            //string xl = userInfo[5];
            //string sx = userInfo[6];
            //string xz = userInfo[7];
            //string zy = userInfo[8];
            //string sheng = userInfo[9];
            //string shi = userInfo[10];
            return "";
        }

        private int GetMaxUserId()
        {
            DbHelper helper = new DbHelper("QpAccount");
            int Id = 1;
            try
            {
                var maxId = Cache.Get("MaxUserId");
                if (maxId == null)
                {
                    string maxUserIdSql = "select max(accountnum) from account";

                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return 1;
        }
    }
}