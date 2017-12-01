/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;

namespace CQ.Domain.Entity.SystemManage
{
    public class ItemsEntity : IEntity<ItemsEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int F_Id { get; set; }
        public int F_ParentId { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        public bool? F_IsTree { get; set; }
        public int? F_Layers { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public int? F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public int? F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public int? F_DeleteUserId { get; set; }
    }
}
