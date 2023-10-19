using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.TempleteSetting
{
    public class TempleatePropertyUserInfo : tbl_TempleatePropertyUser
    {
        public int? Typecontrol { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string key { get; set; }

    }
}