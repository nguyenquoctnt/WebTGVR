using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class BookingPassengerGetbybookingidResponse : BaseResponse
    {
        public List<BookingPassenger> ListBookingPassenger { get; set; }
        public string ErrorValue { get; set; }
        public string ErrorField { get; set; }
        public string Language { get; set; }
    }
}
