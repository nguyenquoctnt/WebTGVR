using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMPNR
    {
        public int OrderId
        {
            get;set;
        }

        public string BookingCode
        {
            get; set;
        }

     
        public string ErrorMessage
        {
            get; set;
        }

      
        public System.DateTime ExpiryDate
        {
            get; set;
        }

      
        public string Airline
        {
            get; set;
        }

    
        public string Status
        {
            get; set;
        }

      
        public string BookingImage
        {
            get; set;
        }

      
        public bool IssueTicket
        {
            get; set;
        }

     
        public string OldCode
        {
            get; set;
        }
    }
}