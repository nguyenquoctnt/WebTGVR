using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class ChangeFlightRequest : BaseRequest
    {
        public string Airline { get; set; }
        public string BookingCode { get; set; }
        public bool AutoIssue { get; set; }
        public NewFlight NewFlight { get; set; }
    }
    public class NewFlight
    {
        public int Id { get; set; }
        public int Itinerary { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int Adt { get; set; }
        public int Chd { get; set; }
        public int Inf { get; set; }
        public string DepartDate { get; set; }
        public string DepartFlightNumber { get; set; }
        public double DepartPrice { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnFlightNumber { get; set; }
        public double ReturnPrice { get; set; }
        public Segment ListSegment { get; set; }
    }
}
