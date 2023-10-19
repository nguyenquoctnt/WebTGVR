using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMBookingRequest
    {
        public VDMAuthentication Authentication
        {
            get; set;
        }
        public string BookType
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


        public VDMPassenger[] ListPassengers
        {
            get; set;
        }


        public VDMContact ContactInfo
        {
            get; set;
        }

        public string currency { get; set; }
        public string Remark
        {
            get; set;
        }
    }
}