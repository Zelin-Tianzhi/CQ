using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Domain.Entity.BusinessData
{
    public class ArticleEntity : IEntity<ArticleEntity>,ICreationAudited,IDeleteAudited,IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ArticleTitle { get; set; }
        public int? F_ArticleType { get; set; }
        public DateTime? F_PublishTime { get; set; }
        public string F_ArticleContent { get; set; }
        public int F_SortCode { get; set; }
        public bool F_IsHot { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool F_EnableMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }

    }
}
