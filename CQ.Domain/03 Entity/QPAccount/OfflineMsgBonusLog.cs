namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OfflineMsgBonusLog")]
    public partial class OfflineMsgBonusLog
    {
        public int ID { get; set; }

        public int MsgID { get; set; }

        public int AccountID { get; set; }

        public int TargetAccountID { get; set; }

        public int OperType { get; set; }

        [Required]
        [StringLength(1024)]
        public string Message { get; set; }

        public int BonusID { get; set; }

        public DateTime SendBonusTime { get; set; }
    }
}
