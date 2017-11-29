
using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Domain.IRepository.BusinessData
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        void DeleteForm(long keyValue);
        void SubmitForm(ProductEntity productEntity, List<ImageEntity> imageList, long keyValue);
    }
}
