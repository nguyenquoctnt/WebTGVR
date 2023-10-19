using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class SettingUserParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public int? Userid { get; set; }
        public SettingUser SettingUser { get; set; }
        public List<SettingUser> SettingUsers { get; set; }
        public SettingUserInfo SettingUserInfo { get; set; }
        public List<SettingUserInfo> SettingUserInfos { get; set; }
        public SettingUserFilter SettingUserFilter { get; set; }
        #endregion
    }
    public class SettingUserFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}
