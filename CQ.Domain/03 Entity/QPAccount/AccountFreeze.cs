namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountFreeze")]
    public partial class AccountFreeze
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        public byte? FreezeType { get; set; }

        public DateTime? UnfreezeDate { get; set; }

        [Column(TypeName = "text")]
        public string LockMessage { get; set; }
    }
}
