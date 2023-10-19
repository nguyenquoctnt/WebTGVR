using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMPassenger
    {
        public int Index
        {
            get; set;
        }
        public int Id
        {
            get; set;
        }


        public string FirstName
        {
            get; set;
        }


        public string LastName
        {
            get; set;
        }


        public string Type
        {
            get; set;
        }


        public bool Gender
        {
            get; set;
        }


        public string Email
        {
            get; set;
        }


        public string Phone
        {
            get; set;
        }


        public System.DateTime Birthday
        {
            get; set;
        }


        public System.DateTime PassportExpiryDate
        {
            get; set;

        }


        public string PassportIssueCountry
        {
            get; set;
        }


        public string PassportNumber
        {
            get; set;
        }


        public string BaggageDeparture
        {
            get; set;
        }
        public string BaggageDepartValue
        {
            get; set;
        }
        public decimal BaggageDepartPrice
        {
            get; set;
        }


        public string BaggageReturn
        {
            get; set;
        }
        public string BaggageReturnValue
        {
            get; set;
        }
        public decimal BaggageReturnPrice
        {
            get; set;
        }

    }
}