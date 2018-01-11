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
        private ImageApp imageApp = new ImageApp();
        public List<ProductEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<ProductEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ProductName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }
        public List<ProductEntity> GetList()
        {
            var expression = ExtLinq.True<ProductEntity>();
            Pagination pagination = new Pagination
            {
                page = 1,
                rows = 20,
                sord = "desc",
                sidx = "F_CreatorTime,F_Id desc"
            };
            return service.FindList(expression, pagination);
        }
        public ProductEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue.ToInt());
        }
        public void DeleteForm(int keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(ProductEntity produceEntity, string[] imgPaths, int keyValue)
        {
            if (keyValue > 0)
            {
                produceEntity.F_Id = keyValue;
            }
            List<ImageEntity> imageEntitys = new List<ImageEntity>();
            foreach (string item in imgPaths)
            {
                ImageEntity imageEntity = new ImageEntity();
                imageEntity.F_Img = item;
                imageEntity.F_FId = produceEntity.F_Id;
                imageEntitys.Add(imageEntity);
            }
            service.SubmitForm(produceEntity, imageEntitys, keyValue);
        }
    }
}
