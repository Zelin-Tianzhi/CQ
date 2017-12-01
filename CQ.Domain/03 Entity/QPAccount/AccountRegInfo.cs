namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountRegInfo")]
    public partial class AccountRegInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        [Required]
        [StringLength(32)]
        public string RegisterAddress { get; set; }

        [StringLength(64)]
        public string RegisterMac { get; set; }

        [StringLength(1024)]
        public string Details { get; set; }

        public DateTime? RegisterDate { get; set; }

        [StringLength(16)]
        public string RealName { get; set; }

        [StringLength(18)]
        public string IdentityCard { get; set; }

        [StringLength(32)]
        public string Telephone { get; set; }
    }
}
