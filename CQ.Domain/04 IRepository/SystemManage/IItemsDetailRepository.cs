using System.Collections.Generic;
using CQ.Domain.Entity.SystemManage;

namespace CQ.Domain.IRepository.SystemManage
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetailEntity>
    {
        List<ItemsDetailEntity> GetItemList(string enCode);
    }
}
