using ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.Biz;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.Models;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;

namespace VDMMutiline.SeachAndBook.Common
{
    public class CommonSaveTicket
    {
        public void saveDetail(BK_BookingDetail ck)
        {
            var dao = new BK_BookingDetailDao();
            dao.Insert(ck);
        }
        public int SaveTicket(int IDBooking, VDMFareDataInfo input, List<Booking> listbockok, int? TypeID)
        {
            var ck = new BK_Ticket
            {
                BookingID = IDBooking,
                FlightNo = input.FlightNumber,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Price = input.TotalPrice,
                FromCity = input.StartPoint,
                ToCity = input.EndPoint,
                Class = input.FareClass,
                ClassName = input.GroupClass,
                Code = input.Airline,
                IsDeleted = false,
                PriceBefore = input.PriceBefor,
                Tax = input.TotalPrice - input.PriceBefor,
                TypeID = TypeID,
                FareAdt = input.FareAdt,
                FareChd = input.FareChd,
                FareInf = input.FareInf,
                FeeAdt = input.FeeAdt,
                FeeChd = input.FeeChd,
                FeeInf = input.FeeInf,
                TaxAdt = input.TaxAdt,
                TaxChd = input.TaxChd,
                TaxInf = input.TaxInf,
                Duration = input.Duration,
                Promo=input.Promo
            };
            var objve = listbockok.Count > 1 ? listbockok.FirstOrDefault(z => z.Airline.Trim().ToUpper() == input.Airline.Trim().ToUpper()) : listbockok.FirstOrDefault();
            if (objve != null)
            {
                ck.CodeBook = objve.BookingCode;
                ck.Exdate = objve.ExpiryDate;
                ck.Eror = objve.ErrorMessage;
            }
            var dao = new BK_TicketDao();
            return dao.Insert(ck);
        }
        public int SaveTicket(BK_Ticket ck)
        {
            var dao = new BK_TicketDao();
            return dao.Insert(ck);
        }
        public int SaveTicketDetail(Segment AvailFlights, int? ticketid)
        {
            var ck = new BK_TicketDetail()
            {
                airlineCode = AvailFlights.Airline,
                flight = AvailFlights.FlightNumber,
                StartDate = AvailFlights.StartTime,
                EndDate = AvailFlights.EndTime,
                FromCity = AvailFlights.StartPoint,
                ToCity = AvailFlights.EndPoint,
                Class = AvailFlights.Class,
                TicketID = ticketid,
                Plane = AvailFlights.Plane,
                Duration = AvailFlights.Duration,
                StopTime = AvailFlights.StopTime,
            };
            var dao = new BK_TicketDao();
            return dao.InsertDetali(ck);
        }
        public int SaveTicketDetail(int? ticketid, VDMFareDataInfo AvailFlights)
        {
            var ck = new BK_TicketDetail()
            {
                airlineCode = AvailFlights.Airline,
                flight = AvailFlights.FlightNumber,
                StartDate = AvailFlights.StartDate,
                EndDate = AvailFlights.EndDate,
                FromCity = AvailFlights.StartPoint,
                ToCity = AvailFlights.EndPoint,
                Class = AvailFlights.FareClass,
                TicketID = ticketid,
                Plane = "",
                Duration = AvailFlights.Duration,
                StopTime = 0,
            };
            var dao = new BK_TicketDao();
            return dao.InsertDetali(ck);
        }
        public int SaveTicketDetail(BK_TicketDetail ck)
        {
            var dao = new BK_TicketDao();
            return dao.InsertDetali(ck);
        }
        public int SaveBooking(List<SettingUserInfo> listsetting, BK_Booking item, SeachParam param, string codegiamgia, string code, VDMUser userInfo)
        {
            decimal tiengiamgia = 0;
            double tonggia = 0;
            var VoucherCodeDao = new VoucherCodeDao();
            if (code != "FAIL" && !string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(codegiamgia.Trim()))
                {
                    var list = VoucherCodeDao.Checkbycode(codegiamgia, CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList());
                    if (list != null)
                    {
                        tiengiamgia = list.Valued.HasValue ? list.Valued.Value : 0;
                    }
                }
            }
            if (param.DepartureFlight != null)
            {
                tonggia += param.DepartureFlight.TotalPrice;
            }
            if (param.ReturnFlight != null)
            {
                tonggia += param.ReturnFlight.TotalPrice;
            }
            var dao = new BK_BookingDao();
            item.SessionID = param.DepartureFlight.SessionAll;
            item.Status = 1;
            item.Ways = param.Passengerlist.Count;
            item.TotalPrice = tonggia - (double)tiengiamgia;
            item.FromCity = param.DepartureFlight.StartPoint;
            item.ToCity = param.DepartureFlight.EndPoint;
            item.FlightDepart = param.DepartureFlight.FlightNumber;
            item.CreatedDate = DateTime.Now;
            item.IP = Utility.GetUserIPAddress();
            item.DiscountCode = codegiamgia;
            item.DiscountPrice = tiengiamgia;
            item.OutputFee = (decimal)tonggia;
            string userid = Constant.KL;

