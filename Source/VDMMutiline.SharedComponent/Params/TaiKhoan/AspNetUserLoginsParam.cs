using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetUserLoginsParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetUserLogin AspNetUserLogins { get; set; }

        public List<AspNetUserLogin> AspNetUserLoginss { get; set; }

        public AspNetUserLoginsInfo AspNetUserLoginsInfo { get; set; }

        public List<AspNetUserLoginsInfo> AspNetUserLoginsInfos { get; set; }

        public AspNetUserLoginsFilter AspNetUserLoginsFilter { get; set; }
        #endregion
    }
    public class AspNetUserLoginsFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}