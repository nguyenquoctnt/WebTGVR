using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.APIExport.Requests
{
    public class SearchRequest
    {
        public int Itinerary { get; set; }
        public string StartDate { get; set; }
        public string ReturnDate { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int Adt { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        public string HeaderUser { get; set; }
        public string HeaderPass { get; set; }
    }
}