using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class VoucherCodeParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_VoucherCode VoucherCode { get; set; }
        public List<tbl_VoucherCode> VoucherCodes { get; set; }
        public VoucherCodeInfo VoucherCodeInfo { get; set; }
        public List<VoucherCodeInfo> VoucherCodeInfos { get; set; }
        public VoucherCodeFilter VoucherCodeFilter { get; set; }
        #endregion
    }
    public class VoucherCodeFilter : BaseFilter
    {
        public int? Id { get; set; }
        public DateTime? FormDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? Status { get; set; }
             public int? StatusInt { get; set; }
        public string Mail { get; set; }
        public List<string> ListuserinSite { get; set; }
    }
}
