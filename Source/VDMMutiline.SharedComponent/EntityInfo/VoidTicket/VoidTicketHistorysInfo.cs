using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.VoidTicket
{
    public class VoidTicketHistorysInfo: tblVoidTicketHistory
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}