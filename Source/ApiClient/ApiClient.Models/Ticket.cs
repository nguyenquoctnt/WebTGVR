using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class Ticket
    {

        private int indexField;

        private string airlineField;

        private string ticketNumberField;

        private System.DateTime issueDateField;

        private string bookingCodeField;

        private string passengerNameField;

        private string bookingFileField;

        private string ticketImageField;

        private double totalPriceField;

        private string statusField;

        private string errorMessageField;

       
        public int Index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
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

       
        public string TicketNumber
        {
            get
            {
                return this.ticketNumberField;
            }
            set
            {
                this.ticketNumberField = value;
            }
        }

       
        public System.DateTime IssueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }

       
        public string BookingCode
        {
            get
            {
                return this.bookingCodeField;
            }
            set
            {
                this.bookingCodeField = value;
            }
        }

       
        public string PassengerName
        {
            get
            {
                return this.passengerNameField;
            }
            set
            {
                this.passengerNameField = value;
            }
        }

       
        public string BookingFile
        {
            get
            {
                return this.bookingFileField;
            }
            set
            {
                this.bookingFileField = value;
            }
        }

       
        public string TicketImage
        {
            get
            {
                return this.ticketImageField;
            }
            set
            {
                this.ticketImageField = value;
            }
        }

       
        public double TotalPrice
        {
            get
            {
                return this.totalPriceField;
            }
            set
            {
                this.totalPriceField = value;
            }
        }

       
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

       
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
    }
}
