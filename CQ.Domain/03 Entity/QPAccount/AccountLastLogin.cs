namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountLastLogin")]
    public partial class AccountLastLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        public int? OnlineTime { get; set; }

        [StringLength(32)]
        public string LastLoginIP { get; set; }

        [StringLength(32)]
        public string LastLoginMac { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime? LastLogoutTime { get; set; }
    }
}
