using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.AddOnService
{
    public class AddOnServiceHistorysInfo : tblAddOnServiceHistory
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GoiHanhLy { get; set; }
    }
}