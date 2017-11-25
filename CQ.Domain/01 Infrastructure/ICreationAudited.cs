using System;

namespace CQ.Domain
{
    public interface ICreationAudited
    {
        long F_Id { get; set; }
        long? F_CreatorUserId { get; set; }
        DateTime? F_CreatorTime { get; set; }
    }
}
