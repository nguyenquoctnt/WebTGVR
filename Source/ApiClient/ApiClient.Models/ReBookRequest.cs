using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class ReBookRequest : BaseRequest
    {
        private int bookingDetailIdField;
        private bool autoIssueField;
        public int BookingDetailId
        {
            get
            {
                return this.bookingDetailIdField;
            }
            set
            {
                this.bookingDetailIdField = value;
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
    }
}
