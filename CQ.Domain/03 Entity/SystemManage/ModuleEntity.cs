/*******************************************************************************
 * Copyright © 2017 Zl 版权所有
 * Author: Zl
 * Description: Tz通用权限
*********************************************************************************/
using System;

namespace CQ.Domain.Entity.SystemManage
{
    public class ModuleEntity : IEntity<ModuleEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public long F_Id { get; set; }
        public long F_ParentId { get; set; }
        public int? F_Layers { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        public string F_Icon { get; set; }
        public string F_UrlAddress { get; set; }
        public string F_Target { get; set; }
        public bool? F_IsMenu { get; set; }
        public bool? F_IsExpand { get; set; }
        public bool? F_IsPublic { get; set; }
        public bool? F_AllowEdit { get; set; }
        public bool? F_AllowDelete { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public long? F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public long? F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public long? F_DeleteUserId { get; set; }
    }
}
