using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMContact
    {
        public bool Gender
        {
            get;set;
        }

        
        public string FirstName
        {
            get; set;
        }

        public string LastName
        {
            get; set;
        }

      
        public string Phone
        {
            get; set;
        }

       
        public string Email
        {
            get; set;
        }
        public string AgentEmail
        {
            get; set;
        }
        public string AgentPhone
        {
            get; set;
        }

        
        public string ContactEmail
        {
            get; set;
        }

      
        public string Address
        {
            get; set;
        }

    
        public string Company
        {
            get; set;
        }
    }
}