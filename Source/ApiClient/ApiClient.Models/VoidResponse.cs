using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class VoidResponse
    {
        public string BookingImage { get; set; }
        public bool Status { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
