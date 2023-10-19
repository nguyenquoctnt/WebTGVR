using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class BookRequest : BaseRequest
    {

        private string bookTypeField;

        private bool useAgentContactField;

        private Contact contactField;

        private Passenger[] listPassengerField;

        private FareDataInfo[] listFareDataField;

        private string remarkField;

        private string paymentMethodField;

        private FlightDetail[] listFlightField;

       
        public string BookType
        {
            get
            {
                return this.bookTypeField;
            }
            set
            {
                this.bookTypeField = value;
            }
        }

       
        public bool UseAgentContact
        {
            get
            {
                return this.useAgentContactField;
            }
            set
            {
                this.useAgentContactField = value;
            }
        }

       
        public Contact Contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }

       
        public Passenger[] ListPassenger
        {
            get
            {
                return this.listPassengerField;
            }
            set
            {
                this.listPassengerField = value;
            }
        }

       
        public FareDataInfo[] ListFareData
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

       
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }

       
        public string PaymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }

       
        public FlightDetail[] ListFlight
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
