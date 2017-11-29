using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Repository.EntityFramework
{
    public class ImageMap: EntityTypeConfiguration<ImageEntity>
    {
        public ImageMap()
        {
            this.ToTable("Bus_Images");
            this.HasKey(t => t.F_Id);
        }
    }
}
