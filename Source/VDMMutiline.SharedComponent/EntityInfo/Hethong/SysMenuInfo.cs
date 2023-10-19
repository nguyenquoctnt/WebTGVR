using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class SysMenuInfo : SysMenu
    {
        public string TenCMCha { get; set; }
        public IEnumerable<int?> SysMenuGroups { get; set; }
    }
}