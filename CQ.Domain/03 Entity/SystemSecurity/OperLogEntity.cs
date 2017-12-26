using System;
using System.ComponentModel.DataAnnotations;

namespace CQ.Domain.Entity.SystemSecurity
{
    public class OperLogEntity : IEntity<OperLogEntity>, ICreationAudited
    {
        public int F_Id { get; set; }
        [StringLength(40)]
        public string F_Account { get; set; }
        [StringLength(100)]
        public string F_TextValue { get; set; }
        [StringLength(50)]
        public string F_Type { get; set; }
        [StringLength(50)]
        public string F_IpAddress { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public int? F_CreatorUserId { get; set; }
    }
}