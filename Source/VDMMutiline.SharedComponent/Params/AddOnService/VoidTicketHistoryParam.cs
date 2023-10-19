using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.Issue;
using VDMMutiline.SharedComponent.EntityInfo.AddOnService;

namespace VDMMutiline.SharedComponent.Params.AddOnService
{
    public class AddOnServiceParam : BaseParam
    {
        public bool isAdmin { get; set; }
        public tblAddOnServiceHistory AddOnServiceHistory { get; set; }
        public List<tblAddOnServiceHistory> AddOnServiceHistorys { get; set; }
        public List<AddOnServiceHistorysInfo> AddOnServiceHistorysInfos { get; set; }
        public AddOnServiceHistoryFilter AddOnServiceHistoryFilter { get; set; }
    }
    public class AddOnServiceHistoryFilter : BaseFilter
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