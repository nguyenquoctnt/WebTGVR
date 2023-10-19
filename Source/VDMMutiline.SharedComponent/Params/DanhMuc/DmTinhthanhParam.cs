using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class DmTinhthanhParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public DmTinhthanh DmTinhthanh { get; set; }

        public List<DmTinhthanh> DmTinhthanhs { get; set; }

        public DmTinhthanhInfo DmTinhthanhInfo { get; set; }

        public List<DmTinhthanhInfo> DmTinhthanhInfos { get; set; }

        public List<DmQuocgiaInfo> DmQuocgiaInfos { get; set; }
        public DmTinhthanhFilter DmTinhthanhFilter { get; set; }
        #endregion
    }
    public class DmTinhthanhFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? Idquocgia { get; set; }
    }

}