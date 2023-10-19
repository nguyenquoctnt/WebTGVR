using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class SysMenuHomeInfo: SysMenuHome
    {
        public bool Status { get; set; }
        public string Username { get; set; }
    }
}