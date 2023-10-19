using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class Booking
    {
        public string Status { get; set; }

        public bool AutoIssue { get; set; }
        public string Airline { get; set; }
        public string BookingCode { get; set; }
        public string GdsCode { get; set; }
        public string Flight { get; set; }
        public string Route { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string BookingImage { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public string ExpiryDt { get; set; }
        public double ExpiryTime { get; set; }
        public double ResponseTime { get; set; }
        public string System { get; set; }
        public double Price { get; set; }
        public double Difference { get; set; }
        public string Session { get; set; }
        public Ticket[] ListTicket { get; set; }
        public FareData FareData { get; set; }
    }

}
