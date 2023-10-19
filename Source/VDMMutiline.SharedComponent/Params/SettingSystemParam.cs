using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class SettingSystemParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public string Key { get; set; }
        public int? IdGroup { get; set; }
        public SysSettingSystem SysSettingSystem { get; set; }
        public List<SysSettingSystem> SysSettingSystems { get; set; }
        public SettingSystemInfo SettingSystemInfo { get; set; }
        public List<SettingSystemInfo> SettingSystemInfos { get; set; }
        public SettingSystemFilter SettingSystemFilter { get; set; }
        #endregion
    }
    public class SettingSystemFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}
