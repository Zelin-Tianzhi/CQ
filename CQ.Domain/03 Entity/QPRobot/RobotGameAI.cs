namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotGameAI")]
    public partial class RobotGameAI
    {
        public int ID { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(64)]
        public string GameName { get; set; }

        [StringLength(2048)]
        public string AIText { get; set; }
    }
}
