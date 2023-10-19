using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class SysMenuGroupParam:BaseParam
    {
        public int? Id { get; set; }
        public int? MenuId { get; set; }
        public SysMenuGroup SysMenuGroup { get; set; }
        public List<SysMenuGroup> SysMenuGroups { get; set; }
        public List<SysMenuGroupInfo> SysMenuGroupInfos { get; set; }
        public SysMenuGroupInfo SysMenuGroupInfo { get; set; }
        public SysMenuGroupFilter SysMenuGroupFilter { get; set; }
    }

    public class SysMenuGroupFilter : BaseFilter
    {
        
    }
}