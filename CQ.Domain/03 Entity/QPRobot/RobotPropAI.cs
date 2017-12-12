namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotPropAI")]
    public partial class RobotPropAI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int propchelueid { get; set; }

        [Required]
        [StringLength(128)]
        public string cheluename { get; set; }

        public int huiyuan { get; set; }

        public int propinfo { get; set; }

        public int yifuxiezi { get; set; }

        public int faxing { get; set; }

        public int chongwu { get; set; }

        public int shoubiao { get; set; }

        public int xianglian { get; set; }

        public int jiezhi { get; set; }

        public int qiche { get; set; }

        public int special { get; set; }
    }
}
