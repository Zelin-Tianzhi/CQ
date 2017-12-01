/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CQ.Domain.Entity.SystemManage;

namespace CQ.Repository.EntityFramework
{
    public class UserMap : EntityTypeConfiguration<UserEntity>
    {
        public UserMap()
        {
            this.ToTable("Sys_User");
            this.Property(t => t.F_Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(t => t.F_Id);
        }
    }
}
