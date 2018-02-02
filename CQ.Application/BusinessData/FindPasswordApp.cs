using CQ.Core;

namespace CQ.Application.BusinessData
{
    public class FindPasswordApp:BaseApp
    {
        #region 属性



        #endregion

        #region 公共方法

        public string SendCheckCode(string account)
        {
            string url = $"http://183.131.69.236:1860/?ysfunction=changepassword&type=1&changetype=1&account={account}";
            string msg = HttpMethods.HttpGet(url);
            var result = string.Empty;
            switch (msg)
            {
                case "-14":
                    result = "未绑定手机号码。";
                    break;
                case "-13":
                    result = "用户帐号错误。";
                    break;
                case "-6":
                    result = "验证码用户发送过于频繁或者超出限定数目。";
                    break;
                case "-999":
                    result = "验证码请求失败。";
                    break;
                case "0":
                    result = "0";
                    break;
            }
            return result;
        }

        public string SubmitModifyPwd(string account,string checkcode, string newpwd)
        {
            string url =
                $"http://183.131.69.236:1860/?ysfunction=changepassword&type=2&changetype=1&account={account}&yanzhengma={checkcode}&password={newpwd}";
            string msg = HttpMethods.HttpGet(url);
            var result = string.Empty;
            switch (msg)
            {
                case "-14":
                    result = "未绑定手机号码。";
                    break;
                case "-13":
                    result = "用户帐号错误。";
                    break;
                case "-6":
                    result = "验证码用户发送过于频繁或者超出限定数目。";
                    break;
                case "-999":
                    result = "验证码请求失败。";
                    break;
                case "0":
                    result = "0";
                    break;
                case "-16":
                    result = "新密码写入失败。";
                    break;
            }
            return result;

        }

        #endregion

        #region 私有方法

        

        #endregion
    }
}