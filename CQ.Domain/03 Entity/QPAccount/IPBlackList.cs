namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IPBlackList")]
    public partial class IPBlackList
    {
        [Key]
        [StringLength(50)]
        public string IP { get; set; }

        public DateTime Time { get; set; }

        public bool IsMac { get; set; }
    }
}
