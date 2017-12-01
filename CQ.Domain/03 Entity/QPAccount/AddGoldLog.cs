namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AddGoldLog")]
    public partial class AddGoldLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddGoldLogID { get; set; }

        [Required]
        [StringLength(32)]
        public string account { get; set; }

        public DateTime CreateTime { get; set; }

        public long Gold { get; set; }
    }
}
