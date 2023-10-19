using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Ultilities;
using ApiClient.Models;
using System.Threading.Tasks;

namespace VDMMutiline.SeachAndBook.Controllers
{
    public class SeachHomeController : PublishController
    {
        private static readonly ILog log =
              LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SeachAndBookCommon SeachAndBookCommon = new SeachAndBookCommon();
        Common.CommonSaveTicket saveTicket = new Common.CommonSaveTicket();
        Common.CommonTicket commonTicket = new Common.CommonTicket();
        #region Seach Home
        public ActionResult Seach()
        {
            Session[Constant.SessionSeach] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Seach(FormCollection forminput)
        {
            Session[Constant.SessionSeach] = null;
            var inputItinerary = forminput.GetValue("ItineraryType");
            var Itinerary = inputItinerary != null ? Utility.ConvertToInt(inputItinerary.AttemptedValue) : 0;
            var inputDepartureAirportCode = forminput.GetValue("hdfStartPoint");
            string DepartureAirportCode = inputDepartureAirportCode != null
                ? inputDepartureAirportCode.AttemptedValue
                : string.Empty;
            var inputDestinationAirportCode = forminput.GetValue("hdfEndPoint");
            string DestinationAirportCode = inputDestinationAirportCode != null
                ? inputDestinationAirportCode.AttemptedValue
                : string.Empty;
            var inputDepartureDate = forminput.GetValue("txtDepartureDate");
            string DepartureDate = inputDepartureDate != null ? inputDepartureDate.AttemptedValue : string.Empty;
            var txtReturnDate = forminput.GetValue("txtReturnDate");
            string ReturnDate = txtReturnDate != null ? txtReturnDate.AttemptedValue : string.Empty;
            var ddlAdult = forminput.GetValue("ddlAdult");
            var Adult = ddlAdult != null ? Utility.ConvertToInt(ddlAdult.AttemptedValue) : 0;
            var ddlChild = forminput.GetValue("ddlChild");
            var Children = ddlChild != null ? Utility.ConvertToInt(ddlChild.AttemptedValue) : 0;
            var ddlInfant = forminput.GetValue("ddlInfant");
            var Infant = ddlInfant != null ? Utility.ConvertToInt(ddlInfant.AttemptedValue) : 0;
            var lLanguageBarCurrency = forminput.GetValue("lLanguageBarCurrency");
            string Currency = lLanguageBarCurrency != null ? lLanguageBarCurrency.AttemptedValue : string.Empty;
            var checktimvere = forminput.GetValue("checktimvere");
            string timvere = checktimvere != null ? checktimvere.AttemptedValue : string.Empty;
            if (timvere.ToUpper() == "ON")
            {
                var ddlStartMonth = forminput.GetValue("ddlStartMonth");
                string StartMonth = ddlStartMonth != null ? ddlStartMonth.AttemptedValue : string.Empty;
                var ddlEndMonth = forminput.GetValue("ddlEndMonth");
                string EndMonth = ddlEndMonth != null ? ddlEndMonth.AttemptedValue : string.Empty;
                if (ddlStartMonth == null)
                {
                    var DepartureDatespl = DepartureDate.Split('/');
                    if (DepartureDatespl.Length > 2)
                        StartMonth = DepartureDatespl[1] + "/" + DepartureDatespl[2];
                }
                if (Itinerary > 1)
                {
                    if (ddlEndMonth == null)
                    {
                        var EndMonthpl = ReturnDate.Split('/');
                        if (EndMonthpl.Length > 2)
                            EndMonth = EndMonthpl[1] + "/" + EndMonthpl[2];
                    }
                }
                return RedirectToAction("SeachMain", "SeachHome", new
                {
                    Itinerary = Itinerary.HasValue ? Itinerary.Value : 0,
                    DepartureAirportCode = DepartureAirportCode,
                    DestinationAirportCode = DestinationAirportCode,
                    Adult = Adult.HasValue ? Adult.Value : 0,
                    Children = Children.HasValue ? Children.Value : 0,
                    Infant = Infant.HasValue ? Infant.Value : 0,
                    AirlineCode = string.Empty,
                    Currency = Currency,
                    older = string.Empty,
                    typeolder = string.Empty,
                    StartMonth = StartMonth,
                    EndMonth = EndMonth
                });
            }
            else
            {
                return RedirectToAction("SeachMain", "SeachHome", new
                {
                    Itinerary = Itinerary.HasValue ? Itinerary.Value : 0,
                    DepartureAirportCode = DepartureAirportCode,
                    DestinationAirportCode = DestinationAirportCode,
                    DepartureDate = DepartureDate,
                    ReturnDate = ReturnDate,
                    Adult = Adult.HasValue ? Adult.Value : 0,
                    Children = Children.HasValue ? Children.Value : 0,
                    Infant = Infant.HasValue ? Infant.Value : 0,
                    AirlineCode = string.Empty,
                    Currency = Currency,
                    older = string.Empty,
                    typeolder = string.Empty,
                });
            }
        }
        [HttpPost]
        public ActionResult SeachMini(FormCollection forminput)
        {
            Session[Constant.SessionSeach] = null;
            var inputItinerary = forminput.GetValue("ItineraryTypeMini");
            var Itinerary = inputItinerary != null ? Utility.ConvertToInt(inputItinerary.AttemptedValue) : 0;
            var inputDepartureAirportCode = forminput.GetValue("hdfStartPointMini");
            string DepartureAirportCode = inputDepartureAirportCode != null
                ? inputDepartureAirportCode.AttemptedValue
                : string.Empty;
            var inputDestinationAirportCode = forminput.GetValue("hdfEndPointMini");
            string DestinationAirportCode = inputDestinationAirportCode != null
                ? inputDestinationAirportCode.AttemptedValue
                : string.Empty;
            var inputDepartureDate = forminput.GetValue("txtDepartureDateMini");
            string DepartureDate = inputDepartureDate != null ? inputDepartureDate.AttemptedValue : string.Empty;
            var txtReturnDate = forminput.GetValue("txtReturnDateMini");
            string ReturnDate = txtReturnDate != null ? txtReturnDate.AttemptedValue : string.Empty;
            var ddlAdult = forminput.GetValue("ddlAdultMini");
            var Adult = ddlAdult != null ? Utility.ConvertToInt(ddlAdult.AttemptedValue) : 0;
            var ddlChild = forminput.GetValue("ddlChildMini");
            var Children = ddlChild != null ? Utility.ConvertToInt(ddlChild.AttemptedValue) : 0;
            var ddlInfant = forminput.GetValue("ddlInfantMini");
            var Infant = ddlInfant != null ? Utility.ConvertToInt(ddlInfant.AttemptedValue) : 0;
            var lLanguageBarCurrency = forminput.GetValue("lLanguageBarCurrencyMini");
            string Currency = lLanguageBarCurrency != null ? lLanguageBarCurrency.AttemptedValue : string.Empty;
            var checktimvere = forminput.GetValue("checktimvereMini");
            string timvere = checktimvere != null ? checktimvere.AttemptedValue : string.Empty;
            if (timvere.ToUpper() == "ON")
            {
                var ddlStartMonth = forminput.GetValue("ddlStartMonthMini");
                string StartMonth = ddlStartMonth != null ? ddlStartMonth.AttemptedValue : string.Empty;
                var ddlEndMonth = forminput.GetValue("ddlEndMonthMini");
                string EndMonth = ddlEndMonth != null ? ddlEndMonth.AttemptedValue : string.Empty;
                if (ddlStartMonth == null)
                {
                    var DepartureDatespl = DepartureDate.Split('/');
                    if (DepartureDatespl.Length > 2)
                        StartMonth = DepartureDatespl[1] + "/" + DepartureDatespl[2];
                }
                if (Itinerary > 1)
                {
                    if (ddlEndMonth == null)
                    {
                        var EndMonthpl = ReturnDate.Split('/');
                        if (EndMonthpl.Length > 2)
                            EndMonth = EndMonthpl[1] + "/" + EndMonthpl[2];
                    }
                }
                return RedirectToAction("SeachMain", "SeachHome", new
                {
                    Itinerary = Itinerary.HasValue ? Itinerary.Value : 0,
                    DepartureAirportCode = DepartureAirportCode,
                    DestinationAirportCode = DestinationAirportCode,
                    Adult = Adult.HasValue ? Adult.Value : 0,
                    Children = Children.HasValue ? Children.Value : 0,
                    Infant = Infant.HasValue ? Infant.Value : 0,
                    AirlineCode = string.Empty,
                    Currency = Currency,
                    older = string.Empty,
                    typeolder = string.Empty,
                    StartMonth = StartMonth,
                    EndMonth = EndMonth
                });
            }
            else
            {
                return RedirectToAction("SeachMain", "SeachHome", new
                {
                    Itinerary = Itinerary.HasValue ? Itinerary.Value : 0,
                    DepartureAirportCode = DepartureAirportCode,
                    DestinationAirportCode = DestinationAirportCode,
                    DepartureDate = DepartureDate,
                    ReturnDate = ReturnDate,
                    Adult = Adult.HasValue ? Adult.Value : 0,
                    Children = Children.HasValue ? Children.Value : 0,
                    Infant = Infant.HasValue ? Infant.Value : 0,
                    AirlineCode = string.Empty,
                    Currency = Currency,
                    older = string.Empty,
                    typeolder = string.Empty,
                });
            }
        }
        public ActionResult SeachMain(int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
            string DepartureDate, string ReturnDate,
            int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder,
            string StopNum,
            string StartMonth, string EndMonth)
        {
            var param = new SeachParam
            {
                Itinerary = Itinerary.HasValue ? Itinerary.Value : 0,
                DepartureAirportCode = DepartureAirportCode,
                DestinationAirportCode = DestinationAirportCode,
                DepartureDate = DepartureDate,
                ReturnDate = ReturnDate,
                Adult = Adult.HasValue ? Adult.Value : 0,
                Children = Children.HasValue ? Children.Value : 0,
                Infant = Infant.HasValue ? Infant.Value : 0,
                AirlineCode = AirlineCode ?? "",
                Currency = Currency ?? "",
                older = older ?? "",
                typeolder = typeolder ?? "",
                StopNum = StopNum,
                StartMonth = StartMonth,
                EndMonth = EndMonth
            };
            return View(param);
        }
        #endregion
        #region seach trong nước
        public async Task<ActionResult> SeachValue(int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
            string DepartureDate, string ReturnDate,
            int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder)
        {
            var daoip = new BlockIPDao();
            if (!daoip.Checkblock(Utility.GetUserIPAddress()))
            {
                return RedirectToAction("block");
            }
            var listsetting = GetSettingByDomainAndUser(SiteMuti.Getsitename());
            var param = await SeachAndBookCommon.seachTrongNuocAsync(listsetting, Itinerary.HasValue ? Itinerary.Value : 0,
                DepartureAirportCode,
                 DestinationAirportCode,
                 DepartureDate,
                  ReturnDate,
                   Adult.HasValue ? Adult.Value : 0,
                   Children.HasValue ? Children.Value : 0,
                 Infant.HasValue ? Infant.Value : 0,
                  AirlineCode ?? "",
                 Currency ?? "",
                  older ?? "",
                   typeolder ?? ""
                                );
            Session[Constant.SessionSeach] = param;
            Session[Constant.SessionSeachBackup] = param;
            return View(param);
        }
        [HttpPost]
        public ActionResult SeachValue(SeachParam param, FormCollection forminput)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var hdfSelectedDepartFlt = forminput.GetValue("hdfSelectedDepartFlt");
            string SelectedDepartFlt = hdfSelectedDepartFlt != null ? hdfSelectedDepartFlt.AttemptedValue : string.Empty;
            var hdfSelectedReturnFlt = forminput.GetValue("hdfSelectedReturnFlt");
            string SelectedReturnFlt = hdfSelectedReturnFlt != null ? hdfSelectedReturnFlt.AttemptedValue : string.Empty;
            var hdhCodeReturn = forminput.GetValue("hdhCodeReturn");
            string CodeReturn = hdhCodeReturn != null ? hdhCodeReturn.AttemptedValue : string.Empty;
            var hdhCodeDepart = forminput.GetValue("hdhCodeDepart");
            string CodeDepart = hdhCodeDepart != null ? hdhCodeDepart.AttemptedValue : string.Empty;
            if (!string.IsNullOrEmpty(SelectedDepartFlt) || !string.IsNullOrEmpty(SelectedReturnFlt))
            {
                if (Session[Constant.SessionSeach] != null)
                {
                    var idCodeReturn = SelectedReturnFlt;
                    var idDepartFlt = SelectedDepartFlt;
                    var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                    SeachRd.DepartureFlight = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataIdValue == idDepartFlt);
                    if (!string.IsNullOrEmpty(SelectedReturnFlt))
                        SeachRd.ReturnFlight = SeachRd.ReturnFlights.FirstOrDefault(z => z.FareDataIdValue == idCodeReturn);
                    Session[Constant.SessionSeach] = SeachRd;
                    Session[Constant.SessionSeachBackup] = SeachRd;
                    return RedirectToAction("Bookticket", "SeachHome", new
                    {
                        CodeReturn = CodeReturn,
                        CodeDepart = CodeDepart,
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(param);
        }
        #endregion
        #region SeachQuocTe
        public async Task<ActionResult> SeachInternational(int? Itinerary, string DepartureAirportCode,
           string DestinationAirportCode, string DepartureDate, string ReturnDate,
           int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder,
           string StopNum)
        {
            var listsetting = GetSettingByDomainAndUser(SiteMuti.Getsitename());
            var param = await SeachAndBookCommon.SeachQuocTe(listsetting,
                 Itinerary.HasValue ? Itinerary.Value : 0,
                DepartureAirportCode,
               DestinationAirportCode,
                DepartureDate,
                ReturnDate,
               Adult.HasValue ? Adult.Value : 0,
                Children.HasValue ? Children.Value : 0,
                Infant.HasValue ? Infant.Value : 0,
                AirlineCode ?? "",
                Currency ?? "",
               older ?? "",
                typeolder ?? "",
                StopNum);
            Session[Constant.SessionSeach] = param;
            Session[Constant.SessionSeachBackup] = param;
            return View(param);

        }
        [HttpPost]
        public ActionResult SeachInternational(SeachParam param, FormCollection forminput)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var hdfSearchParams = forminput.GetValue("hdfSearchParams");
            string SearchParams = hdfSearchParams != null ? hdfSearchParams.AttemptedValue : string.Empty;
            var hdfFareDataId = forminput.GetValue("hdfFareDataId");
            string FareDataId = hdfFareDataId != null ? hdfFareDataId.AttemptedValue : string.Empty;
            var hdhDepartureFlightId = forminput.GetValue("hdhDepartureFlightId");
            string DepartureFlightId = hdhDepartureFlightId != null ? hdhDepartureFlightId.AttemptedValue : string.Empty;
            var hdfReturnFlightId = forminput.GetValue("hdfReturnFlightId");
            string ReturnFlightId = hdfReturnFlightId != null ? hdfReturnFlightId.AttemptedValue : string.Empty;
            if (!string.IsNullOrEmpty(DepartureFlightId) || !string.IsNullOrEmpty(ReturnFlightId))
            {
                if (Session[Constant.SessionSeach] != null)
                {
                    var FareDataIdvl = FareDataId;
                    var idCodeReturn = Utility.ConvertToInt(ReturnFlightId);
                    var idDepartFlt = Utility.ConvertToInt(DepartureFlightId);
                    var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                    SeachRd.FareDataInfo = SeachRd.FareDataList.FirstOrDefault(z => z.FareDataIdValue == FareDataIdvl);
                    SeachRd.DepartureFlightId = idDepartFlt.HasValue ? idDepartFlt.Value : 0;
                    SeachRd.ReturnFlightId = idCodeReturn.HasValue ? idCodeReturn.Value : 0;
                    SeachRd.SearchParams = SearchParams;
                    SeachRd.FareDataIdValue = FareDataIdvl;
                    Session[Constant.SessionSeach] = SeachRd;
                    Session[Constant.SessionSeachBackup] = SeachRd;
                    return RedirectToAction("BookInternational", "SeachHome", new
                    {

                    });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(param);
        }
        public ActionResult GetFareRuleInfo(string seachParam, string fareid)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            var FareData = SeachRd.FareDataList.FirstOrDefault(z => z.FareDataIdValue == fareid);
            if (FareData != null)
            {
                string htmls = SeachAndBookCommon.GetDieuKienQuocTe(fareid, FareData.Itinerary, seachParam);
                ViewBag.contenFareRule = htmls;
            }
            return View(FareData);
        }
        public ActionResult GetDetailFare(string fareid)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            var FareData = SeachRd.FareDataList.FirstOrDefault(z => z.FareDataIdValue == fareid);
            return View(FareData);
        }
        public ActionResult GetDetailFlights(string fareid, int? flightvalue, int? flightReturnId)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            var FareData = SeachRd.FareDataList.FirstOrDefault(z => z.FareDataIdValue == fareid);
            if (FareData != null)
            {
                var flight = FareData.ListFlight.FirstOrDefault(z => z.FlightId == flightvalue);
                if (flightReturnId.HasValue)
                    ViewBag.flightReturn = FareData.ListFlight.FirstOrDefault(z => z.FlightId == flightReturnId);
                return View(flight);
            }
            return View();
        }
        #endregion
        #region Bookickket
        public ActionResult Bookticket(string CodeReturn, string CodeDepart)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
                var param = new SeachParam();
                try
                {
                    var userInfoBySite = GetInforByDomain(SiteMuti.Getsitename());
                    param = SeachAndBookCommon.LoadBooktrongnuoc(userInfoBySite, CodeReturn, CodeDepart, CurrentUser);
                    return View(param);
                }
                catch
                { return View(param); }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Bookticket(SeachParam param, FormCollection form)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
                if (param.BkBooking.Email == null)
                    param.BkBooking.Email = "";
                var settingsite = GetSettingByDomain(SiteMuti.Getsitename(), true);
                //  var settinguser = GetSettingByUser(CurrentUser.UserName, true);
                var daoip = new BlockIPDao();
                var daomail = new BlockEmailDao();
                if (!daoip.Checkblock(Utility.GetUserIPAddress()))
                {
                    return RedirectToAction("block");
                }
                else if (!daomail.Checkblock(param.BkBooking.Email))
                {
                    return RedirectToAction("block");
                }
                var vouchercode = form.GetValue("txtVoucherCode");
                var paramss = (SeachParam)Session[Constant.SessionSeach];
                var ExpiryDate = DateTime.Now.AddHours(24);
                var listbockok = new List<Booking>();
                log.InfoFormat("Bắt đều call bookve: {0}", "");
                var codebook = SeachAndBookCommon.bookve(param, ref ExpiryDate, ref listbockok, settingsite, CurrentUser, form);
                var codebookValue = "";
                param.BkBooking.BookCode = codebook;

                log.Debug("Kết thúc đặt vé");
                if (!string.IsNullOrEmpty(codebook))
                {
                    codebookValue = codebook;
                }
                else
                {
                    var count = listbockok.Where(z => !string.IsNullOrWhiteSpace(z.BookingCode)).Count();
                    if (count > 0 && count < 2)
                    {
                        string BookCode = "";
                        foreach (var pnr in listbockok)
                        {
                            if (!string.IsNullOrEmpty(pnr.BookingCode))
                            {
                                BookCode += pnr.BookingCode + "-";
                            }
                            else
                            {
                                BookCode += "NA" + "-";
                            }
                        }
                        if (!string.IsNullOrEmpty(BookCode) && BookCode != "FAIL")
                        {
                            BookCode = BookCode.Substring(0, BookCode.Length - 1);
                        }
                        codebookValue = BookCode;
                    }
                    else
                    {
                        codebookValue = codebook;
                    }

                }
                param.BkBooking.FailCode = codebookValue;
                param.BkBooking.HoldToDate = DateTime.Now;
                param.BkBooking.Expireddate = ExpiryDate;
                decimal tongtienhanhly = 0;
                log.Error("Bắt đầu lưu SaveBooking");
                int IDBooking = saveTicket.SaveBooking(settingsite, param.BkBooking, param, (vouchercode != null ? vouchercode.AttemptedValue.Trim() : ""), codebook, CurrentUser);
                log.Error("kết thúc lưu");
                // Chiều đi
                if (param.DepartureFlight != null)
                {
                    var input = param.DepartureFlight;
                    var ticketid = saveTicket.SaveTicket(IDBooking, input, listbockok, Constant.Typeve.LuotDi);
                    foreach (var DepartureFligh in paramss.DepartureFlight.ListFlight)
                    {
                        if (DepartureFligh.ListSegment.Count() == 1 && DepartureFligh.ListSegment.Where(z => z.Airline != DepartureFligh.Airline).Count() == 1)
                        {
                            saveTicket.SaveTicketDetail(ticketid, input);
                        }
                        else
                        {
                            foreach (var AvailFlights in DepartureFligh.ListSegment)
                            {
                                saveTicket.SaveTicketDetail(AvailFlights, ticketid);
                            }
                        }
                    }
                    var index = 0;
                    foreach (var item in param.Passengerlist)
                    {
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            item.Price = input.FareAdt + input.FeeAdt + input.TaxAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            item.Price = input.FareChd + input.FeeChd + input.TaxChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            item.Price = input.FareInf + input.FeeInf + input.TaxInf;
                        }
                        if (param.BaggageInfo != null)
                        {

                            if (!string.IsNullOrEmpty(item.BaggageDepartID))
                            {
                                var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DepartureFlight.StartPoint + param.DepartureFlight.EndPoint) && z.Code == item.BaggageDepartID);
                                if (objpass != null)
                                {
                                    item.BaggageDepartValue = objpass.Value;
                                    item.BaggageDepartPrice = Utility.ConvertToDecimal(objpass.Price.ToString());
                                    tongtienhanhly += item.BaggageDepartPrice.HasValue ? item.BaggageDepartPrice.Value : 0;
                                }
                            }
                            if (!string.IsNullOrEmpty(item.BaggageReturnID))
                            {
                                var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.ReturnFlight.StartPoint + param.ReturnFlight.EndPoint) && z.Code == item.BaggageReturnID);
                                if (objpass != null)
                                {
                                    item.BaggageReturnValue = objpass.Value;
                                    item.BaggageReturnPrice = Utility.ConvertToDecimal(objpass.Price.ToString());
                                    tongtienhanhly += item.BaggageReturnPrice.HasValue
                                        ? item.BaggageReturnPrice.Value
                                        : 0;
                                }
                            }
                        }
                        var value = form.GetValue("txtDate" + index);
                        if (value != null)
                            item.Birthday = Utility.ConvertToDate(value.AttemptedValue);
                        var idPassenger = saveTicket.SavePassenger(item, param);
                        item.ID = idPassenger;
                        var ckBookingDetail = new BK_BookingDetail
                        {
                            BookingID = IDBooking,
                            TicketID = ticketid,
                            PassengerID = idPassenger,
                            FlightNo = input.FlightNumber,
                            FromCity = input.StartPoint,
                            ToCity = input.EndPoint,
                            StartDate = input.StartDate,
                        };
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            ckBookingDetail.Price = input.FareAdt + input.FeeAdt + input.TaxAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            ckBookingDetail.Price = input.FareChd + input.FeeChd + input.TaxChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            ckBookingDetail.Price = input.FareInf + input.FeeInf + input.TaxInf;
                        }
                        saveTicket.saveDetail(ckBookingDetail);
                        index++;
                    }
                }
                // chiều về nếu có
                // Chiều về
                if (param.ReturnFlight != null)
                {
                    var input = param.ReturnFlight;
                    var ticketid = saveTicket.SaveTicket(IDBooking, input, listbockok, Constant.Typeve.LuotVe);
                    foreach (var ReturnFlight in paramss.ReturnFlight.ListFlight)
                    {
                        if (ReturnFlight.ListSegment.Count() == 1 && ReturnFlight.ListSegment.Where(z => z.Airline != ReturnFlight.Airline).Count() == 1)
                        {
                            saveTicket.SaveTicketDetail(ticketid, input);
                        }
                        else
                        {
                            foreach (var AvailFlights in ReturnFlight.ListSegment)
                            {
                                saveTicket.SaveTicketDetail(AvailFlights, ticketid);
                            }
                        }
                    }
                    var index = 0;
                    foreach (var item in param.Passengerlist)
                    {
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            item.Price += input.FareAdt + input.FeeAdt + input.TaxAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            item.Price += input.FareChd + input.FeeChd + input.TaxChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            item.Price += input.FareInf + input.FeeInf + input.TaxInf;
                        }
                        var idPassenger = item.ID;
                        var ckBookingDetail = new BK_BookingDetail
                        {
                            BookingID = IDBooking,
                            TicketID = ticketid,
                            PassengerID = idPassenger,
                            FlightNo = input.FlightNumber,
                            FromCity = input.StartPoint,
                            ToCity = input.EndPoint,
                            StartDate = input.StartDate,

                        };
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            ckBookingDetail.Price = input.FareAdt + input.FeeAdt + input.TaxAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            ckBookingDetail.Price = input.FareChd + input.FeeChd + input.TaxChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            ckBookingDetail.Price = input.FareInf + input.FeeInf + input.TaxInf;
                        }
                        saveTicket.saveDetail(ckBookingDetail);
                        index++;
                    }
                }
                var daobook = new BK_BookingDao();
                var bookupdate = new BK_Booking() { ID = IDBooking, BookCode = (codebook == "FAIL" ? "" : codebook) };
                daobook.UpdateCode(bookupdate, tongtienhanhly);
                log.Error("Update code");
                try
                {
                    var userid = Constant.KL;
                    /// write log booking
                    if (CurrentUser != null)
                        userid = CurrentUser.UserName;
                    else
                    {
                        var userdao = new AspNetUsersBiz();
                        var obj = userdao.GetInfoByEmail(param.BkBooking.Email);
                        userid = obj != null ? obj.UserName : Constant.KL;
                    }
                    LogMain.LogBooking(IDBooking, userid, "", "", Constant.LogBookingType.Bookve);
                    var paramout = (SeachParam)Session[Constant.SessionSeach];
                    paramout.Passengerlist = param.Passengerlist;
                    paramout.BaggageInfo = param.BaggageInfo;
                    paramout.BkBooking = param.BkBooking;
                    paramout.Expireddate = ExpiryDate;
                    Session[Constant.SessionSeach] = paramout;
                    Session[Constant.SessionSeachBackup] = paramout;
                    var userSendMail = GetInforByDomain(SiteMuti.Getsitename());
                    if (userSendMail != null)
                    {
                        if (string.IsNullOrEmpty(codebook.Trim()) || codebook == "FAIL" || codebook.Contains("FAIL"))
                        {
                            commonTicket.sendmailFail(IDBooking, settingsite, CurrentUser, userSendMail.Id);
                        }
                        else
                        {
                            commonTicket.sendmail(codebook, settingsite, userSendMail.Id);
                            commonTicket.sendsms(IDBooking, CurrentUser);
                        }
                    }
                    else
                    {
                        WriteLog("Bookticket Không tìm thấy templeate", Constant.LogType.Info);
                    }
                }
                catch (Exception ex)
                {
                    NLogLogger.Info(ex.ToString());
                }
                //}
                var objbooking = daobook.GetInfo(IDBooking);
                if (string.IsNullOrEmpty(codebook.Trim()) || codebook == "FAIL" || codebook.Contains("FAIL"))
                {
                    return RedirectToAction("KetQua", "ThanhToan", new { madonhang = objbooking.rCode });
                }
                else
                {
                    return RedirectToAction("ChonHinhThucThanhToan", "ThanhToan", new { madonhang = objbooking.rCode });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult BookInternational()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
                var param = new SeachParam();
                try
                {

                    param = (SeachParam)Session[Constant.SessionSeach];
                    Session["SeachRdInternational"] = param;
                    if (!string.IsNullOrEmpty(param.FareDataIdValue))
                    {
                        param = SeachAndBookCommon.LoadBookQuocTe(param, CurrentUser);
                        return View(param);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch
                {
                    return View(param);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult BookInternational(SeachParam param, FormCollection form)
        {
            if (Session["SeachRdInternational"] != null)
            {
                if (param.BkBooking.Email == null)
                    param.BkBooking.Email = "";
                var settingsite = GetSettingByDomain(SiteMuti.Getsitename(), true);
                // var settinguser = GetSettingByUser(CurrentUser.UserName, true);
                var daoip = new BlockIPDao();
                var daomail = new BlockEmailDao();
                if (!daoip.Checkblock(Utility.GetUserIPAddress()))
                {
                    return RedirectToAction("block");
                }
                else if (!daomail.Checkblock(param.BkBooking.Email))
                {
                    return RedirectToAction("block");
                }
                var vouchercode = form.GetValue("txtVoucherCode");
                var Sessioninput = (SeachParam)Session["SeachRdInternational"];
                //param.FlightSearchResult = Sessioninput.FlightSearchResult;
                var ExpiryDate = new DateTime();
                Booking prn = new Booking();
                param.FareDataInfo = Sessioninput.FareDataInfo;
                param.FareDataIdValue = Sessioninput.FareDataIdValue;
                param.DepartureFlightId = Sessioninput.DepartureFlightId;
                param.ReturnFlightId = Sessioninput.ReturnFlightId;
                var codebook = SeachAndBookCommon.bookInternational(param, ref ExpiryDate, ref prn, settingsite, CurrentUser, form);
                //if (!string.IsNullOrEmpty(codebook))
                //{
                var ticketid = 0;
                decimal tongtienhanhly = 0;
                param.BkBooking.BookCode = codebook;
                param.BkBooking.HoldToDate = DateTime.Now;
                param.BkBooking.Expireddate = ExpiryDate;
                int IDBooking = saveTicket.SaveBookingInternational(settingsite, param.BkBooking, Sessioninput,
                    (vouchercode != null ? vouchercode.AttemptedValue.Trim() : ""), codebook, CurrentUser);
                var info = Sessioninput.FareDataInfo;
                // Chiều đi
                if (Sessioninput.FareDataInfo.ListFlight.Where(z => z.StartPoint == param.DepartureAirportCode).Count() > 0)
                {

                    var input = Sessioninput.FareDataInfo.ListFlight.FirstOrDefault(
                       z => z.FlightId == Sessioninput.DepartureFlightId);
                    var inputdetail = input.ListSegment.FirstOrDefault();
                    if (input != null && inputdetail != null)
                    {
                        var ck = new BK_Ticket
                        {
                            BookingID = IDBooking,
                            FlightNo = inputdetail.FlightNumber,
                            StartDate = inputdetail.StartTime,
                            EndDate = inputdetail.EndTime,
                            Price = info.TotalPrice,
                            FromCity = input.StartPoint,
                            ToCity = input.EndPoint,
                            Class = inputdetail.Class,
                            ClassName = "",
                            Code = inputdetail.Airline,
                            IsDeleted = false,
                            PriceBefore = info.PriceBefor,
                            Tax = info.TotalPrice - info.PriceBefor,
                            TypeID = Constant.Typeve.LuotDi,
                            QuocTe = true,
                            CodeBook = codebook,
                            Exdate = ExpiryDate,
                            FareAdt = info.FareAdt,
                            FareChd = info.FareChd,
                            FareInf = info.FareInf,
                            FeeAdt = info.FeeAdt,
                            FeeChd = info.FeeChd,
                            FeeInf = info.FeeInf,
                            TaxAdt = info.TaxAdt,
                            TaxChd = info.TaxChd,
                            TaxInf = info.TaxInf,
                            Duration = input.Duration
                        };
                        if (prn != null)
                        {
                            ck.CodeBook = prn.BookingCode;
                            ck.Exdate = prn.ExpiryDate;
                            ck.Eror = prn.ErrorMessage;
                        }
                        ticketid = saveTicket.SaveTicket(ck);
                        foreach (var AvailFlights in input.ListSegment)
                        {
                            var TicketDetail = new BK_TicketDetail()
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
                            saveTicket.SaveTicketDetail(TicketDetail);
                        }
                    }
                    var index = 0;
                    foreach (var item in param.Passengerlist)
                    {
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            item.Price = info.FareAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            item.Price = info.FareChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            item.Price = info.FareInf;
                        }
                        if (param.BaggageInfo != null)
                        {
                            if (!string.IsNullOrEmpty(item.BaggageDepartID))
                            {
                                var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DepartureAirportCode + param.DestinationAirportCode) && z.Code == item.BaggageDepartID);
                                if (objpass != null)
                                {
                                    item.BaggageDepartValue = objpass.Value;
                                    item.BaggageDepartPrice = Utility.ConvertToDecimal(objpass.Price.ToString());
                                    tongtienhanhly += item.BaggageDepartPrice.HasValue ? item.BaggageDepartPrice.Value : 0;
                                }
                            }
                            if (!string.IsNullOrEmpty(item.BaggageReturnID))
                            {
                                var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DestinationAirportCode + param.DepartureAirportCode) && z.Code == item.BaggageReturnID);
                                if (objpass != null)
                                {
                                    item.BaggageReturnValue = objpass.Value;
                                    item.BaggageReturnPrice = Utility.ConvertToDecimal(objpass.Price.ToString());
                                    tongtienhanhly += item.BaggageReturnPrice.HasValue
                                        ? item.BaggageReturnPrice.Value
                                        : 0;
                                }
                            }
                        }
                        var value = form.GetValue("txtDate" + index);
                        if (value != null)
                            item.Birthday = Utility.ConvertToDate(value.AttemptedValue);
                        var idPassenger = saveTicket.SavePassengerInternational(item, Sessioninput);
                        item.ID = idPassenger;
                        var ckBookingDetail = new BK_BookingDetail
                        {
                            BookingID = IDBooking,
                            TicketID = ticketid,
                            PassengerID = idPassenger,
                            FlightNo = inputdetail.FlightNumber,
                            FromCity = input.StartPoint,
                            ToCity = input.EndPoint,
                            StartDate = input.StartDate,
                        };
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            ckBookingDetail.Price = info.FareAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            ckBookingDetail.Price = info.FareChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            ckBookingDetail.Price = info.FareInf;
                        }
                        saveTicket.saveDetail(ckBookingDetail);
                        index++;
                    }
                }
                // chiều về nếu có
                if (Sessioninput.FareDataInfo.ListFlight.Where(z => z.StartPoint == param.DestinationAirportCode).Count() > 0)
                {

                    var input = Sessioninput.FareDataInfo.ListFlight.FirstOrDefault(
                    z => z.FlightId == Sessioninput.ReturnFlightId);
                    var inputdetail = input.ListSegment.FirstOrDefault();
                    if (input != null && inputdetail != null)
                    {
                        var ck = new BK_Ticket
                        {
                            BookingID = IDBooking,
                            FlightNo = inputdetail.FlightNumber,
                            StartDate = inputdetail.StartTime,
                            EndDate = inputdetail.EndTime,
                            Price = info.TotalPrice,
                            FromCity = input.StartPoint,
                            ToCity = input.EndPoint,
                            Class = inputdetail.Class,
                            ClassName = "",
                            Code = inputdetail.Airline,
                            IsDeleted = false,
                            PriceBefore = info.PriceBefor,
                            Tax = info.TotalPrice - info.PriceBefor,
                            TypeID = Constant.Typeve.LuotVe,
                            QuocTe = true,
                            CodeBook = codebook,
                            Exdate = ExpiryDate,
                            FareAdt = info.FareAdt,
                            FareChd = info.FareChd,
                            FareInf = info.FareInf,
                            FeeAdt = info.FeeAdt,
                            FeeChd = info.FeeChd,
                            FeeInf = info.FeeInf,
                            TaxAdt = info.TaxAdt,
                            TaxChd = info.TaxChd,
                            TaxInf = info.TaxInf,
                            Duration = input.Duration
                        };
                        if (prn != null)
                        {
                            ck.CodeBook = prn.BookingCode;
                            ck.Exdate = prn.ExpiryDate;
                            ck.Eror = prn.ErrorMessage;
                        }
                        ticketid = saveTicket.SaveTicket(ck);
                        foreach (var AvailFlights in input.ListSegment)
                        {
                            var TicketDetail = new BK_TicketDetail()
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
                            saveTicket.SaveTicketDetail(TicketDetail);
                        }
                    }
                    var index = 0;
                    foreach (var item in param.Passengerlist)
                    {
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            item.Price = info.FareAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            item.Price = info.FareChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            item.Price = info.FareInf;
                        }
                        var idPassenger = item.ID;
                        var ckBookingDetail = new BK_BookingDetail
                        {
                            BookingID = IDBooking,
                            TicketID = ticketid,
                            PassengerID = idPassenger,
                            FlightNo = inputdetail.FlightNumber,
                            FromCity = input.StartPoint,
                            ToCity = input.EndPoint,
                            StartDate = input.StartDate
                        };
                        if (item.TypeID == Constant.TypePassenger.NguoiLon)
                        {
                            ckBookingDetail.Price = info.FareAdt;
                        }
                        else if (item.TypeID == Constant.TypePassenger.TreEm)
                        {
                            ckBookingDetail.Price = info.FareChd;
                        }
                        else if (item.TypeID == Constant.TypePassenger.EmBe)
                        {
                            ckBookingDetail.Price = info.FareInf;
                        }
                        saveTicket.saveDetail(ckBookingDetail);
                        index++;
                    }

                }
                var daobook = new BK_BookingDao();
                var bookupdate = new BK_Booking() { ID = IDBooking, BookCode = codebook };
                daobook.UpdateCode(bookupdate, tongtienhanhly);
                try
                {
                    /// write log booking
                    string userid = Constant.KL;
                    if (CurrentUser != null)
                    {
                        userid = CurrentUser.UserName;
                    }
                    else
                    {
                        var userdao = new AspNetUsersBiz();
                        var obj = userdao.GetInfoByEmail(param.BkBooking.Email);
                        userid = obj != null ? obj.UserName : Constant.KL;
                    }
                    LogMain.LogBooking(IDBooking, userid, "", "", Constant.LogBookingType.Bookve);
                    var paramout = (SeachParam)Session["SeachRdInternational"];
                    paramout.Passengerlist = param.Passengerlist;
                    paramout.BaggageInfo = param.BaggageInfo;
                    paramout.BkBooking = param.BkBooking;
                    paramout.Expireddate = ExpiryDate;
                    Session[Constant.SessionSeach] = paramout;
                    Session[Constant.SessionSeachBackup] = paramout;

                    var userSendMail = GetInforByDomain(SiteMuti.Getsitename());
                    if (userSendMail != null)
                    {
                        if (string.IsNullOrEmpty(codebook.Trim()))
                        {
                            commonTicket.sendmailFail(IDBooking, settingsite, CurrentUser, userSendMail.Id);
                        }
                        else
                        {
                            commonTicket.sendmail(codebook, settingsite, userSendMail.Id);
                            commonTicket.sendsmsQT(IDBooking);
                        }
                    }
                    else
                    {
                        WriteLog("BookInternational Không tìm thấy templeate", Constant.LogType.Info);
                    }

                    //return RedirectToAction("ResultInternational", new { @madonhang = codebook });

                }
                catch (Exception ex)
                {
                    NLogLogger.Info(ex.ToString());
                }
                //}
                //return RedirectToAction("ResultInternational", new { @madonhang = "" });
                var objbooking = daobook.GetInfo(IDBooking);
                if (string.IsNullOrEmpty(codebook.Trim()) || codebook == "FAIL" || codebook.Contains("FAIL"))
                {
                    return RedirectToAction("KetQua", "ThanhToan", new { madonhang = objbooking.rCode });
                }
                else
                {
                    return RedirectToAction("ChonHinhThucThanhToan", "ThanhToan", new { madonhang = objbooking.rCode });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        public ActionResult GetDepartureFlight(string index)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            return View(SeachRd.DepartureFlights);
        }
        public ActionResult GetDetailDepartureFlight(string index)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            var obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataIdValue == index);
            return View(obj);
        }
        public ActionResult GetDetailReturnFlight(string index)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            var obj = SeachRd.ReturnFlights.FirstOrDefault(z => z.FareDataIdValue == index);
            return View(obj);
        }
        #region voucher
        public ActionResult CheckVoucher(string searchParam, string trongnuoc)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            double tonggia = 0;
            var param = (SeachParam)Session[Constant.SessionSeach];
            if (trongnuoc == "1")
            {

                if (param.DepartureFlight != null)
                {
                    tonggia += param.DepartureFlight.TotalPrice;
                }
                if (param.ReturnFlight != null)
                {
                    tonggia += param.ReturnFlight.TotalPrice;
                }
            }
            else
            {
                tonggia = param.FareDataInfo.TotalPrice;
            }
            var dao = new VoucherCodeDao();
            var list = dao.Checkbycode(searchParam, CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList());
            if (list != null)
            {
                var tiengiamgia = list.Valued.HasValue ? list.Valued.Value : 0;
                return
                    Json(
                        new
                        {
                            TongTien = tonggia.ToString("n0"),
                            km = tiengiamgia.ToString("n0"),
                            giamoi = (tonggia - (double)tiengiamgia).ToString("n0")
                        }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { TongTien = "0" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Tìm vé rẻ
        public ActionResult TimVere(int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
          string StartMonth, string EndMonth,
          int? Adult, int? Children, int? Infant)
        {
            var listsetting = GetSettingByDomain(SiteMuti.Getsitename(), false);
            var param = SeachAndBookCommon.SeachVeThang(listsetting, Itinerary.HasValue ? Itinerary.Value : 0,
                DepartureAirportCode,
                 DestinationAirportCode,
                 StartMonth,
                  EndMonth,
                   Adult.HasValue ? Adult.Value : 0,
                   Children.HasValue ? Children.Value : 0,
                 Infant.HasValue ? Infant.Value : 0
                                );
            Session[Constant.SessionSeach] = param;
            Session[Constant.SessionSeachBackup] = param;
            return View(param);
        }
        [HttpPost]
        public ActionResult TimVere(SeachParam param, FormCollection forminput)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var hdfSelectedDepartFlt = forminput.GetValue("hdfSelectedDepartFlt");
            string SelectedDepartFlt = hdfSelectedDepartFlt != null ? hdfSelectedDepartFlt.AttemptedValue : string.Empty;
            var hdfSelectedReturnFlt = forminput.GetValue("hdfSelectedReturnFlt");
            string SelectedReturnFlt = hdfSelectedReturnFlt != null ? hdfSelectedReturnFlt.AttemptedValue : string.Empty;

            if (!string.IsNullOrEmpty(SelectedDepartFlt) || !string.IsNullOrEmpty(SelectedReturnFlt))
            {
                if (Session[Constant.SessionSeach] != null)
                {
                    var idCodeReturn = Utility.ConvertToInt(SelectedReturnFlt);
                    var idDepartFlt = Utility.ConvertToInt(SelectedDepartFlt);
                    var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                    if (!string.IsNullOrEmpty(SelectedReturnFlt))
                    {
                        var objDeparture = SeachRd.DepartureFlightsVeThang[idDepartFlt.HasValue ? idDepartFlt.Value : 0];
                        var objReturn = SeachRd.ReturnFlightsVeThang[idCodeReturn.HasValue ? idCodeReturn.Value : 0];
                        return RedirectToAction("SeachMain", "SeachHome", new
                        {
                            Itinerary = param.Itinerary,
                            DepartureAirportCode = param.DepartureAirportCode,
                            DestinationAirportCode = param.DestinationAirportCode,
                            DepartureDate = objDeparture.StartDate.ToString("dd/MM/yyyy"),
                            ReturnDate = objReturn.StartDate.ToString("dd/MM/yyyy"),
                            Adult = param.Adult,
                            Children = param.Children,
                            Infant = param.Infant,
                            AirlineCode = string.Empty,
                            Currency = "",
                            older = string.Empty,
                            typeolder = string.Empty,
                            StartMonth = "",
                            EndMonth = ""
                        });
                    }
                    else
                    {
                        var objDeparture = SeachRd.DepartureFlightsVeThang[idDepartFlt.HasValue ? idDepartFlt.Value : 0];
                        return RedirectToAction("SeachMain", "SeachHome", new
                        {
                            Itinerary = param.Itinerary,
                            DepartureAirportCode = param.DepartureAirportCode,
                            DestinationAirportCode = param.DestinationAirportCode,
                            DepartureDate = objDeparture.StartDate.ToString("dd/MM/yyyy"),
                            ReturnDate = "",
                            Adult = param.Adult,
                            Children = param.Children,
                            Infant = param.Infant,
                            AirlineCode = string.Empty,
                            Currency = "",
                            older = string.Empty,
                            typeolder = string.Empty,
                            StartMonth = "",
                            EndMonth = ""
                        });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(param);
        }
        #endregion

        #region hiển thị vé
        public async Task<ActionResult> GetDepartureinfoSeason2()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                // obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataId == id);
                ViewBag.Itinerary = SeachRd.Itinerary;
                var listvalue = new List<VDMFareDataInfo>();
                if (SeachRd.DepartureFlights == null)
                    SeachRd.DepartureFlights = new List<VDMFareDataInfo>();
                if (SeachRd.ListAirline != null)
                {
                    foreach (var item in SeachRd.ListAirline)
                    {
                        var objFlight = SeachRd.DepartureFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (objFlight != null)
                            listvalue.Add(objFlight);
                    }
                }
                ViewBag.CountAirline = listvalue.Count();
                listvalue.AddRange(SeachRd.DepartureFlights.Where(z => listvalue.Select(x => x.FareDataIdValue).Contains(z.FareDataIdValue) == false));
                if (listvalue.Count >= 25)
                    listvalue = listvalue.Skip(25).ToList();
                else listvalue = new List<VDMFareDataInfo>();
                return View(listvalue);
            }
            return View();
        }
        public async Task<ActionResult> GetDepartureinfoSeason1()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                // obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataId == id);
                ViewBag.Itinerary = SeachRd.Itinerary;
                var listvalue = new List<VDMFareDataInfo>();
                if (SeachRd.DepartureFlights == null)
                    SeachRd.DepartureFlights = new List<VDMFareDataInfo>();
                if (SeachRd.ListAirline != null)
                {
                    foreach (var item in SeachRd.ListAirline)
                    {
                        var objFlight = SeachRd.DepartureFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (objFlight != null)
                            listvalue.Add(objFlight);
                    }
                }
                ViewBag.CountAirline = listvalue.Count();
                listvalue.AddRange(SeachRd.DepartureFlights.Where(z => listvalue.Select(x => x.FareDataIdValue).Contains(z.FareDataIdValue) == false));
                if (listvalue.Count >= 5)
                    listvalue = listvalue.Skip(5).Take(20).ToList();
                else listvalue = new List<VDMFareDataInfo>();
                return View(listvalue);
            }
            return View();
        }
        public async Task<ActionResult> GetDepartureinfo()
        {
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if(Session[Constant.SessionSeach]==null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                // obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataId == id);
                ViewBag.Itinerary = SeachRd.Itinerary;
                var listvalue = new List<VDMFareDataInfo>();
                if (SeachRd.DepartureFlights == null)
                    SeachRd.DepartureFlights = new List<VDMFareDataInfo>();
                if (SeachRd.ListAirline != null)
                {
                    foreach (var item in SeachRd.ListAirline)
                    {
                        var objFlight = SeachRd.DepartureFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (objFlight != null)
                            listvalue.Add(objFlight);
                    }
                }
                ViewBag.CountAirline = listvalue.Count();
                listvalue = listvalue.OrderBy(z => z.TotalPrice).ToList();
                listvalue.AddRange(SeachRd.DepartureFlights.Where(z => listvalue.Select(x => x.FareDataIdValue).Contains(z.FareDataIdValue) == false));
                return View(listvalue.Take(5).ToList());
            }
            return View();
        }
        public async Task<ActionResult> GetReturnFlightinfo()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                // obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataId == id);
                var listvalue = new List<VDMFareDataInfo>();
                if (SeachRd.ReturnFlights == null)
                    SeachRd.ReturnFlights = new List<VDMFareDataInfo>();
                if (SeachRd.ListAirline != null)
                {
                    foreach (var item in SeachRd.ListAirline)
                    {
                        var objFlight = SeachRd.ReturnFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (objFlight != null)
                            listvalue.Add(objFlight);
                    }
                }
                ViewBag.CountAirline = listvalue.Count();
                listvalue = listvalue.OrderBy(z => z.TotalPrice).ToList();
                listvalue.AddRange(SeachRd.ReturnFlights.Where(z => listvalue.Select(x => x.FareDataIdValue).Contains(z.FareDataIdValue) == false));
                return View(listvalue.Take(5).ToList());
            }
            return View();
        }
        public async Task<ActionResult> GetReturnFlightinfoSeason1()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                // obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataId == id);
                var listvalue = new List<VDMFareDataInfo>();
                if (SeachRd.ReturnFlights == null)
                    SeachRd.ReturnFlights = new List<VDMFareDataInfo>();
                if (SeachRd.ListAirline != null)
                {
                    foreach (var item in SeachRd.ListAirline)
                    {
                        var objFlight = SeachRd.ReturnFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (objFlight != null)
                            listvalue.Add(objFlight);
                    }
                }
                ViewBag.CountAirline = listvalue.Count();
                listvalue.AddRange(SeachRd.ReturnFlights.Where(z => listvalue.Select(x => x.FareDataIdValue).Contains(z.FareDataIdValue) == false));
                if (listvalue.Count >= 5)
                    listvalue = listvalue.Skip(5).Take(20).ToList();
                else listvalue = new List<VDMFareDataInfo>();
                return View(listvalue.ToList());
            }
            return View();
        }
        public async Task<ActionResult> GetReturnFlightinfoSeason2()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                // obj = SeachRd.DepartureFlights.FirstOrDefault(z => z.FareDataId == id);
                var listvalue = new List<VDMFareDataInfo>();
                if (SeachRd.ReturnFlights == null)
                    SeachRd.ReturnFlights = new List<VDMFareDataInfo>();
                if (SeachRd.ListAirline != null)
                {
                    foreach (var item in SeachRd.ListAirline)
                    {
                        var objFlight = SeachRd.ReturnFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (objFlight != null)
                            listvalue.Add(objFlight);
                    }
                }
                ViewBag.CountAirline = listvalue.Count();
                listvalue.AddRange(SeachRd.ReturnFlights.Where(z => listvalue.Select(x => x.FareDataIdValue).Contains(z.FareDataIdValue) == false));
                if (listvalue.Count >= 25)
                    listvalue = listvalue.Skip(25).ToList();
                else listvalue = new List<VDMFareDataInfo>();
                return View(listvalue);
            }
            return View();
        }
        public async Task<ActionResult> GetFareDataInfo()
        {
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
               
               var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                if (SeachRd.FareDataList == null)
                    SeachRd.FareDataList = new List<VDMFareDataInfo>();
                SeachRd.LstFareData = SeachRd.FareDataList.Take(15).ToList();
                var lstStation = "";
                foreach (var item in SeachRd.FareDataList)
                {
                    foreach (var items in item.ListFlight)
                    {
                        if (!lstStation.Contains(items.StartPoint))
                            lstStation += (items.StartPoint + ".");
                        if (!lstStation.Contains(items.EndPoint))
                            lstStation += (items.EndPoint + ".");
                        foreach (var itemss in items.ListSegment)
                        {
                            if (!lstStation.Contains(itemss.StartPoint))
                                lstStation += (itemss.StartPoint + ".");
                            if (!lstStation.Contains(itemss.EndPoint))
                                lstStation += (itemss.EndPoint + ".");
                        }
                    }
                }
                ViewBag.listStation = Station.GetAirportByListCode(lstStation);
                return View(SeachRd);
            }
            return View();
        }
        public async Task<ActionResult> GetFareDataInfo1()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            VDMFareDataInfo obj = new VDMFareDataInfo();
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                if (SeachRd.FareDataList == null)
                    SeachRd.FareDataList = new List<VDMFareDataInfo>();
                SeachRd.LstFareData = SeachRd.FareDataList.Skip(15).ToList();
                var lstStation = "";
                foreach (var item in SeachRd.FareDataList)
                {
                    foreach (var items in item.ListFlight)
                    {
                        if (!lstStation.Contains(items.StartPoint))
                            lstStation += (items.StartPoint + ".");
                        if (!lstStation.Contains(items.EndPoint))
                            lstStation += (items.EndPoint + ".");
                        foreach (var itemss in items.ListSegment)
                        {
                            if (!lstStation.Contains(itemss.StartPoint))
                                lstStation += (itemss.StartPoint + ".");
                            if (!lstStation.Contains(itemss.EndPoint))
                                lstStation += (itemss.EndPoint + ".");
                        }
                    }
                }
                ViewBag.listStation = Station.GetAirportByListCode(lstStation);
                return View(SeachRd);
            }
            return View();
        }
        public async Task<ActionResult> GetDetailDepartureFlightInternational(string index, int? leg)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            var objFareData = SeachRd.LstFareData.FirstOrDefault(z => z.FareDataIdValue == index);
            var objfl = objFareData.ListFlight.FirstOrDefault(z => z.Leg == leg);
            return View(SeachRd.DepartureFlight);
        }
        public async Task<ActionResult> GetDetailReturnFlightInternational(string index)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            var SeachRd = (SeachParam)Session[Constant.SessionSeach];
            SeachRd.ReturnFlight = SeachRd.ReturnFlights.FirstOrDefault(z => z.FareDataIdValue == index);
            return View(SeachRd.ReturnFlight);
        }
        public async Task<ActionResult> Sort(string older, string typeolder)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
                var param = (SeachParam)Session[Constant.SessionSeach];
                switch (older)
                {
                    case Constant.Olderve.Gia:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.DepartureFlights = param.DepartureFlights.OrderBy(z => z.TotalPrice).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderBy(z => z.TotalPrice).ToList();
                            }
                            else
                            {
                                param.DepartureFlights =
                                    param.DepartureFlights.OrderByDescending(z => z.TotalPrice).ToList();
                                param.ReturnFlights =
                                    param.ReturnFlights.OrderByDescending(z => z.TotalPrice).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.Thoigianbay:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.DepartureFlights = param.DepartureFlights.OrderBy(z => z.Duration).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderBy(z => z.Duration).ToList();
                            }
                            else
                            {
                                param.DepartureFlights =
                                    param.DepartureFlights.OrderByDescending(z => z.Duration).ToList();
                                param.ReturnFlights =
                                    param.ReturnFlights.OrderByDescending(z => z.Duration).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.HangHangkhong:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.DepartureFlights = param.DepartureFlights.OrderBy(z => z.Airline).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderBy(z => z.Airline).ToList();
                            }
                            else
                            {
                                param.DepartureFlights = param.DepartureFlights.OrderByDescending(z => z.Airline).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderByDescending(z => z.Airline).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.GioCatCanh:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.DepartureFlights = param.DepartureFlights.OrderBy(z => z.StartDate).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderBy(z => z.StartDate).ToList();
                            }
                            else
                            {
                                param.DepartureFlights =
                                    param.DepartureFlights.OrderByDescending(z => z.StartDate).ToList();
                                param.ReturnFlights =
                                    param.ReturnFlights.OrderByDescending(z => z.StartDate).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.GioHaCanh:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.DepartureFlights = param.DepartureFlights.OrderBy(z => z.EndDate).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderBy(z => z.EndDate).ToList();
                            }
                            else
                            {
                                param.DepartureFlights.OrderByDescending(z => z.EndDate).ToList();
                                param.ReturnFlights = param.ReturnFlights.OrderByDescending(z => z.EndDate).ToList();
                            }
                            break;
                        }
                    default:
                        {
                            param.DepartureFlights = param.DepartureFlights.OrderBy(z => z.TotalPrice).ToList();
                            param.ReturnFlights = param.ReturnFlights.OrderBy(z => z.TotalPrice).ToList();
                            break;
                        }
                }
                Session[Constant.SessionSeach] = param;
                Session[Constant.SessionSeachBackup] = param;
                return Json(new { isSuccess = false, mess = "Thành công." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { isSuccess = false, mess = "Phiên làm việc đã kết thúc." }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> SortInternational(string older, string typeolder)
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            if (Session[Constant.SessionSeach] != null)
            {
                var param = (SeachParam)Session[Constant.SessionSeach];

                if (param.LstFareData.Count <= 0)
                    param.LstFareData = param.FareDataList;
                if (param.LstFareData.Count > 0)
                {
                    param.countDepartureFlights = param.LstFareData.Count();
                    switch (older)
                    {
                        case Constant.Olderve.Gia:
                            {
                                if (typeolder == Constant.Typeolder.Ascending)
                                {
                                    param.LstFareData = param.LstFareData.OrderBy(z => z.TotalPrice).ToList();

                                }
                                else
                                {
                                    param.LstFareData = param.LstFareData.OrderByDescending(z => z.TotalPrice).ToList();
                                }
                                break;
                            }
                        case Constant.Olderve.Thoigianbay:
                            {
                                if (typeolder == Constant.Typeolder.Ascending)
                                {
                                    param.LstFareData = param.LstFareData.OrderBy(z => z.Duration).ToList();

                                }
                                else
                                {
                                    param.LstFareData = param.LstFareData.OrderByDescending(z => z.Duration).ToList();
                                }
                                break;
                            }
                        case Constant.Olderve.HangHangkhong:
                            {
                                if (typeolder == Constant.Typeolder.Ascending)
                                {
                                    param.LstFareData = param.LstFareData.OrderBy(z => z.Airline).ToList();
                                }
                                else
                                {
                                    param.LstFareData = param.LstFareData.OrderByDescending(z => z.Airline).ToList();
                                }
                                break;
                            }
                        case Constant.Olderve.GioCatCanh:
                            {
                                if (typeolder == Constant.Typeolder.Ascending)
                                {
                                    param.LstFareData = param.LstFareData.OrderBy(z => z.StartDate).ToList();
                                }
                                else
                                {
                                    param.LstFareData = param.LstFareData.OrderByDescending(z => z.StartDate).ToList();
                                }
                                break;
                            }
                        case Constant.Olderve.GioHaCanh:
                            {
                                if (typeolder == Constant.Typeolder.Ascending)
                                {
                                    param.LstFareData = param.LstFareData.OrderBy(z => z.EndDate).ToList();
                                }
                                else
                                {
                                    param.LstFareData = param.LstFareData.OrderByDescending(z => z.EndDate).ToList();
                                }
                                break;
                            }
                        default:
                            {
                                param.LstFareData = param.LstFareData.OrderBy(z => z.TotalPrice).ToList();
                                break;
                            }
                    }

                }
                param.FareDataList = param.LstFareData;
                Session[Constant.SessionSeach] = param;
                Session[Constant.SessionSeachBackup] = param;
                return Json(new { isSuccess = false, mess = "Thành công." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { isSuccess = false, mess = "Phiên làm việc đã kết thúc." }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Copy()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            string value = "";
            if (Session[Constant.SessionSeach] != null)
            {

                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                var DiemDi = getsanbaycode(SeachRd.DepartureAirportCode);
                var DiemDen = getsanbaycode(SeachRd.DestinationAirportCode);
                value += DiemDi + "(" + SeachRd.DepartureAirportCode + ")" + " - " + DiemDen + "(" + SeachRd.DestinationAirportCode + ")" + " " + SeachRd.DepartureDate + "<br>";
                foreach (var item in SeachRd.DepartureFlights)
                {
                    var PriceAdult = UIUty.ChuyenDoiTienTe(item.FareAdt);
                    var FeeAdult = UIUty.ChuyenDoiTienTe(item.FeeAdt);
                    var TaxAdult = UIUty.ChuyenDoiTienTe(item.TaxAdt);
                    var gia = (PriceAdult + FeeAdult + TaxAdult).ToString("n0");
                    var gialent = gia.Length;
                    value += item.FlightNumber + " " + item.StartDate.ToString("HH:mm") + " " + (gialent > 4 ? gia.Remove(gialent - 4, 4) + "K" : gia) + "<br>";
                }
                if (SeachRd.Itinerary > 1)
                {
                    value += DiemDen + "(" + SeachRd.DestinationAirportCode + ")" + " - " + DiemDi + "(" + SeachRd.DepartureAirportCode + ")" + " " + SeachRd.ReturnDate + "<br>";
                    foreach (var item in SeachRd.ReturnFlights)
                    {
                        var PriceAdult = UIUty.ChuyenDoiTienTe(item.FareAdt);
                        var FeeAdult = UIUty.ChuyenDoiTienTe(item.FeeAdt);
                        var TaxAdult = UIUty.ChuyenDoiTienTe(item.TaxAdt);
                        var gia = (PriceAdult + FeeAdult + TaxAdult).ToString("n0");
                        var gialent = gia.Length;
                        value += item.FlightNumber + " " + item.StartDate.ToString("HH:mm") + " " + (gialent > 4 ? gia.Remove(gialent - 4, 4) + "K" : gia) + "<br>";
                    }
                }
            }
            ViewBag.contend = value;
            return View();
        }
        public async Task<ActionResult> CopyInternational()
        {
            if (Session[Constant.SessionSeach] == null)
            {
                Session[Constant.SessionSeach] = Session[Constant.SessionSeachBackup];
            }
            string value = "";
            if (Session[Constant.SessionSeach] != null)
            {
                var SeachRd = (SeachParam)Session[Constant.SessionSeach];
                var list = SeachRd.FareDataList.ToList();
                foreach (var item in list)
                {
                    var FeeAdult = UIUty.ChuyenDoiTienTe(item.FeeAdt);
                    var FeeChild = UIUty.ChuyenDoiTienTe(item.FeeChd);
                    var FeeInfant = UIUty.ChuyenDoiTienTe(item.FeeInf);
                    var FareAdult = UIUty.ChuyenDoiTienTe(item.FareAdt);
                    var FareChild = UIUty.ChuyenDoiTienTe(item.FareChd);
                    var FareInfant = UIUty.ChuyenDoiTienTe(item.FareInf);
                    var Pricebefor = UIUty.ChuyenDoiTienTe(item.TotalPrice);
                    var TotalFare = UIUty.ChuyenDoiTienTe(item.TotalPrice);
                    var TaxAdult = UIUty.ChuyenDoiTienTe(item.TaxAdt);
                    var TaxChild = UIUty.ChuyenDoiTienTe(item.TaxChd);
                    var TaxInfant = UIUty.ChuyenDoiTienTe(item.TaxInf);
                    var CurrencyCode = UIUty.GetDonViTienTe();
                    var ListDepartFlight = item.ListFlight.Where(z => z.StartPoint == SeachRd.DepartureAirportCode);
                    var ListReturnFlight = item.ListFlight.Where(z => z.StartPoint == SeachRd.DestinationAirportCode);
                    foreach (var itemve in ListDepartFlight)
                    {
                        if (ListReturnFlight.Count() == 0)
                        {
                            foreach (var items in ListDepartFlight.Where(z => z.FlightId == itemve.FlightId))
                            {
                                foreach (var itemAvail in items.ListSegment)
                                {
                                    value += itemAvail.FlightNumber + " " +
                                        itemAvail.StartTime.Day + Utility.MonthName(itemAvail.StartTime.Month) +
                                        " " + itemAvail.StartPoint + " " + itemAvail.EndPoint +
                                        " " + itemAvail.StartTime.ToString("HH:mm").Replace(":", "") +
                                        " " + itemAvail.EndTime.ToString("HH:mm").Replace(":", "") + "<br>";
                                }
                                var gia = FareAdult.ToString("#,0.#");
                                var gialent = gia.Length;
                                value += "Giá: " + (gialent > 4 ? gia.Remove(gialent - 4, 4) + "K" : gia) + "<br>";
                            }
                        }
                        else
                        {
                            foreach (var itemveve in ListReturnFlight)
                            {
                                foreach (var items in ListDepartFlight.Where(z => z.FlightId == itemve.FlightId))
                                {
                                    foreach (var itemAvail in items.ListSegment)
                                    {
                                        value += itemAvail.FlightNumber + " " +
                                            itemAvail.StartTime.Day + Utility.MonthName(itemAvail.StartTime.Month) +
                                            " " + itemAvail.StartPoint + " " + itemAvail.EndPoint +
                                            " " + itemAvail.StartTime.ToString("HH:mm").Replace(":", "") +
                                            " " + itemAvail.EndTime.ToString("HH:mm").Replace(":", "") + "<br>";
                                    }

                                }
                                foreach (var items in ListReturnFlight.Where(z => z.FlightId == itemveve.FlightId))
                                {
                                    foreach (var itemAvail in items.ListSegment)
                                    {
                                        value += itemAvail.FlightNumber + " " +
                                            itemAvail.StartTime.Day + Utility.MonthName(itemAvail.StartTime.Month) +
                                            " " + itemAvail.StartPoint + " " + itemAvail.EndPoint +
                                            " " + itemAvail.StartTime.ToString("HH:mm").Replace(":", "") +
                                            " " + itemAvail.EndTime.ToString("HH:mm").Replace(":", "") + "<br>";
                                    }

                                }
                                var gia = FareAdult.ToString("#,0.#");
                                var gialent = gia.Length;
                                value += "Giá: " + (gialent > 4 ? gia.Remove(gialent - 4, 4) + "K" : gia) + "<br>";
                            }

                        }
                    }
                }
            }
            ViewBag.contend = value;
            return View();
        }
        private string getsanbaycode(string code)
        {
            var obj = Station.Getbycode(code);
            if (obj != null)
                return obj.AirportName;
            return "";
        }
        #endregion
        public ActionResult block()
        {
            return View();
        }
    }
}