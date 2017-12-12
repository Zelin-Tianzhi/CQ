namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotRoomAI")]
    public partial class RobotRoomAI
    {
        public int ID { get; set; }

        public int LoginRoomRate { get; set; }

        public int LeaveRoomRate { get; set; }

        public int InRoomMinTime { get; set; }

        public int InRoomMaxTime { get; set; }

        [Required]
        [StringLength(256)]
        public string AIText { get; set; }

        [Required]
        [StringLength(256)]
        public string PriorityTable { get; set; }

        [StringLength(256)]
        public string RoomName { get; set; }

        [Column(TypeName = "text")]
        public string Config { get; set; }
    }
}
