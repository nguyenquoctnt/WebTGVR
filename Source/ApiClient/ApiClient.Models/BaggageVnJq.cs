using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class BaggageVnJq
    {
        public int? FlightNumFrom { get; set; }
        public int? FlightNumTo { get; set; }
        public string FlightNum { get; set; }
        public string StartPoind { get; set; }
        public string EndPoind { get; set; }
        public string FareClass0Kg { get; set; }
    }
}
