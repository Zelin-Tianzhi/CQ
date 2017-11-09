using CQ.Core;
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
    public class ProductApp
    {
        private IProductRepository service = new ProductRepository();
        public List<ProductEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<ProductEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ProductName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }
        public ProductEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(ProductEntity areaEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                areaEntity.Modify(keyValue);
                service.Update(areaEntity);
            }
            else
            {
                service.Insert(areaEntity);
            }
        }
    }
}
