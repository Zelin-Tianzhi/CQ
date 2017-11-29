using System.Collections.Generic;
using CQ.Domain.Entity.SystemManage;

namespace CQ.Domain.IRepository.SystemManage
{
    public interface IRoleRepository : IRepositoryBase<RoleEntity>
    {
        void DeleteForm(long keyValue);
        void SubmitForm(RoleEntity roleEntity, List<RoleAuthorizeEntity> roleAuthorizeEntitys, long keyValue);
    }
}