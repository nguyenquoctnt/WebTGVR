using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FlightInfo
    {

        private string flightValueField;

       
        public string FlightValue
        {
            get
            {
                return this.flightValueField;
            }
            set
            {
                this.flightValueField = value;
            }
        }
    }
}