            if (userInfo != null)
            {
                userid = userInfo.UserName;
                item.UserCreate = userInfo.UserName;
            }
            else
            {
                var userdao = new AspNetUsersBiz();
                var paramuser = new AspNetUsersParam() { UrlDomain = SiteMuti.Getsitename() };
                userdao.GetInfobydomain(paramuser);
                if (paramuser.AspNetUsersInfo != null)
                {
                    item.UserCreate = paramuser.AspNetUsersInfo.UserName;
                }
            }
            if (userid == Constant.KL)
            {
                if (string.IsNullOrEmpty(item.Email))
                {
                    var listsys = listsetting;
                    var email = "";
                    var objmail = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
                    if (objmail != null)
                    {
                        email = objmail.Value;
                        item.Email = email;
                    }
                }
            }
            item.SourceSite = SiteMuti.Getsitename();
            if (param.ReturnFlight != null)
            {
                item.FlightReturn = param.ReturnFlight.FlightNumber;
            }
            if (code != "FAIL" && !string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(codegiamgia.Trim()))
                {
                    var list = VoucherCodeDao.Checkbycode(codegiamgia, CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList());
                    if (list != null)
                    {
                        VoucherCodeDao.UpdateStatus(list.Id, true, code);
                    }
                }
            }
            item.UserId = userid;
            var idbk = dao.Insert(item);
            var paymentinfo = new tbl_PaymentInfo();
            paymentinfo.IdHinhThuc = Constant.Hinhthucthanhtoav2.chuyenkhoan;
            paymentinfo.IdBooking = idbk;
            dao.UpdatePayment(paymentinfo);
            return idbk;
        }
        public int SaveBookingInternational(List<SettingUserInfo> listsetting, BK_Booking item, SeachParam param, string codegiamgia, string code, VDMUser userInfo)
        {
            decimal tiengiamgia = 0;
            var VoucherCodeDao = new VoucherCodeDao();
            if (code != "FAIL" && !string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(codegiamgia.Trim()))
                {

                    var list = VoucherCodeDao.Checkbycode(codegiamgia, CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList());
                    if (list != null)
                    {
                        tiengiamgia = list.Valued.HasValue ? list.Valued.Value : 0;
                    }
                }
            }
            var info = param.FareDataInfo;
            // Chiều đi
            if (param.FareDataInfo.ListFlight.Count() > 0)
            {

                var input = param.FareDataInfo.ListFlight.FirstOrDefault(
                      z => z.FlightId == param.DepartureFlightId);
                var inputdetail = input.ListSegment.FirstOrDefault();
                if (input != null && inputdetail != null)
                {

                    var dao = new BK_BookingDao();
                    item.SessionID = info.SessionId;
                    item.Status = 1;
                    item.Ways = param.Passengerlist.Count;
                    item.TotalPrice = param.FareDataInfo.TotalPrice - (double)tiengiamgia;
                    item.FromCity = param.DepartureAirportCode;
                    item.ToCity = param.DestinationAirportCode;
                    item.DiscountCode = codegiamgia;
                    item.DiscountPrice = tiengiamgia;
                    item.OutputFee = (decimal)param.FareDataInfo.TotalPrice;
                    item.FlightDepart = inputdetail.FlightNumber;
                    item.CreatedDate = DateTime.Now;
                    item.IP = Utility.GetUserIPAddress();
                    string userid = Constant.KL;
                    if (userInfo != null)
                    {
                        userid = userInfo.UserName;
                        item.UserCreate = userInfo.UserName;
                    }
                    else
                    {
                        var userdao = new AspNetUsersBiz();
                        var paramuser = new AspNetUsersParam() { UrlDomain = SiteMuti.Getsitename() };
                        userdao.GetInfobydomain(paramuser);
                        if (paramuser.AspNetUsersInfo != null)
                        {
                            item.UserCreate = paramuser.AspNetUsersInfo.UserName;
                        }
                    }
                    if (userid == Constant.KL)
                    {
                        if (string.IsNullOrEmpty(item.Email))
                        {
                            var listsys = listsetting;
                            var email = "";
                            var objmail = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
                            if (objmail != null)
                            {
                                email = objmail.Value;
                                item.Email = email;
                            }
                        }
                    }
                    item.SourceSite = SiteMuti.Getsitename();
                    item.UserId = userid;
                    if (param.Itinerary > 1)
                    {
                        var inputReturn = param.FareDataInfo.ListFlight.FirstOrDefault(
                            z => z.FlightId == param.ReturnFlightId);
                        var inputdetailReturn = inputReturn.ListSegment.FirstOrDefault();
                        if (inputReturn != null && inputdetailReturn != null)
                        {
                            item.FlightReturn = inputdetailReturn.FlightNumber;
                        }
                    }

                    if (code != "FAIL" && !string.IsNullOrEmpty(code))
                    {
                        if (!string.IsNullOrEmpty(codegiamgia.Trim()))
                        {
                            var list = VoucherCodeDao.Checkbycode(codegiamgia, CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList());
                            if (list != null)
                            {
                                VoucherCodeDao.UpdateStatus(list.Id, true, code);
                            }
                        }
                    }
                    var idbk = dao.Insert(item);
                    var paymentinfo = new tbl_PaymentInfo();
                    paymentinfo.IdHinhThuc = Constant.Hinhthucthanhtoav2.chuyenkhoan;
                    paymentinfo.IdBooking = idbk;
                    dao.UpdatePayment(paymentinfo);
                    return idbk;
                }
            }
            return 0;
        }
        public int SaveBookingInternational(List<SettingUserInfo> listsetting, BK_Booking item, SeachParam param, VDMUser userInfo)
        {
            var info = param.FareDataInfo;
            // Chiều đi
            if (param.FareDataInfo.ListFlight.Count() > 0)
            {

                var input = param.FareDataInfo.ListFlight.FirstOrDefault(
                      z => z.FlightId == param.DepartureFlightId);
                var inputdetail = input.ListSegment.FirstOrDefault();
                if (input != null && inputdetail != null)
                {
                    //   var infodetail = info.ListAvailFlt.FirstOrDefault();
                    var dao = new BK_BookingDao();
                    item.SessionID = info.SessionId;
                    item.Status = 1;
                    item.Ways = param.Passengerlist.Count;
                    item.TotalPrice = param.FareDataInfo.TotalPrice;
                    item.FromCity = param.DepartureAirportCode;
                    item.ToCity = param.DestinationAirportCode;
                    item.IP = Utility.GetUserIPAddress();
                    item.FlightDepart = inputdetail.FlightNumber;
                    item.CreatedDate = DateTime.Now;
                    string userid = Constant.KL;
                    if (userInfo != null)
                    {
                        userid = userInfo.UserName;
                        item.UserCreate = userInfo.UserName;
                    }
                    else
                    {
                        var userdao = new AspNetUsersBiz();
                        //var obj = userdao.GetInfoByEmail(item.Email);
                        //userid = obj != null ? obj.UserName : Constant.KL;
                        var paramuser = new AspNetUsersParam() { UrlDomain = SiteMuti.Getsitename() };
                        userdao.GetInfobydomain(paramuser);
                        if (paramuser.AspNetUsersInfo != null)
                        {
                            item.UserCreate = paramuser.AspNetUsersInfo.UserName;
                        }
                    }
                    if (userid == Constant.KL)
                    {
                        if (string.IsNullOrEmpty(item.Email))
                        {
                            var listsys = listsetting;
                            var email = "";
                            var objmail = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
                            if (objmail != null)
                            {
                                email = objmail.Value;
                                item.Email = email;
                            }
                        }
                    }
                    item.SourceSite = SiteMuti.Getsitename();
                    item.UserId = userid;
                    if (param.Itinerary > 1)
                    {
                        var inputReturn = param.FareDataInfo.ListFlight.FirstOrDefault(
                            z => z.FlightId == param.ReturnFlightId);
                        var inputdetailReturn = inputReturn.ListSegment.FirstOrDefault();
                        if (inputReturn != null && inputdetailReturn != null)
                        {
                            item.FlightReturn = inputdetailReturn.FlightNumber;
                        }
                    }
                    var idbk = dao.Insert(item);
                    var paymentinfo = new tbl_PaymentInfo();
                    paymentinfo.IdHinhThuc = Constant.Hinhthucthanhtoav2.chuyenkhoan;
                    paymentinfo.IdBooking = idbk;
                    dao.UpdatePayment(paymentinfo);
                    return idbk;
                }
            }
            return 0;
        }
        public int SavePassengerInternational(BK_PassengerInfo item, SeachParam param)
        {
            var fullname = item.FullName.Split(' ');
            string LastName = "";
            for (int i = 1; i < fullname.Count(); i++)
            {
                LastName += fullname[i] + " ";
            }
            var dbItem = new BK_Passenger
            {
                TypeID = item.TypeID,
                FirstName = fullname.Count() >= 1 ? Utility.ConvertToUnsign(fullname[0]) : "",

                Sex = item.Sex,
                Name = Utility.ConvertToUnsign(LastName),
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone,
                City = param.DepartureAirportCode,
                BaggageDepartID = item.BaggageDepartID,
                BaggageReturnID = item.BaggageReturnID,
                BaggageDepartValue = (item.BaggageDepartValue),
                BaggageReturnValue = (item.BaggageReturnValue),
                BaggageDepartPrice = item.BaggageDepartPrice,
                BaggageReturnPrice = item.BaggageReturnPrice,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(item.Nam) && !string.IsNullOrEmpty(item.Thang) && !string.IsNullOrEmpty(item.Ngay))
            {
                dbItem.Birthday = item.TypeID != Constant.TypePassenger.NguoiLon ? new DateTime(int.Parse(item.Nam), int.Parse(item.Thang), int.Parse(item.Ngay)) : item.Birthday;
            }
            else
            {
                dbItem.Birthday = item.Birthday;
            }
            if (param.BaggageInfo != null)
            {

                if (!string.IsNullOrEmpty(item.BaggageDepartID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DepartureAirportCode + param.DestinationAirportCode) && z.Code == item.BaggageDepartID);
                    if (objpass != null)
                    {
                        dbItem.BaggageDepartValue = objpass.Value;
                        dbItem.BaggageDepartPrice = Utility.ConvertToDecimal(objpass.Price.ToString());
                    }
                }
                if (!string.IsNullOrEmpty(item.BaggageReturnID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DestinationAirportCode + param.DepartureAirportCode) && z.Code == item.BaggageReturnID);
                    if (objpass != null)
                    {
                        dbItem.BaggageReturnValue = objpass.Value;
                        dbItem.BaggageReturnPrice = Utility.ConvertToDecimal(objpass.Price.ToString());

                    }
                }
            }
            var dao = new BK_PassengerDao();
            return dao.Insert(dbItem);
        }
        public int SavePassenger(BK_PassengerInfo item, SeachParam param)
        {
            var fullname = item.FullName.Split(' ');
            string LastName = "";
            for (int i = 1; i < fullname.Count(); i++)
            {
                LastName += fullname[i] + " ";
            }
            var dbItem = new BK_Passenger
            {
                TypeID = item.TypeID,
                FirstName = fullname.Count() >= 1 ? Utility.ConvertToUnsign(fullname[0]) : "",

                Sex = item.Sex,
                Name = Utility.ConvertToUnsign(LastName),
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone,
                City = param.DepartureFlight.StartPoint,
                BaggageDepartID = item.BaggageDepartID,
                BaggageReturnID = item.BaggageReturnID,
                BaggageDepartPrice = item.BaggageDepartPrice,
                BaggageReturnPrice = item.BaggageReturnPrice,
                IsDeleted = false
            };
            if (!string.IsNullOrEmpty(item.Nam) && !string.IsNullOrEmpty(item.Thang) && !string.IsNullOrEmpty(item.Ngay))
            {
                dbItem.Birthday = item.TypeID != Constant.TypePassenger.NguoiLon ? new DateTime(int.Parse(item.Nam), int.Parse(item.Thang), int.Parse(item.Ngay)) : item.Birthday;
            }
            else
            {
                dbItem.Birthday = item.Birthday;
            }
            if (param.DepartureFlight != null)
            {
                if (param.DepartureFlight.Airline == "QH")
                {
                    if (param.DepartureFlight.FareClass.ToLower() != "BAMBOOECO".ToLower() && param.DepartureFlight.FareClass?.ToLower() != "ECONOMYSAVERMAX".ToLower())
                    {
                        dbItem.BaggageDepartValue = (item.TypeID == Constant.TypePassenger.EmBe ? "" : "20");
                    }
                    else
                    {
                        dbItem.BaggageDepartValue = item.BaggageDepartValue;
                    }
                }
                else if (param.DepartureFlight.Airline == "VN")
                {
                    if (((param.DepartureFlight.Ishow23KgVN ?? false)==true) && (param.DepartureFlight.IsVNJQ0Kg ?? false) == false)
                    {
                        dbItem.BaggageDepartValue = (item.TypeID == Constant.TypePassenger.EmBe ? "10" : "23");
                    }
                    else dbItem.BaggageDepartValue = "0";
                }
                else
                {
                    dbItem.BaggageDepartValue = item.BaggageDepartValue;
                }
            }
            if (param.ReturnFlight != null)
            {
                if (param.ReturnFlight.Airline == "QH")
                {
                    if (param.ReturnFlight.FareClass?.ToLower() != "BAMBOOECO".ToLower() && param.ReturnFlight.FareClass?.ToLower() != "ECONOMYSAVERMAX".ToLower() )
                    {
                        dbItem.BaggageReturnValue = (item.TypeID == Constant.TypePassenger.EmBe ? "" : "20");
                    }
                    else
                    {
                        dbItem.BaggageReturnValue = item.BaggageReturnValue;
                    }
                }
                else if (param.ReturnFlight.Airline == "VN")
                {
                    if (((param.ReturnFlight.Ishow23KgVN ?? false)==true) && (param.ReturnFlight.IsVNJQ0Kg ?? false) == false)
                    {
                        dbItem.BaggageReturnValue = (item.TypeID == Constant.TypePassenger.EmBe ? "10" : "23");
                    }
                    else dbItem.BaggageReturnValue = "0";
                }
                else
                {
                    dbItem.BaggageReturnValue = item.BaggageReturnValue;
                }
            }
            if (param.BaggageInfo != null)
            {

                if (!string.IsNullOrEmpty(item.BaggageDepartID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DepartureFlight.StartPoint + param.DepartureFlight.EndPoint) && z.Code == item.BaggageDepartID);
                    if (objpass != null)
                    {
                        dbItem.BaggageDepartValue = objpass.Value;
                        dbItem.BaggageDepartPrice = Utility.ConvertToDecimal(objpass.Price.ToString());
                    }
                }
                if (!string.IsNullOrEmpty(item.BaggageReturnID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.ReturnFlight.StartPoint + param.ReturnFlight.EndPoint) && z.Code == item.BaggageReturnID);
                    if (objpass != null)
                    {
                        dbItem.BaggageReturnValue = objpass.Value;
                        dbItem.BaggageReturnPrice = Utility.ConvertToDecimal(objpass.Price.ToString());

                    }
                }
            }
            var dao = new BK_PassengerDao();
            return dao.Insert(dbItem);
        }
    }
}