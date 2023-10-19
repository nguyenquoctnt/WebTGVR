using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.Issue;
using VDMMutiline.SharedComponent.EntityInfo.VoidTicket;

namespace VDMMutiline.SharedComponent.Params.VoidTicket
{
    public class VoidTicketHistoryParam : BaseParam
    {
        public bool isAdmin { get; set; }
        public tblVoidTicketHistory VoidTicketHistory { get; set; }
        public List<VoidTicketHistorysInfo> VoidTicketHistorysInfos { get; set; }
        public VoidTicketHistoryFilter VoidTicketHistoryFilter { get; set; }
    }
    public class VoidTicketHistoryFilter : BaseFilter
    {
        public List<string> UserList { get; set; }
        public int? ParentId { get; set; }
        public int? UserIdCreate { get; set; }
        public string UserCreate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string BookCode { get; set; }
    }
}