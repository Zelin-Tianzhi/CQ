using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using CQ.Core;
using CQ.Domain.Entity.BusinessData;
using CQ.Domain.Entity.SystemSecurity;
using CQ.Domain.IRepository.BusinessData;
using CQ.Repository.BusinessData;
using CQ.Repository.EntityFramework;

namespace CQ.Application.GameUsers
{
    public class RechargeOrderApp : BaseApp
    {
        #region 属性

        private IRechargeOrderRepository service = new RechargeOrderRepository();
        private readonly DbHelper _qpAccount = new DbHelper("QpAccount");

        #endregion

        #region 公共方法

        public List<RechargeOrderEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<RechargeOrderEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Account.Contains(keyword));
            }
            if (!queryParam["begintime"].IsEmpty())
            {
                var begintime = queryParam["begintime"].ToString().ToDate();
                expression = expression.And(t => t.F_CreatorTime >= begintime);
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                var endtime = queryParam["endtime"].ToString().ToDate();
                expression = expression.And(t => t.F_CreatorTime <= endtime);
            }
            return service.FindList(expression, pagination);
        }

        public int SubmitEntity(string userNum, long amounts, string czType, string keyValue)
        {
            RechargeOrderEntity entity = new RechargeOrderEntity();
            var userId = GetIdByNum(keyValue, 0);
            var userName = GetIdByNum(keyValue, 2);
            entity.Create();
            entity.F_AccountId = userId.ToInt64();
            entity.F_IpAddress = Net.Ip;
            entity.F_Account = userName;
            entity.F_Amounts = amounts;
            entity.F_State = 9;
            var orderNo = GetNewOrderNo(keyValue);
            entity.F_OrderNo = orderNo;
            var rows = service.Insert(entity);
            var result = RechargeYb(userName, entity.F_Amounts, 5, 1, 1);
            //ChongzhiNotify(userName, amounts, 5, 1, 0);
            if (result.Trim() == "0" && rows > 0)
            {
                var expression = ExtLinq.True<RechargeOrderEntity>();
                expression = expression.And(t => t.F_OrderNo == orderNo);
                var model = service.FindEntity(expression);
                model.F_State = 10;
                rows = service.Update(model);
            }
            return result.ToInt();
        }

        public string GetNewOrderNo(string usernum)
        {
            List<char> last = usernum.Remove(0, usernum.Length - 4).ToList();
            List<char> frist = usernum.Substring(0, 4).ToList();
            string newLast = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                int temp = last[i].ToInt() | frist[i].ToInt();
                newLast += temp.ToString();
            }
            newLast = newLast.PadLeft(4, '0');
            newLast = newLast.Remove(0, newLast.Length - 4);
            var unixtime = Common.GetCurrentTimeUnix().ToString();
            Random r = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var num1 = r.Next(1, 9);
            var num2 = r.Next(1, 9);
            var timeStr = unixtime.Remove(0, unixtime.Length - 9);
            var orderNo = num1 + "" + num2 + timeStr + newLast;
            return orderNo;
        }

        public string GetIdByNum(string account, int type)
        {
            var sql = string.Empty;// 
            switch (type)
            {
                case 0:
                    sql = $"select AccountID from Account where Accountnum={account}";
                    break;
                case 1:
                    sql = $"select Account from Account where AccountID={account}";
                    break;
                case 2:
                    sql = $"select Account from Account where AccountNum={account}";
                    break;
                case 3:
                    sql = $"select Accountnum from Account where Account={account}";
                    break;
            }
            var obj = _qpAccount.GetObject(sql, null);
            return obj?.ToString() ?? "0";
        }

        #endregion

        #region 私有方法



        private string RechargeYb(string account, long values, int type, int opertype, int nosendmail)
        {
            string url = GetUrlStr() +
                         $"ysfunction=chongzhifunc&account={account}&values={values}&type={type}&opertype={opertype}&nosendmail={nosendmail}";
            string msg = HttpMethods.HttpGet(url);
            return msg;
        }

        public string ChongzhiNotify(string account, long values, int type, int opertype, int nosendmail)
        {
            string url = GetUrlStr() +
                         $"ysfunction=chongzhifuncnotify&account={account}&values={values}&type={type}&opertype={opertype}&nosendmail={nosendmail}s";
            string msg = HttpMethods.HttpGet(url);
            return msg;
        }

        #endregion
    }
}