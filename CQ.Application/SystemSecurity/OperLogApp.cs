using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CQ.Core;
using CQ.Domain.Entity.SystemSecurity;
using CQ.Domain.IRepository.SystemSecurity;
using CQ.Repository.SystemSecurity;

namespace CQ.Application.SystemSecurity
{
    public class OperLogApp : BaseApp
    {
        #region 属性

        private IOperLogRepository service = new OperLogRepository();

        #endregion

        #region 公共方法

        public List<OperLogEntity> GetList(Pagination pagination, string queryJson,int type)
        {
            var expression = ExtLinq.True<OperLogEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Account.Contains(keyword));
            }
            if (!queryParam["begintime"].IsEmpty())
            {
                var begintime = queryParam["begintime"].ToString().ToDate();
                expression = expression.And(t=> t.F_CreatorTime >= begintime);
            }
            if (!queryParam["endtime"].IsEmpty())
            {
                var endtime = queryParam["endtime"].ToString().ToDate();
                expression = expression.And(t => t.F_CreatorTime <= endtime);
            }
            if (type == 1)
            {
                expression = expression.And(t => t.F_Type == 1 || t.F_Type == 2);
            }
            else if (type == 2)
            {
                expression = expression.And(t => t.F_Type == 5);
            }
            else if (type == 3)
            {
                expression = expression.And(t => t.F_Type == 9);
            }
            return service.FindList(expression, pagination);
        }
        /// <summary>
        /// 记录管理员操作日志
        /// </summary>
        /// <param name="operLogEntity"></param>
        /// <returns></returns>
        public int WriteLog(OperLogEntity operLogEntity)
        {
            operLogEntity.Create();
            operLogEntity.F_IpAddress = Net.Ip;
            int rows = service.Insert(operLogEntity);
            return rows;
        }

        #endregion

        #region 私有方法



        #endregion
    }
}