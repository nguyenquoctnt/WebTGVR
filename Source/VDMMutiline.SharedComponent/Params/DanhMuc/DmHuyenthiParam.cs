using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class DmHuyenthiParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public DmHuyenthi DmHuyenthi { get; set; }

        public List<DmHuyenthi> DmHuyenthis { get; set; }

        public DmHuyenthiInfo DmHuyenthiInfo { get; set; }

        public List<DmHuyenthiInfo> DmHuyenthiInfos { get; set; }

        public DmHuyenthiFilter DmHuyenthiFilter { get; set; }
        public List<DmTinhthanhInfo> DmTinhthanhInfos { get; set; }
        public List<DmQuocgiaInfo> DmQuocgiaInfos { get; set; }
        #endregion
    }
    public class DmHuyenthiFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? Idtinhthanh { get; set; }
    }

}