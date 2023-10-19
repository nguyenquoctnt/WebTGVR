using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMSegment
    {
        public int Id
        {
            get; set;
        }

        public string CabinOfService
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

        public System.DateTime StartTime
        {
            get; set;
        }

        public System.DateTime EndTime
        {
            get; set;
        }


        public string FlightNumber
        {
            get; set;
        }


        public int Duration
        {
            get; set;
        }


        public string Class
        {
            get; set;

        }


        public string Plane
        {
            get; set;
        }


        public bool HasStop
        {
            get; set;
        }


        public string StopPoint
        {
            get; set;
        }


        public double StopTime
        {
            get; set;
        }


        public bool DayChange
        {
            get; set;
        }


        public bool StopOvernight
        {
            get; set;
        }


        public string StartTerminal
        {
            get; set;
        }


        public string EndTerminal
        {
            get; set;
        }


        public bool ChangeStation
        {
            get; set;
        }


        public bool ChangeAirport
        {
            get; set;
        }


        public string FrstDwnlnStp
        {
            get; set;
        }


        public string LastDwnlnStp
        {
            get; set;
        }


        public bool LastItem
        {
            get; set;
        }
        public VDMFlightDesignator FlightDesignator
        {
            get; set;
        }
    }
}