using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class SysMenuParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public SysMenu SysMenu { get; set; }

        public List<SysMenu> SysMenus { get; set; }

        public SysMenuInfo SysMenuInfo { get; set; }

        public List<SysMenuInfo> SysMenuInfos { get; set; }
        public List<SiteInfo> SiteInfos { get; set; }

        public SysMenuFilter SysMenuFilter { get; set; }
        public  List<AspNetRoleGroupsInfo> AspNetRoleGroupsInfos { get; set; }
        public List<AspNetGroupsInfo> AspNetGroupsInfos { get; set; }
        public List<SysMenuGroupInfo> SysMenuGroupInfos { get; set; }
        public List<SysMenuGroup> SysMenuGroups { get; set; }
        #endregion
    }
    public class SysMenuFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}