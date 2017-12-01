
using CQ.Domain.Entity.QPAccount;
using CQ.Domain.IRepository.QPAccount;
using CQ.Repository.EntityFramework;

namespace CQ.Repository.QPAccount
{
    public class AccountRepository : RepositoryBase<Account>,IAccountRepository
    {
        public AccountRepository()
        {
            dbcontext = new QpAccount();
        }
    }
}