using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class Baggage
    {
        public string Airline
        {
            get; set;

        }
        public int Leg
        {
            get; set;
        }
        public string Route
        {
            get; set;
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
    }
}
