using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class BaggageVn
    {
        public string StartPoind { get; set; }
        public string EndPoind { get; set; }
        public string FlightNum { get; set; }
        public string FareClass { get; set; }
        public decimal? PriceFare { get; set; }
    }
}
