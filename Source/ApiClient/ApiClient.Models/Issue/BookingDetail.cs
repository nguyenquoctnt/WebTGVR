using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class BookingDetail
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int BookingId { get; set; }
        public int Leg { get; set; }
        public string BookingStatus { get; set; }
        public string BookingCode { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime DepartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Itinerary { get; set; }
        public string Airline { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string System { get; set; }
        public double Fare { get; set; }
        public double Tax { get; set; }
        public double Fee { get; set; }
        public double ServiceFee { get; set; }
        public double BaggageFee { get; set; }
        public double GrandTotal { get; set; }
        public string Currency { get; set; }
        public double ResponseTime { get; set; }
        public string ErrorMessage { get; set; }
        public string AgentName { get; set; }
        public int Adt { get; set; }
        public int Chd { get; set; }
        public int Inf { get; set; }
        public List<BookingFlight> ListFlight { get; set; }
    }
}
