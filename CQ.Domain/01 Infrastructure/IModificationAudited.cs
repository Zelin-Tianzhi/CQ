using System;

namespace CQ.Domain
{
    public interface IModificationAudited
    {
        long F_Id { get; set; }
        long? F_LastModifyUserId { get; set; }
        DateTime? F_LastModifyTime { get; set; }
    }
}
