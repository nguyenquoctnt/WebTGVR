using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
   public class MailParam:BaseParam
    { 
       #region Action
        public int Id { get; set; }
        public tbl_Mail Mail { get; set; }
        public List<tbl_Mail> Mails { get; set; }
        public MailInfo MailInfo { get; set; }
        public List<MailInfo> MailInfos { get; set; }
        public MailFilter MailFilter { get; set; }
        public string SourceSite { get; set; }
        public string UserName { get; set; }
        public List<string> listuserinsite { get; set; }
        #endregion
    }
   public class MailFilter : BaseFilter
   {
       public int? Id { get; set; }
   }
}
