using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FlightRequest
    {

        private string startPointField;

        private string endPointField;

        private string departDateField;

        private string airlineField;

       
        public string StartPoint
        {
            get
            {
                return this.startPointField;
            }
            set
            {
                this.startPointField = value;
            }
        }

       
        public string EndPoint
        {
            get
            {
                return this.endPointField;
            }
            set
            {
                this.endPointField = value;
            }
        }

       
        public string DepartDate
        {
            get
            {
                return this.departDateField;
            }
            set
            {
                this.departDateField = value;
            }
        }

       
        public string Airline
        {
            get
            {
                return this.airlineField;
            }
            set
            {
                this.airlineField = value;
            }
        }
    }
}
