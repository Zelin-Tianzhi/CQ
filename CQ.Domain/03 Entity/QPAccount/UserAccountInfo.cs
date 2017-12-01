namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAccountInfo")]
    public partial class UserAccountInfo
    {
        public int AccountID { get; set; }

        public int AccountNum { get; set; }

        [Key]
        [StringLength(32)]
        public string NickName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? ChangeNickNameTime { get; set; }

        public int? ExtraInfo { get; set; }

        public long? TotalActivityScore { get; set; }

        public int? FamilyID { get; set; }

        public DateTime? MemberExpiryTime { get; set; }
    }
}
