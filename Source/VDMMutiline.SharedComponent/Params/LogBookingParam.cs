using System.Collections.Generic;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Entities;
using System;
namespace VDMMutiline.SharedComponent.Params
{
   public class LogBookingParam :BaseParam
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public tbl_LogBooking LogBooking { get; set; }
        public List<tbl_LogBooking> LogBookings { get; set; }
        public LogBookingInfo LogBookingInfo { get; set; }
        public List<LogBookingInfo> LogBookingInfos { get; set; }
        public List<AspNetUserInfo> UserInfos { get; set; }
        public LogBookingFilter LogBookingFilter { get; set; }
    }
    public class LogBookingFilter : BaseFilter
    {
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string TuNgayDenNgay { get; set; }
        public string User { get; set; }
        public string Bookcode { get; set; }
        public int? BookingID  { get; set; }
    }
}
