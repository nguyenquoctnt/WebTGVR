using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMSeachParam
    {
        public VDMAuthentication Authentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Itinerary { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        
    }
}