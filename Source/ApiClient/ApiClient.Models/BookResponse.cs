using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class BookResponse : BaseResponse
    {

        private int bookingIdField;
        private Booking[] listBookingField;
        public int BookingId
        {
            get
            {
                return this.bookingIdField;
            }
            set
            {
                this.bookingIdField = value;
            }
        }
        public Booking[] ListBooking
        {
            get
            {
                return this.listBookingField;
            }
            set
            {
                this.listBookingField = value;
            }
        }
        public bool Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorValue { get; set; }
        public string ErrorField { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}
