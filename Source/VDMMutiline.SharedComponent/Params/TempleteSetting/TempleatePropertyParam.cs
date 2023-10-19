using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
namespace VDMMutiline.SharedComponent.Params.TempleteSetting
{
    public class TempleatePropertyParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_TempleateProperty TempleateProperty { get; set; }
        public List<tbl_TempleateProperty> TempleatePropertys { get; set; }
        public TempleatePropertyInfo TempleatePropertyInfo { get; set; }
        public List<TempleatePropertyInfo> TempleatePropertyInfos { get; set; }
        public List<TempleatePropertyGroupInfo> TempleatePropertyGroupInfos { get; set; }
        
        public TempleatePropertyFilter TempleatePropertyFilter { get; set; }
        #endregion
    }
    public class TempleatePropertyFilter : BaseFilter
    {
        public int? Id { get; set; }
    }
}