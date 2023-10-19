using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Constants;
using System.Web.Script.Serialization;
namespace VDMMutiline.Core
{
    public class LogMain
    {

        public static void LogBooking(int? bookingid, string UserCreate, string noidungcu,string noidungmoi,int LogBookingType)
        {
            BK_BookingDao BookingDao = new BK_BookingDao();
            BK_BookingDetailDao BookingDetailDao = new BK_BookingDetailDao();
            BK_PassengerDao PassengerDao = new BK_PassengerDao();
            BK_TicketDao TicketDao = new BK_TicketDao();
            LogBookingDao LogBookingDao = new LogBookingDao();
            var objbooking = BookingDao.GetInfo(bookingid ?? 0);
            if (objbooking != null)
            {
                var listbookingdetail = BookingDetailDao.Getbylistbybooking(objbooking.ID);
                var listpassenger = PassengerDao.GetbyBookingId(objbooking.ID);
                var listicket= TicketDao.GetlistbybookingId(objbooking.ID);
                var listicketdetail = TicketDao.GetDetailBybooking(objbooking.ID);
                string jsonbooking = new JavaScriptSerializer().Serialize(objbooking);
                string jsonbookingdetail = new JavaScriptSerializer().Serialize(listbookingdetail);
                string jsonlistpassenger = new JavaScriptSerializer().Serialize(listpassenger);
                string jsonlisticket = new JavaScriptSerializer().Serialize(listicket);
                string jsonlisticketdetail=  new JavaScriptSerializer().Serialize(listicketdetail);
                var ck = new tbl_LogBooking()
                {
                    Idbooking = objbooking.ID,
                    TypeAction= LogBookingType,
                    Bookingcode= objbooking.BookCode,
                    Madonhang= objbooking.rCode,
                    Ngaytao=DateTime.Now,
                    Nguoitao= !string.IsNullOrEmpty(UserCreate)? UserCreate: Constant.KL,
                    LastJsonBK_Booking= jsonbooking,
                    LastJsonBK_BookingDetail = jsonbookingdetail,
                    LastJsonBK_Passenger= jsonlistpassenger,
                    LastJsonBK_Ticket = jsonlisticket,
                    LastJSonBK_TicketDetail= jsonlisticketdetail,
                    Noidungcu= noidungcu,
                    Noidungmoi= noidungmoi,
                };
                LogBookingDao.Insert(ck);

            }

        }
   
    }
}