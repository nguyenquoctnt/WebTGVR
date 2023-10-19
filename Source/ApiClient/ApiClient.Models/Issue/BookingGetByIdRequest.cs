using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class BookingGetByIdRequest : BaseRequest
    {
        public int? BookingId { get; set; }
    }
}
