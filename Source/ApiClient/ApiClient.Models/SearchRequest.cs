using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class SearchRequest : BaseRequest
    {

        private int adtField;

        private int chdField;

        private int infField;

        private string viewModeField;

        private FlightRequest[] listFlightField;

       
        public int Adt
        {
            get
            {
                return this.adtField;
            }
            set
            {
                this.adtField = value;
            }
        }

       
        public int Chd
        {
            get
            {
                return this.chdField;
            }
            set
            {
                this.chdField = value;
            }
        }

       
        public int Inf
        {
            get
            {
                return this.infField;
            }
            set
            {
                this.infField = value;
            }
        }

       
        public string ViewMode
        {
            get
            {
                return this.viewModeField;
            }
            set
            {
                this.viewModeField = value;
            }
        }

       
        public FlightRequest[] ListFlight
        {
            get
            {
                return this.listFlightField;
            }
            set
            {
                this.listFlightField = value;
            }
        }
    }
}
