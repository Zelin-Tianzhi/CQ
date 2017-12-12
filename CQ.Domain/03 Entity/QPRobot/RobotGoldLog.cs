namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotGoldLog")]
    public partial class RobotGoldLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string RobotAccount { get; set; }

        public long ChangeMoney { get; set; }

        public DateTime? Timer { get; set; }
    }
}
