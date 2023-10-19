using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMFlightDesignator
    {
        public string CarrierCode
        {
            get; set;
        }
        public string FlightNumber
        {
            get; set;
        }
        public string OpSuffix
        {
            get; set;
        }
    }
}