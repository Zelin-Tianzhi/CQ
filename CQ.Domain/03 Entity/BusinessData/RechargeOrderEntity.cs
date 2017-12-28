using System;
using System.ComponentModel.DataAnnotations;

namespace CQ.Domain.Entity.BusinessData
{
    public class RechargeOrderEntity : IEntity<RechargeOrderEntity>, ICreationAudited
    {
        public int F_Id { get; set; }
        [StringLength(50)]
        public string F_Account { get; set; }
        [StringLength(50)]
        public string F_OrderNo { get; set; }
        public long F_Amounts { get; set; }
        public long F_Gold { get; set; }
        [StringLength(50)]
        public string F_IpAddress { get; set; }
        public int F_State { get; set; }
        public int F_Mode { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public int? F_CreatorUserId { get; set; }
    }
}