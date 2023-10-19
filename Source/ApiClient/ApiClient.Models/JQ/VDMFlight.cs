using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMFlight
    {
        public int FlightId
        {
            get; set;
        }

       
        public int FareDataId
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

       
        public int Duration
        {
            get; set;
        }

       
        public int StopNum
        {
            get; set;
        }

       
        public bool HasDownStop
        {
            get; set;
        }

       
        public bool NoRefund
        {
            get; set;
        }

       
        public string Default
        {
            get; set;
        }

      
        public string SelectValue
        {
            get; set;
        }

        public int JQScheduleIndex { get; set; }
        public int JQJourneyIndex { get; set; }
        public int JQDatemaketIndex { get; set; }
        public VDMSegment[] ListSegment { get; set; }
        public List<VDMBaggage> JQLstVDMBaggage { get; set; }
    }
}