using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class SysVungHienthiParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public SysVungHienthi SysVungHienthi { get; set; }

        public List<SysVungHienthi> SysVungHienthis { get; set; }

        public SysVungHienthiInfo SysVungHienthiInfo { get; set; }

        public List<SysVungHienthiInfo> SysVungHienthiInfos { get; set; }

        public SysVungHienthiFilter SysVungHienthiFilter { get; set; }
        #endregion
    }
    public class SysVungHienthiFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}