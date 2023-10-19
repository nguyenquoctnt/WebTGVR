using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.SharedComponent.EntityInfo.Issue
{
    public class ConfirmIssueInfo
    {
        public IssueInfo IssueInfo { get; set; }
        public BookingInfo BookingInfos { get; set; }
    }
    public class IssueInfo
    {
        public string BookingCode { get; set; }
        public string Remark { get; set; }
        public string AirlineCode { get; set; }
    }
}