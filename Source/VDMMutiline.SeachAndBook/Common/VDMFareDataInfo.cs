using System.Collections.Generic;
using VDMMutiline.SeachAndBook.MonthSearch;
using ApiClient.Models;
using System;
namespace VDMMutiline.SeachAndBook
{
    public class VDMFareDataInfo : FareData
    {
       public  string FareDataIdValue { get; set; }
        public string SessionId { get; set; }
        public string StartPoint { get; set; }
        public int StopNum { get; set; }
        public string GroupClass { get; set; }
        public string EndPoint { get; set; }
        public string FlightValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string FareClass { get; set; }

        public string FlightNumber { get; set; }
        public int ItineraryType { get; set; }
        public string SessionAll { get; set; }
        public double PriceBefor { get; set; }
        public double PriceBeforNet { get; set; }
        public double FareAdtNet { get; set; }
        public List<VDMMutiline.SeachAndBook.MonthSearch.AvailFlight> AvailFlight { get; set; }
        public bool? Ishow23KgVN { get; set; }
        public bool? IsVNJQ { get; set; }
        public bool? IsVNJQ0Kg { get; set; }
    }
}