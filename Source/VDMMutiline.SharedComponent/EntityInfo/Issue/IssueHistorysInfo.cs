using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.Issue
{
    public class IssueHistorysInfo: tblIssueHistory
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
    }
}