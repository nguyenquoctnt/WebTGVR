using BookingResponse.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingResponse.Database
{
    public class BookingData
    {
        public static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["BookingResponseConnectionString"].ConnectionString ?? "";

        public tblBookResponseBooking GetBookingInfoByCode(string BookingCode, string Airline)
        {
            using (var db = new BookingDataDataContext(ConnectionString))
            {
                return db.tblBookResponseBookings.FirstOrDefault(z => z.BookingCode == BookingCode.Trim().ToUpper() && z.Airline == Airline.Trim().ToUpper());
            }
        }
    }
}
