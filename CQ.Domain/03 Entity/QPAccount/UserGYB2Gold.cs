namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserGYB2Gold
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserGBYID { get; set; }

        public int AccountID { get; set; }

        public int GYBType { get; set; }

        public long GYBValue { get; set; }

        public long MoneyGet { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
