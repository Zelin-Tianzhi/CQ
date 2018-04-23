using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CQ.Core;
using CQ.Core.Log;
using CQ.WebApi.Application;
using CQ.WebApi.Models;

namespace CQ.WebApi.Controllers
{
    [AuthorizeAttrbute]
    public class FactoryController : ApiController
    {
        public Log Log
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IHttpActionResult GetFactory([FromUri]Parameters parameters)
        {
            Log.Debug(parameters.ToJson());
            string funName = parameters.ysfunction;
            if (!(funName == "onlineuser" || funName == "getuserdata" || funName == "getusercount"))
            {
                string sourceIP = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                if (!IsTrustAddress(sourceIP))
                {
                    return Json("不是信任的地址");
                }
            }
            OperationApp app = new OperationApp();
            dynamic result = string.Empty;
            switch (funName)
            {
                case "logout":
                    result = app.Logout(parameters);
                    break;
                case "userlogin":
                    result = app.UserLogin(parameters);
                    break;
                case "mblogin":
                    result = app.UserLoginVerify(parameters);
                    break;
                case "onlineuser":
                   result = app.GetOnlineUser(parameters);
                    break;
                case "getuserdata":
                   result= app.GetUserData(parameters);
                    break;
                case "getusercount":
                    result = app.GetAllUserCount(parameters);
                    break;
                case "register":
                   result= app.RegisterUser(parameters);
                    break;
                case "lockuser":
                   result= app.LockUser(parameters);
                    break;
                case "tuser":
                    result = app.TUser(parameters);
                    break;
                case "broad":
                    result = app.BroadCastInfo(parameters);
                    break;
                case "broadinfo":
                    result = app.GMBroadCastInfo(parameters);
                    break;
                case "chongzhi":
                    result = app.Chongzhi(parameters);
                    break;
                case "chongzhifunc":
                    result = app.Chongzhifunc(parameters);
                    break;
                case "chongzhifuncnotify":
                    result = app.ChongzhifuncNotify(parameters);
                    break;
                case "changepwd":
                    result = app.ChangePwd(parameters);
                    break;
                case "fangchengmi":
                    result = app.FangChenMi(parameters);
                    break;
                case "addprop":
                    result = app.AddPropToUser(parameters);
                    break;
                case "changeaccounttype":
                    result = app.ChangeAccountType(parameters);
                    break;
                case "resetbankpwd":
                    result = app.ResetBankPwd(parameters);
                    break;
                case "setshenfenzheng":
                    result = app.SetShenFenZheng(parameters);
                    break;
                case "changenicheng":
                    result = app.ChangeNicheng(parameters);
                    break;
                case "chongzhijinbi":
                    result = app.ChongzhiJinbi(parameters);
                    break;
                case "kouchujinbi":
                    result = app.KouChuJinbi(parameters);
                    break;
                case "delprop":
                    result = app.DelPropFromUser(parameters);
                    break;
                case "delpropnotify":
                    result = app.DelPropFromUserNotify(parameters);
                    break;
                case "deletemacbind":
                    result = app.DeleteMacBind(parameters);
                    break;
                case "startflopserver":
                    result = app.StartFlopServer(parameters);
                    break;
                case "deleteuserallprop":
                    result = app.DeleteUseAllProp(parameters);
                    break;
                case "deleteuserpropbyid":
                    result = app.DeleteUsePropByID(parameters);
                    break;
                case "deleteuserpropbytype":
                    result = app.DeleteUsePropByType(parameters);
                    break;
                case "setgameserver":
                    result = app.SetGameServer(parameters);
                    break;
                case "sendzhanneixin":
                    result = app.SendZhanNeiXin(parameters);
                    break;
                case "sendzhanneixintouser":
                    result = app.SendZhanNeiXinToUser(parameters);
                    break;
                case "reloadconfig":
                    result = app.ReloadConfig(parameters);
                    break;
                case "changerobotphoto":
                    result = app.ChangeRobotPhoto(parameters);
                    break;
                case "changerobotmember":
                    result = app.ChangeRobotMember(parameters);
                    break;
                case "addblacklist":
                    result = app.AddBlackList(parameters);
                    break;
                case "checkuserauth":
                    result = app.CheckUserAuth(parameters);
                    break;
                case "dorobotchelueid":
                    result = app.DoRobotChelueID(parameters);
                    break;
                case "clearmoney":
                    result = app.ClearMoney(parameters);
                    break;
                case "changeuserdetails":
                    result = app.ChangeUserDetails(parameters);
                    break;
                case "setphonenumber":
                    result = app.SetPhoneNumber(parameters);
                    break;
                case "setfunctionhyd":
                    result = app.SetFunctionHYD(parameters);
                    break;
                case "setclosetime":
                    result = app.SetCloseTime(parameters);
                    break;
                case "questionverify":
                    result = app.QuestionVerify(parameters);
                    break;
                default:
                    result = "-404";
                    break;
            }

            return Json(result);
        }

        public IHttpActionResult Get(int id)
        {
            return Ok(id);
        }


        private bool IsTrustAddress(string ip)
        {
            if (ip == "127.0.0.1")
            {
                return true;
            }
            return true;
        }
    }
}
