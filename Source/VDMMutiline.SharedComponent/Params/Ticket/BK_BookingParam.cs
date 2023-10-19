using System;
using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.Ticket;

namespace VDMMutiline.SharedComponent.Params
{
    public class BK_BookingParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public BK_Booking Booking { get; set; }
        public List<BK_Booking> Bookings { get; set; }
        public BK_BookingInfo BookingInfo { get; set; }
        public List<BK_BookingInfo> BookingInfos { get; set; }
        public PagedList.IPagedList<BK_BookingInfo> PagedList { get; set; }
        public List<AirlineInfo> AirlineInfos { get; set; }
        public BK_BookingFilter BookingFilter { get; set; }
        public List<AspNetUserInfo> UserInfos { get; set; }
        public List<TicketCountInfo> TicketCountInfos { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string note { get; set; }
        public int? bookingid { get; set; }
        public int? TrangThai { get; set; }
        public string TaiKhoan { get; set; }
        public string Ngay { get; set; }
        public string Gio { get; set; }
        public string Nguoixuly { get; set; }
        public string HoTenNguoixuly { get; set; }
        public string Ngayxuly { get; set; }
        public bool? Daxuly { get; set; }
        public DateTime? DateBooked { get; set; }
        public DateTime? DateEpired { get; set; }
        public int? Status { get; set; }
        public Decimal Gross { get; set; }
        public Decimal TotalPrice { get; set; }
        public List<BK_BookingDetail> BookingDT { get; set; }
        public List<BK_Ticket> Ticket { get; set; }
        public int UserId { get; set; }
        public int UserName { get; set; }
        public string RetunUrl { get; set; }
        #endregion
    }
    public class BK_BookingFilter : BaseFilter
    {
        public List<string> ListUserName { get; set; }
        public int? Id { get; set; }
        public int? StatusVe { get; set; }
        public string AirlineCode { get; set; }
        public string TuNgayDenNgay { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaiKhoan { get; set; }
        public AspNetUser User { get; set; }
        public bool? Vehethangiucho { get; set; }
        public bool? DatVeThatBai { get; set; }
    }
}
