using System;

namespace CQ.Domain.Entity.SystemSecurity
{
    public class OperLogEntity : IEntity<OperLogEntity>, ICreationAudited
    {
        public int F_Id { get; set; }
        public string F_Account { get; set; }
        public string F_TextValue { get; set; }
        public string F_Type { get; set; }
        public string F_IpAddress { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public int? F_CreatorUserId { get; set; }
    }
}