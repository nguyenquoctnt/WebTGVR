using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.TempleteSetting
{
    public class TempleatePropertyInfo: tbl_TempleateProperty
    {
        public string NameGroup { get; set; }
        public string TypeName { get; set; }
    }
}