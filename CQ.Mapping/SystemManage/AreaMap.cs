/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;
using System.Data.Entity.ModelConfiguration;
using CQ.Domain.Entity.SystemManage;

namespace CQ.Mapping.SystemManage
{
    public class AreaMap : EntityTypeConfiguration<AreaEntity>
    {
        public AreaMap()
        {
            this.ToTable("Sys_Area");
            this.HasKey(t => t.F_Id);
        }
    }
}
