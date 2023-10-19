using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class SearchMinResponse : BaseResponse
    {
        private FareData minFlightField;
       
        public FareData MinFlight
        {
            get
            {
                return this.minFlightField;
            }
            set
            {
                this.minFlightField = value;
            }
        }
    }

}
