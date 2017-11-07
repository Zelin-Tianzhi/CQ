using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQ.Data.Repository;
using CQ.Domain.Entity.SystemManage;

namespace CQ.Domain.IRepository.SystemManage
{
    public interface IModuleButtonRepository : IRepositoryBase<ModuleButtonEntity>
    {
        void SubmitCloneButton(List<ModuleButtonEntity> entitys);
    }
}
