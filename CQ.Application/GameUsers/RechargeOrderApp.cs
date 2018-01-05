using System;
using System.Collections.Generic;
using System.Linq;
using CQ.Core;
using CQ.Domain.Entity.BusinessData;
using CQ.Domain.Entity.SystemSecurity;
using CQ.Domain.IRepository.BusinessData;
using CQ.Repository.BusinessData;

namespace CQ.Application.GameUsers
{
    public class RechargeOrderApp
    {
        #region 属性

        private IRechargeOrderRepository service = new RechargeOrderRepository();

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

        public int SubmitEntity(RechargeOrderEntity entity, string keyValue)
        {
            entity.Create();
            entity.F_AccountId = keyValue.ToInt64();
            entity.F_IpAddress = Net.Ip;
            entity.F_State = 9;
            entity.F_Mode = 10;

            return 0;
        }

        public string GetNewOrderNo(string usernum)
        {
            List<char> last = usernum.Remove(0, usernum.Length - 4).ToList();
            List<char> frist = usernum.Substring(0, 4).ToList();
            string newLast = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                int temp = last[i].ToInt() & frist[i].ToInt();
                newLast += temp.ToString();
            }
            newLast = newLast.PadLeft(4, '0').Substring(0, 4);
            var unixtime = Common.GetCurrentTimeUnix();
            unixtime = unixtime >> 2;
            string orderNo = String.Empty;
            var id = Guid.NewGuid().ToString().Replace("-", "");

            return id;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}