using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class SearchMinRequest : BaseRequest
    {

        private FlightRequest flightRequestField;
       
        public FlightRequest FlightRequest
        {
            get
            {
                return this.flightRequestField;
            }
            set
            {
                this.flightRequestField = value;
            }
        }
    }
}
