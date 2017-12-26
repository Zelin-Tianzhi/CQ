using CQ.Domain.Entity.SystemSecurity;
using CQ.Domain.IRepository.SystemSecurity;
using CQ.Repository.EntityFramework;

namespace CQ.Repository.SystemSecurity
{
    public class OperLogRepository:RepositoryBase<OperLogEntity>,IOperLogRepository
    {
        public OperLogRepository()
        {
            dbcontext = new CqDbContext();
        }
    }
}