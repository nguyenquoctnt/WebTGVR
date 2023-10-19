using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMFlightSearchResult
    {
        public string Session { get; set; }
        public string Guid { get; set; }
        public string signatureJQ { get; set; }
        public string ErrorMessage { get; set; }
        public string Type { get; set; }
        public int Itinerary { get; set; }
        public System.DateTime SearchTime { get; set; }
        public VDMFareData[] ListFareData { get; set; }
        public VDMLccFlight[] ListDepartFlight { get; set; }
        public VDMLccFlight[] ListReturnFlight { get; set; }
    }
    
}