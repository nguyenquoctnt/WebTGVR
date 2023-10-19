using System;
using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class BK_PassengerInfo : BK_Passenger
    {
        public string FullName { get; set; }
        public double Price { get; set; }
        public string FlightNo { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime? StartDate { get; set; }
        public string codedi { get; set; }
        public string codeve { get; set; }
        public string Ngay { get; set; }
        public string Thang { get; set; }
        public string Nam { get; set; }
        public List<int> Listngay { get; set; }
        public List<int> Listthang { get; set; }
        public List<int> Listnamembe { get; set; }
        public List<int> Listnamtreem { get; set; }
        public int? Typeve { get; set; }
    }
}
