using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class SettingUserInfo : SettingUser
    {
        public int? IdKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupID { get; set; }
    }
}
