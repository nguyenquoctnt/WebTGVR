using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetRolesParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetRole AspNetRoles { get; set; }

        public List<AspNetRole> AspNetRoless { get; set; }

        public AspNetRolesInfo AspNetRolesInfo { get; set; }

        public List<AspNetRolesInfo> AspNetRolesInfos { get; set; }

        public AspNetRolesFilter AspNetRolesFilter { get; set; }
        #endregion
    }
    public class AspNetRolesFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}