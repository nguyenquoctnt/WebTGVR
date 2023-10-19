using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models.Issue
{
    public class BookingPassenger
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int Index { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public double Fare { get; set; }
        public double Tax { get; set; }
        public double Fee { get; set; }
        public double ServiceFee { get; set; }
        public double BaggageFee { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpirationDate { get; set; }
        public string Nationality { get; set; }
        public string IssueCountry { get; set; }
        public List<BookingBaggage> ListBaggage { get; set; }
    }
}
