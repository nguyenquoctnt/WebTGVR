using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMRequestGetSeatInfo
    {
        public VDMAuthentication Authentication
        {
            get; set;
        }
        public int Itinerary
        {
            get; set;
        }
        public string SessionDepart
        {
            get; set;
        }
        public string SessionReturn
        {
            get; set;
        }
        public int FareDataId
        {
            get; set;
        }
        public int DepartFlightId
        {
            get; set;
        }
        public int ReturnFlightId
        {
            get; set;
        }
    }
}