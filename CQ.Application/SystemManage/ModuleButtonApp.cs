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
    public class ModuleButtonApp
    {
        private IModuleButtonRepository service = new ModuleButtonRepository();

        public List<ModuleButtonEntity> GetList(int moduleId = 0)
        {
            var expression = ExtLinq.True<ModuleButtonEntity>();
            if (moduleId > 0)
            {
                expression = expression.And(t => t.F_ModuleId == moduleId);
            }
            return service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleButtonEntity GetForm(string keyValue)
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
        public void SubmitForm(ModuleButtonEntity moduleButtonEntity, int keyValue)
        {
            if (keyValue > 0)
            {
                moduleButtonEntity.Modify(keyValue);
                service.Update(moduleButtonEntity);
            }
            else
            {
                moduleButtonEntity.Create();
                service.Insert(moduleButtonEntity);
            }
        }
        public void SubmitCloneButton(int moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data = this.GetList();
            List<ModuleButtonEntity> entitys = new List<ModuleButtonEntity>();
            foreach (string item in ArrayId)
            {
                ModuleButtonEntity moduleButtonEntity = data.Find(t => t.F_Id == item.ToInt64());
                moduleButtonEntity.F_ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            service.SubmitCloneButton(entitys);
        }
    }
}
