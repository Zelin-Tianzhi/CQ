
using CQ.Domain.Entity.SystemManage;

namespace CQ.Domain.IRepository.SystemManage
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        void DeleteForm(long keyValue);
        void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, long keyValue);
    }
}