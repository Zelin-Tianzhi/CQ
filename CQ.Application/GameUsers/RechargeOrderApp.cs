using System.Collections.Generic;
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
                var begintime = queryParam["begintime"].ToString();
                expression = expression.And(t => t.F_CreatorTime >= begintime.ToDate());
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                var endtime = queryParam["endtime"].ToString();
                expression = expression.And(t => t.F_CreatorTime <= endtime.ToDate());
            }
            return service.FindList(expression, pagination);
        }

        #endregion

        #region 私有方法



        #endregion
    }
}