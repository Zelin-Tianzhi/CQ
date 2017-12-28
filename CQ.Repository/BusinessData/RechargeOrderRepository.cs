using CQ.Domain.Entity.BusinessData;
using CQ.Domain.IRepository.BusinessData;
using CQ.Repository.EntityFramework;

namespace CQ.Repository.BusinessData
{
    public class RechargeOrderRepository:RepositoryBase<RechargeOrderEntity>,IRechargeOrderRepository
    {
        public RechargeOrderRepository()
        {
            dbcontext=new CqDbContext();
        }
    }
}