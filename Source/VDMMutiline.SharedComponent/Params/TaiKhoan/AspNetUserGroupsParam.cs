using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetUserGroupsParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public int userId { get; set; }
        public AspNetUserGroup AspNetUserGroups { get; set; }

        public List<AspNetUserGroup> AspNetUserGroupss { get; set; }

        public AspNetUserGroupsInfo AspNetUserGroupsInfo { get; set; }

        public List<AspNetUserGroupsInfo> AspNetUserGroupsInfos { get; set; }

        public AspNetUserGroupsFilter AspNetUserGroupsFilter { get; set; }
        #endregion
    }
    public class AspNetUserGroupsFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}