namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAdditionDescribe")]
    public partial class UserAdditionDescribe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolutionID { get; set; }

        public int KeyDefine { get; set; }

        public int MaskDefine { get; set; }

        public int MultipleValues { get; set; }

        public int AdditionBonusID { get; set; }
    }
}
