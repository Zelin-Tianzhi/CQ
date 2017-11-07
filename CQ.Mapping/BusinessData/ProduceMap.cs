using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Mapping.BusinessData
{
    public class ProduceMap: EntityTypeConfiguration<ProductEntity>
    {
        public ProduceMap()
        {
            this.ToTable("Bus_Products");
            this.HasKey(t => t.F_Id);
        }
    }
}
