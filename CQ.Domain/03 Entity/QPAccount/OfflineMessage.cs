namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OfflineMessage")]
    public partial class OfflineMessage
    {
        public int ID { get; set; }

        public int AccountID { get; set; }

        public int TargetAccountID { get; set; }

        public int OperType { get; set; }

        [Required]
        [StringLength(1024)]
        public string Message { get; set; }

        public DateTime CreateDate { get; set; }

        public int IsRead { get; set; }

        public int BonusID { get; set; }

        public int AccountType { get; set; }

        public int AccountSecondType { get; set; }

        public int Member { get; set; }
    }
}
