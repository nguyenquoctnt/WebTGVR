using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class VoidRequest : BaseRequest
    {
        public string Airline { get; set; }
        public string BookingCode { get; set; }
        public bool CancelBooking { get; set; }
    }
}
