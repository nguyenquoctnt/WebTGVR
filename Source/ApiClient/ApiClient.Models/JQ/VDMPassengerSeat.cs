using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMPassengerSeat
    {
        public int Index
        {
            get; set;
        }
        public int Id
        {
            get; set;
        }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string FirstName
        {
            get; set;
        }
        public string LastName
        {
            get; set;
        }
        public string Type
        {
            get; set;
        }
        public bool Gender
        {
            get; set;
        }
        public string Seat
        {
            get; set;
        }
        public double? Price
        {
            get; set;
        }
        public int? GroupSeat
        {
            get; set;
        }

    }
}