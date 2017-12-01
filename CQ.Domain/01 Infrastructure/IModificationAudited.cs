using System;

namespace CQ.Domain
{
    public interface IModificationAudited
    {
        int F_Id { get; set; }
        int? F_LastModifyUserId { get; set; }
        DateTime? F_LastModifyTime { get; set; }
    }
}
