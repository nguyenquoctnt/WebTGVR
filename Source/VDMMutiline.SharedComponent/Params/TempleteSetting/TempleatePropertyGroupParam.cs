using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
namespace VDMMutiline.SharedComponent.Params.TempleteSetting
{
    public class TempleatePropertyGroupParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_TempleatePropertyGroup TempleatePropertyGroup { get; set; }
        public List<tbl_TempleatePropertyGroup> TempleatePropertyGroups { get; set; }
        public TempleatePropertyGroupInfo TempleatePropertyGroupInfo { get; set; }
        public List<TempleatePropertyGroupInfo> TempleatePropertyGroupInfos { get; set; }
        public TempleatePropertyGroupFilter TempleatePropertyGroupFilter { get; set; }
        #endregion
    }
    public class TempleatePropertyGroupFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}