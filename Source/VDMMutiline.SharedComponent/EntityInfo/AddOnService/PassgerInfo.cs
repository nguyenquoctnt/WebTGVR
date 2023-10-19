using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.SharedComponent.EntityInfo.AddOnService
{
    public class PassgerInfo
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public string PassportExpirationDate { get; set; }
        public int TicketLeg { get; set; }
        public string TicketRoute { get; set; }
        public string BaggageValue { get; set; }
        public string BaggageCode { get; set; }
        public double BaggagePrice { get; set; }
        public string BaggageRoute { get; set; }
    }
}