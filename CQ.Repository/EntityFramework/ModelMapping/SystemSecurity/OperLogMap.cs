using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CQ.Domain.Entity.SystemSecurity;

namespace CQ.Repository.EntityFramework
{
    public class OperLogMap: EntityTypeConfiguration<OperLogEntity>
    {
        public OperLogMap()
        {
            this.ToTable("Sys_OperLog");
            this.Property(t => t.F_Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(t => t.F_Id);
        }
    }
}