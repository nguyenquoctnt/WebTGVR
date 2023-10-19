using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
   public class BookingDetailGetByBookingIdResponse : BaseResponse
    {
        public List<BookingDetail> ListBookingDetail { get; set; }
        public string ErrorValue { get; set; }
        public string ErrorField { get; set; }
    }
}
