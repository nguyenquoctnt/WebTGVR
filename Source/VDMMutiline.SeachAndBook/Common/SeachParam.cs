using System;
using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using ApiClient.Models;

namespace VDMMutiline.SeachAndBook
{
    public class SeachParam
    {
        public BK_Booking BkBooking { get; set; }
        public BaggageResponse BaggageInfo { get; set; }
     //  public SearchResponse FlightSearchResult { get; set; }
        public List<VDMFareDataInfo> DepartureFlights { get; set; }
        public List<VDMFareDataInfo> ReturnFlights { get; set; }
        public List<BK_PassengerInfo> Passengerlist { get; set; }
        public List<AirlineInfo> ListAirline { get; set; }
        public VDMFareDataInfo DepartureFlight { get; set; }
        public VDMFareDataInfo ReturnFlight { get; set; }
        public int countDepartureFlights { get; set; }
        public int countReturnFlights { get; set; }
        public int? Itinerary { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }
        public string Currency { get; set; }
        public int? Adult { get; set; }
        public int? Children { get; set; }
        public int? Infant { get; set; }
        public string AirlineCode { get; set; }
        public string older { get; set; }
        public string typeolder { get; set; }
        public bool TrongNuoc { get; set; }
        public List<VDMFareDataInfo> LstFareData { get; set; }
        public List<VDMFareDataInfo> FareDataList { get; set; }
        public VDMFareDataInfo FareDataInfo { get; set; }
        public int? DepartureFlightId { get; set; }
        public int? ReturnFlightId { get; set; }
        public string SearchParams { get; set; }
        public string FareDataIdValue { get; set; }
        public string StopNum { get; set; }
        public List<int> ListStopNum { get; set; }
        public DateTime Expireddate { get; set; }
        public bool timvere { get; set; }
        public DateTime NgayHienThuc { get; set; }

        public List<VeThangareDataInfo> DepartureFlightsVeThang { get; set; }
        public List<VeThangareDataInfo> ReturnFlightsVeThang { get; set; }
       
    }
}