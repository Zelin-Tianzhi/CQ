namespace CQ.Domain.Entity.QPRobot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class robotchangegold3
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short GameID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime CreationDate { get; set; }

        public long GoldAdd { get; set; }
    }
}
