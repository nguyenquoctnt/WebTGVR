using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetUsersParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetUser AspNetUsers { get; set; }
        public string UrlDomain { get; set; }
        public List<AspNetUser> AspNetUserss { get; set; }

        public AspNetUserInfo AspNetUsersInfo { get; set; }
        public List<SettingUserInfo> SettingUserInfos { get; set; }
        public List<AspNetUserInfo> AspNetUsersInfos { get; set; }

        public AspNetUsersFilter AspNetUsersFilter { get; set; }
        public List<AspNetRolesInfo> RoleList { get; set; }
        public AspNetUserRolesInfo AspNetUserRolesInfo { get; set; }
        public List<AspNetUserRole> AspNetUserRoles { get; set; }
        public List<AspNetUserRolesInfo> AspNetUserRolesInfos { get; set; }
        public List<AspNetGroupsInfo> AspNetGroupsInfos { get; set; }
        public List<SysMenuHomeInfo> SysMenuHomeInfos { get; set; }
        public List<tbl_StyleSite> StyleSites { get; set; }
        public AspNetGroup AspNetGroup { get; set; }
        public AspNetUserGroup AspNetUserGroup { get; set; }
        public List<AspNetUserGroupsInfo> AspNetUserGroupsInfos { get; set; }
        #endregion
    }
    public class AspNetUsersFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public List<string> UserList { get; set; }
    }

}