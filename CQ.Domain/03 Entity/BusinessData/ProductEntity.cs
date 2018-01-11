using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Domain.Entity.BusinessData
{
    public class ProductEntity : IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int F_Id { get; set; }
        public string F_ProductName { get; set; }
        public bool F_IsHot { get; set; }
        [StringLength(200)]
        public string F_IcoImg { get; set; }
        public string F_Explain { get; set; }
        [Column(TypeName = "text")]
        public string F_Rule { get; set; }
        public int F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool F_EnableMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public int? F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public int? F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public int? F_DeleteUserId { get; set; }
        public string F_Remark { get; set; }
    }
}
