using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.EntityInfo.Issue;

namespace VDMMutiline.SharedComponent.EntityInfo.AddOnService
{
    public class ConfirmAddOnServiceInfo
    {
        public AddOnServiceInfo AddOnServiceInfo { get; set; }
        public BookingInfo BookingInfo { get; set; }
        public List<PassgerInfo> PassgerInfo { get; set; }
    }
    public class AddOnServiceInfo
    {
        public string BookingCode { get; set; }
        public int? Status { get; set; }
        public string Remark { get; set; }
        public string AirlineCode { get; set; }
    }
}