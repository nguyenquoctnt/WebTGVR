using ApiClient.Models.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Common
{

    public class BookingDTC
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        BookingResponse.Database.BookingData dataBooking = new BookingResponse.Database.BookingData();
        public SharedComponent.EntityInfo.BookingInfo GetBookingByCode(string code, string airline)
        {
            var bookingInfo = new SharedComponent.EntityInfo.BookingInfo();
            var getBookingId = dataBooking.GetBookingInfoByCode(code, airline);
            if (getBookingId != null)
            {
                var requestBooking = new BookingGetByIdRequest()
                {
                    BookingId = getBookingId.BookingId,
                    HeaderUser = GetKeySetting.HeaderUser,
                    HeaderPass = GetKeySetting.HeaderPassword,
                    AgentAccount = GetKeySetting.Username,
                    AgentPassword = GetKeySetting.Password,
                };
                var apiBooking = apiClient.bookingGetbyid(requestBooking);
                if(apiBooking!=null)
                {
                    bookingInfo.AirlineCode = airline;
                    bookingInfo.Booking = apiBooking.Booking;
                    if(bookingInfo.Booking !=null)
                    {
                        var requestbookingDetail = new BookingdetailGetbybookingidRequest()
                        {
                            BookingId = getBookingId.BookingId,
                            HeaderUser = GetKeySetting.HeaderUser,
                            HeaderPass = GetKeySetting.HeaderPassword,
                            AgentAccount = GetKeySetting.Username,
                            AgentPassword = GetKeySetting.Password,
                        };
                        var apiBookingDetail = apiClient.bookingDetailGetbybookingid(requestbookingDetail);
                        if(apiBookingDetail!=null)
                        {
                            bookingInfo.Tickets = apiBookingDetail.ListBookingDetail;
                        }
                        var requestbookingPassenger = new BookingPassengerGetbybookingidRequest()
                        {
                            BookingId = getBookingId.BookingId,
                            HeaderUser = GetKeySetting.HeaderUser,
                            HeaderPass = GetKeySetting.HeaderPassword,
                            AgentAccount = GetKeySetting.Username,
                            AgentPassword = GetKeySetting.Password,
                        };
                        var apiBookingPassger = apiClient.bookingPassengerGetbybookingid(requestbookingPassenger);
                        if(apiBookingPassger!=null)
                        {
                            bookingInfo.Passengers = apiBookingPassger.ListBookingPassenger;
                        }
                        var requestbookingbaggage = new BookingBaggageGetByBookingIdRequest()
                        {
                            BookingId = getBookingId.BookingId,
                            HeaderUser = GetKeySetting.HeaderUser,
                            HeaderPass = GetKeySetting.HeaderPassword,
                            AgentAccount = GetKeySetting.Username,
                            AgentPassword = GetKeySetting.Password,
                        };
                        var baggage =  apiClient.bookingBaggageGetByBookingId(requestbookingbaggage);
                        if (baggage != null)
                        {
                            bookingInfo.ListBookingBaggage = baggage.ListBookingBaggage;
                        }
                       
                    }
                }
            }
            else
            {
                bookingInfo = null;
            }
            return bookingInfo;
        }
    }
}