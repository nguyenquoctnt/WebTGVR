using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class DmXaphuongParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public DmXaphuong DmXaphuong { get; set; }
        public List<DmXaphuong> DmXaphuongs { get; set; }
        public DmXaphuongInfo DmXaphuongInfo { get; set; }
        public List<DmXaphuongInfo> DmXaphuongInfos { get; set; }
        public List<DmQuocgiaInfo> DmQuocgiaInfos { get; set; }
        public List<DmTinhthanhInfo> DmTinhthanhInfos { get; set; }
        public List<DmHuyenthiInfo> DmHuyenthiInfos { get; set; }
        public DmXaphuongFilter DmXaphuongFilter { get; set; }
        #endregion
    }
    public class DmXaphuongFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? IdHuyen { get; set; }
    }

}