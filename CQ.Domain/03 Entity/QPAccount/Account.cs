namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public int AccountID { get; set; }

        public int AccountNum { get; set; }

        [Column("Account")]
        [Required]
        [StringLength(32)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(32)]
        public string NickName { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [StringLength(32)]
        public string BankPassword { get; set; }

        public bool Sex { get; set; }

        public byte AccountType { get; set; }

        public byte AccountSecondType { get; set; }

        public int Charm { get; set; }

        public int? PhotoUUID { get; set; }

        public long Gold { get; set; }

        public long GoldBank { get; set; }

        public long Bean { get; set; }

        public long TotalExp { get; set; }

        public int TotalOnlineSecond { get; set; }

        public int YuanBao { get; set; }

        public long VipExp { get; set; }

        public int? SafeWay { get; set; }

        public int? OpenFunctionFlag { get; set; }
    }
}
