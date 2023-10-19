using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using System.Collections.Generic;

namespace VDMMutiline.SharedComponent.Params
{
    public class MenuHomeParam : BaseParam
    {
      
        #region Action
        public int Id { get; set; }
        public SysMenuHome SysMenuHome { get; set; }
        public List<SysMenuHome> SysMenuHomes { get; set; }
        public SysMenuHomeInfo SysMenuHomeInfo { get; set; }
        public List<SysMenuHomeInfo> SysMenuHomeInfos { get; set; }
        public SysUserMenuHome SysUserMenuHome { get; set; }
        public List<SysUserMenuHome> SysUserMenuHomes { get; set; }
        public SysUserMenuHomeInfo SysUserMenuHomeInfo { get; set; }
        public List<SysUserMenuHomeInfo> SysUserMenuHomeInfos { get; set; }
        public SysMenuHomeFilter SysMenuHomeFilter { get; set; }
        #endregion
    }
    public class SysMenuHomeFilter : BaseFilter
    {
        public int? UserId { get; set; }
    }
}