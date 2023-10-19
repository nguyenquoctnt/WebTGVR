using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.WebBackEnd.Models
{
    public class CreatePassengerModel
    {
        public int? Idbooking { get; set; }
        public int? Idticket { get; set; }
        public int? TypeID { get; set; }
        public bool IsReturn { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Birthday { get; set; }
        public string Sex { get; set; }
        public string BaggageDepartID { get; set; }
        public string BaggageReturnID { get; set; }
        public string BaggageDepartValue { get; set; }
        public string BaggageReturnValue { get; set; }
        public string BaggageDepartPrice { get; set; }
        public string BaggageReturnPrice { get; set; }
        public string TotalPriceDepart { get; set; }
        public string TotalPriceReturn { get; set; }

    }
}