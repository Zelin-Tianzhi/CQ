namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RobotPropItemAI")]
    public partial class RobotPropItemAI
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int propchelueid { get; set; }

        public int propid { get; set; }

        public int propgailv { get; set; }

        public int assembletype { get; set; }

        public int? AssemblePos { get; set; }
    }
}
