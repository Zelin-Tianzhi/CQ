using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Mapping.BusinessData
{
    public class ArticleMap: EntityTypeConfiguration<ArticleEntity>
    {
        public ArticleMap()
        {
            this.ToTable("Bus_Articles");
            this.HasKey(t => t.F_Id);
        }
    }
}
