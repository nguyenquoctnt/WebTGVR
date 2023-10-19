using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class IssueResponse : BaseResponse
    {

        private Ticket[] listTicketField;

        private string bookingImageField;

        public Ticket[] ListTicket
        {
            get
            {
                return this.listTicketField;
            }
            set
            {
                this.listTicketField = value;
            }
        }

       
        public string BookingImage
        {
            get
            {
                return this.bookingImageField;
            }
            set
            {
                this.bookingImageField = value;
            }
        }
    }
}
