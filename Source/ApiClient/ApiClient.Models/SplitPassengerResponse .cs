using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class SplitPassengerResponse : BaseResponse
    {
        public string Airline { get; set; }
        public string BookingCode { get; set; }
        public string BookingImage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CurrentBookingCode { get; set; }
        public string CurrentBookingImage { get; set; }
    }
}
