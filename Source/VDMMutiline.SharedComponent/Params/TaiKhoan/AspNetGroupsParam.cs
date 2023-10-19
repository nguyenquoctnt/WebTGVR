using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetGroupsParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetGroup AspNetGroups { get; set; }
        public List<AspNetGroup> AspNetGroupss { get; set; }
        public AspNetGroupsInfo AspNetGroupsInfo { get; set; }
        public List<AspNetGroupsInfo> AspNetGroupsInfos { get; set; }
        public AspNetGroupsFilter AspNetGroupsFilter { get; set; }
        public List<AspNetRolesInfo> RoleList { get; set; }

        #endregion
    }
    public class AspNetGroupsFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}