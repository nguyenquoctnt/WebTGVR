using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class SearchResponse : BaseResponse
    {

        private string flightTypeField;

        private string sessionField;

        private int itineraryField;

        private FareData[] listFareDataField;

       
        public string FlightType
        {
            get
            {
                return this.flightTypeField;
            }
            set
            {
                this.flightTypeField = value;
            }
        }

       
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

       
        public int Itinerary
        {
            get
            {
                return this.itineraryField;
            }
            set
            {
                this.itineraryField = value;
            }
        }

       
        public FareData[] ListFareData
        {
            get
            {
                return this.listFareDataField;
            }
            set
            {
                this.listFareDataField = value;
            }
        }
    }
}
