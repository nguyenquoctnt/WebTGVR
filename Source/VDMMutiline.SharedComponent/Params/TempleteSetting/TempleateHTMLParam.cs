using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
namespace VDMMutiline.SharedComponent.Params.TempleteSetting
{
    public class TempleateHTMLParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_TempleateHTML TempleateHTML { get; set; }
        public List<tbl_TempleateHTML> TempleateHTMLs { get; set; }
        public TempleateHTMLInfo TempleateHTMLInfo { get; set; }
        public List<TempleateHTMLInfo> TempleateHTMLInfos { get; set; }
        public TempleateHTMLFilter TempleateHTMLFilter { get; set; }
        #endregion
    }
    public class TempleateHTMLFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}