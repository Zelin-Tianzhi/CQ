using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Domain.Entity.BusinessData
{
    public class ImageEntity : IEntity<ImageEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public int F_Category { get; set; }
        public string F_FId { get; set; }
        public string F_Thumb { get; set; }
        public string F_Img { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
    }
}
