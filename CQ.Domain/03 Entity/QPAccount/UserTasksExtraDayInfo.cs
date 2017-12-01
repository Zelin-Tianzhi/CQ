namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserTasksExtraDayInfo")]
    public partial class UserTasksExtraDayInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        public DateTime? OnLineDate { get; set; }

        [Column(TypeName = "text")]
        public string UserDayInfo { get; set; }

        [Column(TypeName = "text")]
        public string UserGlobalInfo { get; set; }
    }
}
