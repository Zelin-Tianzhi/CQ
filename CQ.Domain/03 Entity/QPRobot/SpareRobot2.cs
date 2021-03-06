namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SpareRobot2
    {
        [Key]
        [StringLength(50)]
        public string Account { get; set; }

        [Required]
        [StringLength(50)]
        public string NikeName { get; set; }

        [StringLength(32)]
        public string PassWord { get; set; }

        public int AccountNum { get; set; }

        public int? AccountID { get; set; }
    }
}
