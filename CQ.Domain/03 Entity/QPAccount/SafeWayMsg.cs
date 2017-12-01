namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SafeWayMsg")]
    public partial class SafeWayMsg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
