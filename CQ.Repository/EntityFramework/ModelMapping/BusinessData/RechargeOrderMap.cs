using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CQ.Domain.Entity.BusinessData;

namespace CQ.Repository.EntityFramework
{
    public class RechargeOrderMap: EntityTypeConfiguration<RechargeOrderEntity>
    {
        public RechargeOrderMap()
        {
            this.ToTable("Bus_RechargeOrder");
            this.Property(t => t.F_Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(t => t.F_Id);
        }
    }
}