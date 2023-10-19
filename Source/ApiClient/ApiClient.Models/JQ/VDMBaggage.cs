using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMBaggage
    {
        public string AirlineCode
        {
            get;set;
        }

       
        public string Code
        {
            get; set;
        }

       
        public string Currency
        {
            get; set;
        }

       
        public string Name
        {
            get; set;
        }

      
        public double Price
        {
            get; set;
        }

       
        public string Value
        {
            get; set;
        }
        public  string BookType { get; set; }
        public  string SessionDepart { get; set; }
        public  string SessionReturn { get; set; }
        public  int FareDataId { get; set; }
        public  int DepartFlightId { get; set; }
        public  int Itinerary { get; set; }
        public  int ReturnFlightId { get; set; }

    }
}