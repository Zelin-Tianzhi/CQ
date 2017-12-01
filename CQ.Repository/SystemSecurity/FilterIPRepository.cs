/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;
using CQ.Domain.Entity.SystemSecurity;
using CQ.Domain.IRepository.SystemSecurity;
using CQ.Repository.EntityFramework;

namespace CQ.Repository.SystemSecurity
{
    public class FilterIPRepository : RepositoryBase<FilterIPEntity>, IFilterIPRepository
    {
        public FilterIPRepository()
        {
            dbcontext = new CqDbContext();
        }
    }
}
