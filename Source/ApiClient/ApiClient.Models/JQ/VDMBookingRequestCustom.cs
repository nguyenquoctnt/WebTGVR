using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMBookingRequestCustom
    {
        public VDMAuthentication Authentication
        {
            get; set;
        }
        public int Itinerary
        {
            get; set;
        }


        public string StartPoint
        {
            get; set;
        }


        public string EndPoint
        {
            get; set;
        }


        public System.DateTime DepartDate
        {
            get; set;
        }


        public System.Nullable<System.DateTime> ReturnDate
        {
            get; set;
        }


        public int Adt
        {
            get; set;
        }


        public int Chd
        {
            get; set;
        }


        public int Inf
        {
            get; set;
        }


        public string AirlineDepart
        {
            get; set;
        }


        public string AirlineReturn
        {
            get; set;
        }


        public string FlightNumberDepart
        {
            get; set;
        }


        public string FlightNumberReturn
        {
            get; set;

        }


        public double PriceDepart
        {
            get; set;

        }


        public double PriceReturn
        {
            get; set;
        }


        public bool AutoIssueTicketDepart
        {
            get; set;
        }


        public bool AutoIssueTicketReturn
        {
            get; set;
        }


        public bool UseAgentContact
        {
            get; set;
        }


        public bool CompareOnBaseFare
        {
            get; set;
        }


        public VDMPassenger[] ListPassengers
        {
            get; set;
        }


        public VDMContact ContactInfo
        {
            get; set;
        }


        public string Remark
        {
            get; set;
        }
        public string ArilineCode { get; set; }
        public string SessionDepart { get; set; }
        public string SessionReturn { get; set; }
        public int? FareDataId { get; set; }
        public  string currency { get; set; }
        public int DepartFlightId { get; set; }
        public int ReturnFlightId { get; set; }
        //public FareData ListFareData { get; set; }
        //public LccFlight DepartFlight { get; set; }
        //public LccFlight ReturnFlight { get; set; }

    }
}