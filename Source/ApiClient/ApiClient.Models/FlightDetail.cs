using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FlightDetail
    {

        private string airlineField;

        private int legField;

        private string startPointField;

        private string endPointField;

        private string departDateField;

        private string departTimeField;

        private string flightNumberField;

        private string fareClassField;

        private double priceField;

        private string compareModeField;

        private bool autoIssueField;

        private System.DateTime departDtField;

       
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

       
        public int Leg
        {
            get
            {
                return this.legField;
            }
            set
            {
                this.legField = value;
            }
        }

       
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

       
        public string DepartTime
        {
            get
            {
                return this.departTimeField;
            }
            set
            {
                this.departTimeField = value;
            }
        }

       
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }

       
        public string FareClass
        {
            get
            {
                return this.fareClassField;
            }
            set
            {
                this.fareClassField = value;
            }
        }

       
        public double Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

       
        public string CompareMode
        {
            get
            {
                return this.compareModeField;
            }
            set
            {
                this.compareModeField = value;
            }
        }

       
        public bool AutoIssue
        {
            get
            {
                return this.autoIssueField;
            }
            set
            {
                this.autoIssueField = value;
            }
        }

       
        public System.DateTime DepartDt
        {
            get
            {
                return this.departDtField;
            }
            set
            {
                this.departDtField = value;
            }
        }
    }
}
