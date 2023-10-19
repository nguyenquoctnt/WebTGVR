using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class CancelResponse : BaseResponse
    {


        public string BookingCode
        {
            get;set;
        }
        public string BookingImage
        {
            get; set;
        }
    }
}
