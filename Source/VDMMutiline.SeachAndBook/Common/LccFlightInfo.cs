using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace VDMMutiline.SeachAndBook
{
    public class VeThangareDataInfo : VeThangFareData
    {
        public int TotalDuration { get; set; }
        public int StopNum { get; set; }
        public Double Pricebefor { get; set; }
        public string AirlineCode { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public List<VeThangSegment> ListSegment { get; set; }
        public string SessionAll { get; set; }
        public string DebugID { get; set; }
        public int ItineraryType { get; set; }
        public string Airline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int Stops { get; set; }
        public int Duration { get; set; }
        public string SelectValue { get; set; }
        public double PriceBefor { get; set; }
        public string GroupClass { get; set; }
        public string FareClass { get; set; }
        public string FlightNumber { get; set; }
    }
    public class VeThangFareData
    {

        public int FareDataId
        {
            get;
            set;
        }

        public string PlatingCarrier
        {
            get;
            set;
        }
        public int Itinerary
        {
            get;
            set;
        }
        public int Adt
        {
            get;
            set;
        }
        public int Chd
        {
            get;
            set;
        }
        public int Inf
        {
            get;
            set;
        }
        public double FareAdt
        {
            get;
            set;
        }
        public double FareChd
        {
            get;
            set;
        }
        public double FareInf
        {
            get;
            set;
        }
        public double TaxAdt
        {
            get;
            set;
        }
        public double TaxChd
        {
            get;
            set;
        }
        public double TaxInf
        {
            get;
            set;
        }
        public double FeeAdt
        {
            get;
            set;
        }
        public double FeeChd
        {
            get;
            set;
        }
        public double FeeInf
        {
            get;
            set;
        }
        public double TotalPrice
        {
            get;
            set;
        }
        public double TotalNetPrice
        {
            get;
            set;
        }
        public string Currency
        {
            get;
            set;
        }

    }
    public class VeThangSegment
    {


        public int Id
        {
            get;
            set;
        }

        public string Airline
        {
            get;
            set;
        }

        public string StartPoint
        {
            get;
            set;
        }

        public string EndPoint
        {
            get;
            set;
        }

        public System.DateTime StartTime
        {
            get;
            set;
        }

        public System.DateTime EndTime
        {
            get;
            set;
        }


        public string FlightNumber
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public string Class
        {
            get;
            set;
        }

        public string Plane
        {
            get; set;
        }
    }
}