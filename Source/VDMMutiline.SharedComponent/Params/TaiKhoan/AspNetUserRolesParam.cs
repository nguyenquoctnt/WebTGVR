using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetUserRolesParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetUserRole AspNetUserRoles { get; set; }

        public List<AspNetUserRole> AspNetUserRoless { get; set; }

        public AspNetUserRolesInfo AspNetUserRolesInfo { get; set; }

        public List<AspNetUserRolesInfo> AspNetUserRolesInfos { get; set; }

        public AspNetUserRolesFilter AspNetUserRolesFilter { get; set; }
        #endregion
    }
    public class AspNetUserRolesFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}