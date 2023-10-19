using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMRequestAssignSeat
    {
        public string bookcode { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public List<VDMPassengerSeat> listPassengerSeat { get; set; }
    }
}