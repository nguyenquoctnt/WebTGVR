using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMRequestInfo
    {
        public VDMAuthentication Authentication
        {
            get; set;
        }
        public string BookType
        {
            get; set;
        }
        public string AirlineCodeDepart
        {
            get;set;
        }
        public string AirlineCodeReturn
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