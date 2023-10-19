using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class SysCategoryParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public SysCategory SysCategory { get; set; }

        public List<SysCategory> SysCategorys { get; set; }

        public SysCategoryInfo SysCategoryInfo { get; set; }

        public List<SysCategoryInfo> SysCategoryInfos { get; set; }

        public SysCategoryFilter SysCategoryFilter { get; set; }

      
        #endregion
    }
    public class SysCategoryFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? type { get; set; }
        public string SeoUrl { get; set; }
        public List<string> ListUserName { get; set; }
        public int? IdGianhang { get; set; }
        public bool? IsHome { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsContend { get; set; }
        public bool? Isfooter { get; set; }
    }

}