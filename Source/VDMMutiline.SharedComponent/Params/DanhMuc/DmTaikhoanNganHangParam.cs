using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class DmTaikhoanNganHangParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public DmTaikhoanNganHang DmTaikhoanNganHang { get; set; }

        public List<DmTaikhoanNganHang> DmTaikhoanNganHangs { get; set; }

        public DmTaikhoanNganHangInfo DmTaikhoanNganHangInfo { get; set; }

        public List<DmTaikhoanNganHangInfo> DmTaikhoanNganHangInfos { get; set; }

        public DmTaikhoanNganHangFilter DmTaikhoanNganHangFilter { get; set; }
        #endregion
    }
    public class DmTaikhoanNganHangFilter : BaseFilter
    {
        public int? Id { get; set; }
        public List<string> ListUserName { get; set; }
    }

}