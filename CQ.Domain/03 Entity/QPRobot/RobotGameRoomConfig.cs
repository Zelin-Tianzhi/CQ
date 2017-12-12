namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotGameRoomConfig")]
    public partial class RobotGameRoomConfig
    {
        public int ID { get; set; }

        [Required]
        [StringLength(1024)]
        public string TimeText { get; set; }

        [Required]
        [StringLength(32)]
        public string roomname { get; set; }

        public int roomid { get; set; }
    }
}
