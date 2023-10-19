using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
   public class BookingGetByIdResponse: BaseResponse
    {
        public ApiClient.Models.Issue.Booking Booking { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorValue { get; set; }
        public string ErrorField { get; set; }
    }
}
