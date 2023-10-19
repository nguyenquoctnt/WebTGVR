using System;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class BK_BookingInfo : BK_Booking
    {
        public int ticketid { get; set; }
        public DateTime? Startdate { get; set; }
        public bool? daguimailnhac { get; set; }
        public bool? Daguimailcamon { get; set; }
        public string hangve { get; set; }
        public tbl_PaymentInfo PaymentInfo { get; set; }
    }
}
