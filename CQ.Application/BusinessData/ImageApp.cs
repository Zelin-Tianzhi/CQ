using CQ.Domain.Entity.BusinessData;
using CQ.Domain.IRepository.BusinessData;
using CQ.Repository.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Application.BusinessData
{
    public class ImageApp
    {
        private IImagesRepository service = new ImageRepository();

        public void SubmitForm(ImageEntity imageEntity, int keyValue)
        {
            if (keyValue > 0)
            {
                imageEntity.Modify(keyValue);
                service.Update(imageEntity);
            }
            else
            {
                imageEntity.Create();
                service.Insert(imageEntity);
            }
        }

        public List<ImageEntity> GetList(int keyValue)
        {
            return service.IQueryable().Where(t=> t.F_FId == keyValue).ToList();
        }
        public void DeleteForm(int keyValue)
        {
            service.Delete(t => t.F_FId == keyValue);
        }

        public void SubmitForm(List<ImageEntity> list)
        {
            service.Insert(list);
        }
    }
}
