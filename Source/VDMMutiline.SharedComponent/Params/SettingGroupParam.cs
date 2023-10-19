using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class SettingGroupParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public int? IdGroup { get; set; }
        public int? Userid { get; set; }
        public SettingGroup SettingGroup { get; set; }
        public List<SettingGroup> SettingGroups { get; set; }
        public SettingGroupInfo SettingGroupInfo { get; set; }
        public List<SettingGroupInfo> SettingGroupInfos { get; set; }
        public SettingGroupFilter SettingGroupFilter { get; set; }
        #endregion
    }
    public class SettingGroupFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}
