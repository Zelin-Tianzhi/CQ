namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RbtNickNameLog")]
    public partial class RbtNickNameLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int AccountNum { get; set; }

        [Required]
        [StringLength(32)]
        public string NickName { get; set; }

        public int Result { get; set; }

        public DateTime? Time { get; set; }
    }
}
