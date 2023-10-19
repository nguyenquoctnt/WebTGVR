using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMLccFlight
    {

        public int FlightId
        {
            get; set;
        }
        public string Airline
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

        public System.DateTime StartDate
        {
            get; set;
        }

        public System.DateTime EndDate
        {
            get; set;
        }

        public string FlightNumber
        {
            get; set;
        }

        public int Stops
        {
            get; set;
        }

        public int Duration
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

        public double FareAdt
        {
            get; set;
        }

        public double FareChd
        {
            get; set;
        }

        public double FareInf
        {
            get; set;
        }

        public double TaxAdt
        {
            get; set;
        }

        public double TaxChd
        {
            get; set;
        }

        public double TaxInf
        {
            get; set;
        }

        public double FeeAdt
        {
            get; set;
        }
        public double FeeChd
        {
            get; set;
        }

        public double FeeInf
        {
            get; set;
        }

        public double ServiceFeeAdt
        {
            get; set;
        }

        public double ServiceFeeChd
        {
            get; set;
        }


        public double ServiceFeeInf
        {
            get; set;
        }


        public double TotalServiceFee
        {
            get; set;
        }


        public double TotalNetPrice
        {
            get; set;
        }


        public double TotalPrice
        {
            get; set;
        }


        public string Currency
        {
            get; set;
        }

        public string GroupClass
        {
            get; set;
        }


        public string FareClass
        {
            get; set;
        }


        public bool Promo
        {
            get; set;
        }


        public string SelectValue
        {
            get; set;
        }


        public string Cookies
        {
            get; set;
        }


        public bool ViewFareMode
        {
            get; set;
        }


        public VDMSegment[] ListSegment
        {
            get; set;
        }


        public int JQScheduleIndex { get; set; }
        public int JQJourneyIndex { get; set; }
        public int JQDatemaketIndex { get; set; }
        public List<VDMBaggage> JQLstVDMBaggage { get; set; }
    }
}