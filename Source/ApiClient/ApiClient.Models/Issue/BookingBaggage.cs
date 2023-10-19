using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class BookingBaggage
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public string Route { get; set; }
        public string Baggage { get; set; }
        public double Price { get; set; }
        public string Value { get; set; }
        public string Currency { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
    }
}
