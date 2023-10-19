using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.Params
{
    public class SettingGroupInfo : SettingGroup
    {
        public int IdKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
