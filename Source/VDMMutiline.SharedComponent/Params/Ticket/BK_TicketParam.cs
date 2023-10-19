using System;
using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BK_TicketParamParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public BK_Ticket BK_Ticket { get; set; }
        public List<BK_Ticket> BK_Tickets { get; set; }
        public BK_TicketInfo BK_TicketInfo { get; set; }
        public List<BK_TicketInfo> BK_TicketInfos { get; set; }
        public BK_TicketFilter BK_TicketFilter { get; set; }
        public PagedList.IPagedList<BK_TicketInfo> PagedList { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string TuNgayDenNgay { get; set; }
        public List<string> ListUserName { get; set; }
        #endregion
    }
    public class BK_TicketFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? vitri { get; set; }

    }
}
