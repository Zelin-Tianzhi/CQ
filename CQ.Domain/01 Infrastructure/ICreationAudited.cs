using System;

namespace CQ.Domain
{
    public interface ICreationAudited
    {
        int F_Id { get; set; }
        int? F_CreatorUserId { get; set; }
        DateTime? F_CreatorTime { get; set; }
    }
}
