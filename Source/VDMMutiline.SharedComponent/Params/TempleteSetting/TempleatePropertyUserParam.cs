using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
namespace VDMMutiline.SharedComponent.Params.TempleteSetting
{
    public class TempleatePropertyUserParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_TempleatePropertyUser TempleatePropertyUser { get; set; }
        public List<tbl_TempleatePropertyUser> TempleatePropertyUsers { get; set; }
        public TempleatePropertyUserInfo TempleatePropertyUserInfo { get; set; }
        public List<TempleatePropertyUserInfo> TempleatePropertyUserInfos { get; set; }
        public TempleatePropertyUserFilter TempleatePropertyUserFilter { get; set; }
        #endregion
    }
    public class TempleatePropertyUserFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
    }
}