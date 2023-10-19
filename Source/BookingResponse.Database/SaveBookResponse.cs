using ApiClient.Models;
using BookingResponse.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Script.Serialization;

namespace BookingResponse.Database
{
    public class SaveBookResponse
    {
        public static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["BookingResponseConnectionString"].ConnectionString ?? "";
        public void save(BookRequest request, BookResponse response)
        {

            try
            {
                if (response == null || request == null)
                    return;
                using (var db = new BookingDataDataContext(ConnectionString))
                {
                    string urlfull = HttpContext.Current.Request.Url.AbsoluteUri;
                    string path = HttpContext.Current.Request.Url.AbsolutePath;
                    string url = urlfull.Replace(path, "");

                    var requestJson = new JavaScriptSerializer().Serialize(request);
                    var responseJson = new JavaScriptSerializer().Serialize(response);
                    var BookResponse = new tblBookResponse()
                    {
                        BookingId = response.BookingId,
                        Status = response.Status,
                        ErrorCode = response.ErrorCode,
                        ErrorValue = response.ErrorValue,
                        ErrorField = response.ErrorField,
                        Message = response.Message,
                        Language = response.Language,
                        RequestJson = requestJson,
                        ResponseJson = responseJson,
                        SourceSite= url,
                    };
                    db.tblBookResponses.InsertOnSubmit(BookResponse);
                    db.SubmitChanges();
                    if (response.ListBooking != null)
                    {
                        foreach (var item in response.ListBooking)
                        {
                            var ckBookResponseBooking = new tblBookResponseBooking()
                            {
                                BookingId = response.BookingId,
                                Status = item.Status,
                                AutoIssue = item.AutoIssue,
                                Airline = item.Airline,
                                BookingCode = item.BookingCode,
                                GdsCode = item.GdsCode,
                                Flight = item.Flight,
                                Route = item.Route,
                                ErrorCode = item.ErrorCode,
                                ErrorMessage = item.ErrorMessage,
                                BookingImage = item.BookingImage,
                                ExpiryDate = item.ExpiryDate,
                                ExpiryDt = item.ExpiryDt,
                                ExpiryTime = (int)item.ExpiryTime,
                                ResponseTime = (decimal)item.ResponseTime,
                                System = item.System,
                                Price = (decimal)item.Price,
                                Difference = (decimal)item.Difference,
                                Session = item.Session,
                            };
                            db.tblBookResponseBookings.InsertOnSubmit(ckBookResponseBooking);
                            db.SubmitChanges();
                        }
                    }
                    using (var tran = new TransactionScope())
                    {
                        try
                        {
                            if (response.ListBooking != null)
                            {
                                foreach (var item in response.ListBooking)
                                {
                                    if (item.ListTicket != null)
                                    {
                                        foreach (var ticket in item.ListTicket)
                                        {
                                            var ckTicket = new tblBookResponseTicket
                                            {
                                                BookingId = response.BookingId,
                                                Index = ticket.Index,
                                                Airline = ticket.Airline,
                                                TicketNumber = ticket.TicketNumber,
                                                IssueDate = ticket.IssueDate,
                                                BookingCode = ticket.BookingCode,
                                                PassengerName = ticket.PassengerName,
                                                BookingFile = ticket.BookingFile,
                                                TicketImage = ticket.TicketImage,
                                                TotalPrice = (decimal)ticket.TotalPrice,
                                                Status = ticket.Status,
                                                ErrorMessage = ticket.ErrorMessage,
                                            };
                                            db.tblBookResponseTickets.InsertOnSubmit(ckTicket);
                                            db.SubmitChanges();
                                        }
                                    }
                                    if (item.FareData != null)
                                    {
                                        var ckBookResponseFareData = new tblBookResponseFareData()
                                        {
                                            BookingId = response.BookingId,
                                            FareDataId = item.FareData.FareDataId,
                                            Airline = item.FareData.Airline,
                                            Itinerary = item.FareData.Itinerary,
                                            Leg = item.FareData.Leg,
                                            Promo = item.FareData.Promo,
                                            Currency = item.FareData.Currency,
                                            System = item.FareData.System,
                                            //FareType = item.FareData.FareType,
                                            //CacheAge = item.FareData.CacheAge,
                                            Adt = item.FareData.Adt,
                                            Chd = item.FareData.Chd,
                                            Inf = item.FareData.Inf,
                                            FareAdt = (decimal)item.FareData.FareAdt,
                                            FareChd = (decimal)item.FareData.FareChd,
                                            FareInf = (decimal)item.FareData.FareInf,
                                            TaxAdt = (decimal)item.FareData.TaxAdt,
                                            TaxChd = (decimal)item.FareData.TaxChd,
                                            TaxInf = (decimal)item.FareData.TaxInf,
                                            FeeAdt = (decimal)item.FareData.FeeAdt,
                                            FeeChd = (decimal)item.FareData.FeeChd,
                                            FeeInf = (decimal)item.FareData.FeeInf,
                                            ServiceFeeAdt = (decimal)item.FareData.ServiceFeeAdt,
                                            ServiceFeeChd = (decimal)item.FareData.ServiceFeeChd,
                                            ServiceFeeInf = (decimal)item.FareData.ServiceFeeInf,
                                            TotalNetPrice = (decimal)item.FareData.TotalNetPrice,
                                            TotalServiceFee = (decimal)item.FareData.TotalServiceFee,
                                            TotalDiscount = (decimal)item.FareData.TotalDiscount,
                                            TotalCommission = (decimal)item.FareData.TotalCommission,
                                            TotalPrice = (decimal)item.FareData.TotalPrice,
                                        };
                                        db.tblBookResponseFareDatas.InsertOnSubmit(ckBookResponseFareData);
                                        db.SubmitChanges();
                                        if (item.FareData.ListFlight != null)
                                        {
                                            foreach (var flight in item.FareData.ListFlight)
                                            {
                                                var ckBookResponseFlight = new tblBookResponseFlight()
                                                {
                                                    BookingId = response.BookingId,
                                                    FareDataId = item.FareData.FareDataId,
                                                    FlightId = flight.FlightId,
                                                    Leg = flight.Leg,
                                                    Airline = flight.Airline,
                                                    Operating = flight.Operating,
                                                    StartPoint = flight.StartPoint,
                                                    EndPoint = flight.EndPoint,
                                                    StartDate = flight.StartDate,
                                                    EndDate = flight.EndDate,
                                                    StartDt = flight.StartDt,
                                                    EndDt = flight.EndDt,
                                                    FlightNumber = flight.FlightNumber,
                                                    StopNum = flight.StopNum,
                                                    HasDownStop = flight.HasDownStop,
                                                    Duration = flight.Duration,
                                                    NoRefund = flight.NoRefund,
                                                    GroupClass = flight.GroupClass,
                                                    FareClass = flight.FareClass,
                                                    FareBasis = flight.FareBasis,
                                                    SeatRemain = flight.SeatRemain,
                                                    Promo = flight.Promo,
                                                    FlightValue = flight.FlightValue,
                                                };
                                                db.tblBookResponseFlights.InsertOnSubmit(ckBookResponseFlight);
                                                db.SubmitChanges();
                                                if (flight.ListSegment != null)
                                                {
                                                    foreach (var segment in flight.ListSegment)
                                                    {
                                                        var ckBookResponseSegment = new tblBookResponseSegment()
                                                        {
                                                            SegmentId = segment.Id,
                                                            FlightId = flight.FlightId,
                                                            FareDataId = item.FareData.FareDataId,
                                                            BookingId = response.BookingId,
                                                            Airline = segment.Airline,
                                                            MarketingAirline = segment.MarketingAirline,
                                                            OperatingAirline = segment.OperatingAirline,
                                                            StartPoint = segment.StartPoint,
                                                            EndPoint = segment.EndPoint,
                                                            StartTime = segment.StartTime,
                                                            EndTime = segment.EndTime,
                                                            StartTm = segment.StartTm,
                                                            EndTm = segment.EndTm,
                                                            FlightNumber = segment.FlightNumber,
                                                            Duration = segment.Duration,
                                                            Class = segment.Class,
                                                            Seat = segment.Seat,
                                                            Plane = segment.Plane,
                                                            StartTerminal = segment.StartTerminal,
                                                            EndTerminal = segment.EndTerminal,
                                                            HasStop = segment.HasStop,
                                                            StopPoint = segment.StopPoint,
                                                            StopTime = (int)segment.StopTime,
                                                            DayChange = segment.DayChange,
                                                            StopOvernight = segment.StopOvernight,
                                                            ChangeStation = segment.ChangeStation,
                                                            ChangeAirport = segment.ChangeAirport,
                                                            LastItem = segment.LastItem,
                                                            HandBaggage = segment.HandBaggage,
                                                            AllowanceBaggage = segment.AllowanceBaggage,
                                                        };
                                                        db.tblBookResponseSegments.InsertOnSubmit(ckBookResponseSegment);
                                                        db.SubmitChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                tran.Complete();
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
