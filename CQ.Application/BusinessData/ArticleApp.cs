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
    public class ArticleApp
    {
        private IArticleRepository service = new ArticleRepository();

        public List<ArticleEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<ArticleEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ArticleTitle.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public List<ArticleEntity> GetList(int articleType)
        {
            var expression = ExtLinq.True<ArticleEntity>();
            if (articleType != 0)
                if (articleType == 99)
                {
                    expression = expression.And(t => t.F_IsHot == true);
                }
                else
                {
                    expression = expression.And(t => t.F_ArticleType == articleType);
                }
            Pagination pagination = new Pagination
            {
                page = 1,
                rows = 20,
                sord = "desc",
                sidx = "F_PublishTime desc"

            };
            return service.FindList(expression, pagination);
        }

        public ArticleEntity GetForm(int keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(int keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(ArticleEntity areaEntity, int keyValue = 0)
        {
            if (keyValue > 0)
            {
                areaEntity.Modify(keyValue);
                service.Update(areaEntity);
            }
            else
            {
                areaEntity.Create();
                service.Insert(areaEntity);
            }
        }
    }
}
