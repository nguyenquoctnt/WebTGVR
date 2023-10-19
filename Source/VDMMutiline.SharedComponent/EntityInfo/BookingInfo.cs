using ApiClient.Models.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.SharedComponent.EntityInfo
{ 
    public class BookingInfo
    {
        public ApiClient.Models.Issue.Booking Booking { get; set; }
        public List<ApiClient.Models.Issue.BookingDetail> Tickets { get; set; }
        public List<ApiClient.Models.Issue.BookingPassenger> Passengers { get; set; }
        public List<BookingBaggage> ListBookingBaggage { get; set; }
        public string AirlineCode { get; set; }
    }
}