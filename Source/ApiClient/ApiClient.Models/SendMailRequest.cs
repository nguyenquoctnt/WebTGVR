using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class SendMailRequest
    {
        public string Airline { get; set; }
        public string BookingCode { get; set; }
        public string Email { get; set; }
        public string HeaderUser { get; set; }
        public string HeaderPass { get; set; }
        public string AgentAccount { get; set; }
        public string AgentPassword { get; set; }
        public string ProductKey { get; set; }
        public string Language { get; set; }

    }
}
