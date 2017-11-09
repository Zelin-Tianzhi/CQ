using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Domain.Entity.BusinessData
{
    public class ProductEntity : IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ProductName { get; set; }
        public bool F_IsHot { get; set; }
        public string F_ProductImg { get; set; }
        public string F_Explain { get; set; }
        public string F_Rule { get; set; }
        public int F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool F_EnableMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public string F_Remark { get; set; }
    }
}
