using CQ.Data.Repository;
using CQ.Domain.Entity.BusinessData;
using CQ.Domain.IRepository.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Repository.BusinessData
{
    public class ProductRepository: RepositoryBase<ProductEntity>, IProductRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ProductEntity>(t => t.F_Id == keyValue);
                db.Delete<ImageEntity>(t => t.F_FId == keyValue);
                db.Commit();
            }
        }

        public void SubmitForm(ProductEntity productEntity, List<ImageEntity> imageEntitys, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(productEntity);
                }
                else
                {
                    db.Insert(productEntity);
                }
                db.Delete<ImageEntity>(t => t.F_FId == productEntity.F_Id);
                db.Insert(imageEntitys);
                db.Commit();
            }
        }
    }
}
