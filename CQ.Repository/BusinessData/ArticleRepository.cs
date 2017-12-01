
using CQ.Domain.Entity.BusinessData;
using CQ.Domain.IRepository.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQ.Repository.EntityFramework;

namespace CQ.Repository.BusinessData
{
    public class ArticleRepository : RepositoryBase<ArticleEntity>, IArticleRepository
    {
        public ArticleRepository()
        {
            dbcontext = new CqDbContext();
        }
    }
}
