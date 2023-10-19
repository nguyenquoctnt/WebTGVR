using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMRequestGetSeat
    {
        public string bookcode { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
    }
}