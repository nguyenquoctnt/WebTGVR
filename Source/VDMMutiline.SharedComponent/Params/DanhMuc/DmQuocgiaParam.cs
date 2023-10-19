using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class DmQuocgiaParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public DmQuocgia DmQuocgia { get; set; }

        public List<DmQuocgia> DmQuocgias { get; set; }

        public DmQuocgiaInfo DmQuocgiaInfo { get; set; }

        public List<DmQuocgiaInfo> DmQuocgiaInfos { get; set; }

        public DmQuocgiaFilter DmQuocgiaFilter { get; set; }
        #endregion
    }
    public class DmQuocgiaFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}