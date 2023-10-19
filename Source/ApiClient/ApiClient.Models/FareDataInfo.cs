using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FareDataInfo
    {

        private string sessionField;

        private int fareDataIdField;

        private bool autoIssueField;

        private FlightInfo[] listFlightField;

       
        public string Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }

       
        public int FareDataId
        {
            get
            {
                return this.fareDataIdField;
            }
            set
            {
                this.fareDataIdField = value;
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
        public string CAcode { get; set; }
       
        public FlightInfo[] ListFlight
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
