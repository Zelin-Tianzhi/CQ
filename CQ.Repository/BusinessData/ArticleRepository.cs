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
    public class ArticleRepository : RepositoryBase<ArticleEntity>, IArticleRepository
    {
    }
}
