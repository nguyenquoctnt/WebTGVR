using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.Issue;

namespace VDMMutiline.SharedComponent.Params.Issue
{
    public class IssueParam : BaseParam
    {
        public bool isAdmin { get; set; }
        public tblIssueHistory IssueHistory { get; set; }
        public List<tblIssueHistory> IssueHistorys { get; set; }
        public List<IssueHistorysInfo> IssueHistorysInfos { get; set; }
        public IssueFilter IssueFilter { get; set; }
    }
    public class IssueFilter : BaseFilter
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