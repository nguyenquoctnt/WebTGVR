using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.EntityInfo.Issue;

namespace VDMMutiline.SharedComponent.EntityInfo.VoidTicket
{
    public class ConfirmVoidInfo
    {
        public VoidInfo VoidInfo { get; set; }
        public BookingInfo BookingInfo { get; set; }
    }
    public class VoidInfo
    {
        public string BookingCode { get; set; }
        public string Remark { get; set; }
        public string AirlineCode { get; set; }
    }
}