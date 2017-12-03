using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQ.Core;
using CQ.Domain.Entity.SystemManage;
using CQ.Domain.IRepository.SystemManage;
using CQ.Repository.SystemManage;

namespace CQ.Application.SystemManage
{
    public class ModuleApp
    {
        private IModuleRepository service = new ModuleRepository();

        public List<ModuleEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue.ToInt());
        }
        public void DeleteForm(int keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(ModuleEntity moduleEntity, int keyValue)
        {
            if (keyValue > 0)
            {
                moduleEntity.Modify(keyValue);
                service.Update(moduleEntity);
            }
            else
            {
                moduleEntity.Create();
                service.Insert(moduleEntity);
            }
        }
    }
}
