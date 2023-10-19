using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
namespace VDMMutiline.SharedComponent.Params.TempleteSetting
{
    public class TempleateHTMLUserParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_TempleateHTMLUser TempleateHTMLUser { get; set; }
        public List<tbl_TempleateHTMLUser> TempleateHTMLUsers { get; set; }
        public TempleateHTMLUserInfo TempleateHTMLUserInfo { get; set; }
        public List<TempleateHTMLUserInfo> TempleateHTMLUserInfos { get; set; }
        public TempleateHTMLUserFilter TempleateHTMLUserFilter { get; set; }
        #endregion
    }
    public class TempleateHTMLUserFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
    }
}