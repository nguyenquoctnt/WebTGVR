using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class Booking
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public string OrderCode { get; set; }
        public string BookingStatus { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IPAddress { get; set; }
        public double PaymentFee { get; set; }
        public string PaymentCode { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentGatewayCode { get; set; }
        public string PaymentGatewayStatus { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string BookingCode { get; set; }
        public string FlightInfo { get; set; }
        public string PassengerName { get; set; }
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime DepartDate { get; set; }
        public double Fare { get; set; }
        public double Tax { get; set; }
        public double Fee { get; set; }
        public double ServiceFee { get; set; }
        public double BaggageFee { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string Currency { get; set; }
        public string BookType { get; set; }
        public string ResponseTime { get; set; }
        public bool Checked { get; set; }
        public bool Visible { get; set; }
        public int CustomerId { get; set; }
        public string HandleBy { get; set; }
        public string CouponCode { get; set; }
        public double CouponAmount { get; set; }
        public bool? Locked { get; set; }
        public int? LockedBy { get; set; }
        public int Adt { get; set; }
        public int Chd { get; set; }
        public int Inf { get; set; }
        public string Remark { get; set; }
        public string Note { get; set; }
        public string AgentName { get; set; }
    }
}
