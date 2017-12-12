namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotPropSpecialItemAI")]
    public partial class RobotPropSpecialItemAI
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int propchelueid { get; set; }

        public int aoyungailv { get; set; }

        public int aoyunqichegailv { get; set; }

        public int zhongzigailv { get; set; }

        [Required]
        [StringLength(10)]
        public string zhongziqichegailv { get; set; }

        public int majianggailv { get; set; }

        public int majiangqichegailv { get; set; }
    }
}
