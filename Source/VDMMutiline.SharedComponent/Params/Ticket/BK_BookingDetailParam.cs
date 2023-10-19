using System;
using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BK_BookingDetailParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public BK_BookingDetail BK_BookingDetail { get; set; }
        public List<BK_BookingDetail> BK_BookingDetails { get; set; }
        public BK_BookingDetailInfo BK_BookingDetailInfo { get; set; }
        public List<BK_BookingDetailInfo> BK_BookingDetailInfos { get; set; }
        public BK_BookingDetailAnhFilter BK_BookingDetailAnhFilter { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateBooked { get; set; }
        public DateTime DateEpired { get; set; }
        public int Status { get; set; }
        public Decimal Gross { get; set; }
        public Decimal TotalPrice { get; set; }
        public List<BK_BookingDetail> BookingDT { get; set; }
        public List<BK_Ticket> Ticket { get; set; }
        public int UserId { get; set; }
        public int UserName { get; set; }
        #endregion
    }
    public class BK_BookingDetailAnhFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? vitri { get; set; }
    }
}
