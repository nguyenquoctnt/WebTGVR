using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetRoleGroupsParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetRoleGroup AspNetRoleGroups { get; set; }

        public List<AspNetRoleGroup> AspNetRoleGroupss { get; set; }

        public AspNetRoleGroupsInfo AspNetRoleGroupsInfo { get; set; }

        public List<AspNetRoleGroupsInfo> AspNetRoleGroupsInfos { get; set; }

        public AspNetRoleGroupsFilter AspNetRoleGroupsFilter { get; set; }
        #endregion
    }
    public class AspNetRoleGroupsFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}