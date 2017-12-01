using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Repository.EntityFramework
{
    public class ProduceMap: EntityTypeConfiguration<ProductEntity>
    {
        public ProduceMap()
        {
            this.ToTable("Bus_Products");
            this.Property(t => t.F_Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(t => t.F_Id);
        }
    }
}
