using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class MailContenParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_MailContend MailContend { get; set; }
        public List<tbl_MailContend> MailContends { get; set; }
        public MailContendInfo MailContendInfo { get; set; }
        public List<MailContendInfo> MailContendInfos { get; set; }
        public MailContenFilter MailContenFilter { get; set; }
        public List<string> userNameList { get; set; }
        #endregion
    }
    public class MailContenFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}
