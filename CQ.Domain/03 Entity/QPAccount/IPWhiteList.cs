namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IPWhiteList")]
    public partial class IPWhiteList
    {
        [Key]
        [StringLength(50)]
        public string IP { get; set; }
    }
}
