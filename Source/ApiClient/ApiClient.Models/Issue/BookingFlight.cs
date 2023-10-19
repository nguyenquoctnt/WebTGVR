using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class BookingFlight
    {
        public int Id { get; set; }
        public int BookingDetailId { get; set; }
        public int Leg { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public double Duration { get; set; }
        public int StopNum { get; set; }
        public string StopPoint { get; set; }
        public bool NoRefund { get; set; }
        public string FareClass { get; set; }
        public bool Promo { get; set; }
        public List<BookingSegment> ListSegment { get; set; }
    }
}
