namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotAccount")]
    public partial class RobotAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        [Required]
        [StringLength(64)]
        public string Account { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        [Required]
        [StringLength(64)]
        public string GroupName { get; set; }

        public int State { get; set; }

        public int? RoomAIID { get; set; }

        public int? GameAIID { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        public int? gameroomconfigid { get; set; }
    }
}
