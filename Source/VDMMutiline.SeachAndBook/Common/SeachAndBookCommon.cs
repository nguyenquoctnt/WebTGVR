using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VDMMutiline.Biz;
using VDMMutiline.Core;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.Models;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SendMailAndSMS.TeampleateVe;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;
using ApiClient.Models;
using System.Threading.Tasks;
using VDMMutiline.SeachAndBook.Common;
using BookingResponse.Database;

namespace VDMMutiline.SeachAndBook
{
    public class SeachAndBookCommon : BaseSeach
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        CommonTicket common = new CommonTicket();
        SaveBookResponse saveBookResponse = new SaveBookResponse();
        public async Task<SeachParam> seachTrongNuocAsync(List<SettingUserInfo> listsetting, int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
         string DepartureDate, string ReturnDate,
         int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder)
        {
            var jsonSerialiser = new JavaScriptSerializer();
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
                //FlightSearchResult = new SearchResponse(),
                TrongNuoc = true,
            };
            var ListAirlineCode = new List<string>();
            if (!string.IsNullOrEmpty(AirlineCode))
            {
                var spi = AirlineCode.Split(';');
                foreach (var s in spi)
                {
                    if (!string.IsNullOrEmpty(s))
                        ListAirlineCode.Add(s.Trim().ToUpper());
                }
            }
            if (Itinerary == 1)
                ReturnDate = "";
            #region Trong nước
            var startdate = Utility.ConvertToDate(DepartureDate);
            var enddate = Utility.ConvertToDate(ReturnDate);
            SearchRequest request = new SearchRequest
            {
                HeaderUser = HeaderUser,
                HeaderPass = HeaderPassword,
                AgentAccount = Username,
                AgentPassword = Password,
                LanguageCode = "vi",
                Adt = Adult ?? 0,
                Chd = Children ?? 0,
                Inf = Infant ?? 0,
                ViewMode = "default",
            };

            var lstFlightConvert = new List<VDMFareDataInfo>();
            var data = new SearchResponse();
            data.ListFareData = new List<FareData>().ToArray();
            var VNData = SeachByAirline(request, Itinerary, DepartureAirportCode, DestinationAirportCode, startdate, enddate, "VN");
            var VJDATA = SeachByAirline(request, Itinerary, DepartureAirportCode, DestinationAirportCode, startdate, enddate, "VJ");
            var QHDATA = SeachByAirline(request, Itinerary, DepartureAirportCode, DestinationAirportCode, startdate, enddate, "QH");
            var VUDATA = SeachByAirline(request, Itinerary, DepartureAirportCode, DestinationAirportCode, startdate, enddate, "VU");
            //var JQDATA = SeachByAirlineJQ(Itinerary, DepartureAirportCode, DestinationAirportCode, startdate, enddate, request.Adt, request.Chd, request.Inf);
            //await Task.WhenAll(VNData, JQDATA, VJDATA, QHDATA, VUDATA);
            await Task.WhenAll(VNData, VJDATA, QHDATA, VUDATA);
            //await Task.WhenAll(VNData, VJDATA, QHDATA);


            if (VNData != null)
            {
                if (VNData.Result != null)
                {
                    var listhanche = new List<MonthSearch.HanchesohieuInfo>();
                    using (var service = new MonthSearch.MonthSearchSoapClient())
                    {
                        var srlisthanche = await service.GetlisthancheAsync("VN");
                        if (srlisthanche != null)
                            if (srlisthanche.Body != null)
                                if (srlisthanche.Body.GetlisthancheResult != null)
                                    listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
                    }
                    data.Itinerary = VNData.Result.Itinerary;
                    data.Message = VNData.Result.Message;
                    data.Status = VNData.Result.Status;
                    data.ErrorCode = VNData.Result.ErrorCode;
                    data.FlightType = VNData.Result.FlightType;
                    var listCheckBaggageVn = apiClient.GetBaggageVn(DepartureAirportCode, DestinationAirportCode);
                    var listCheckBaggageVnJq = apiClient.GetBaggageVnJq(DepartureAirportCode, DestinationAirportCode);
                    if (Itinerary > 1)
                    {
                        listCheckBaggageVn.AddRange(apiClient.GetBaggageVn(DestinationAirportCode, DepartureAirportCode));
                        listCheckBaggageVnJq.AddRange(apiClient.GetBaggageVnJq(DestinationAirportCode, DepartureAirportCode));
                    }
                    if (VNData.Result.ListFareData == null)
                        VNData.Result.ListFareData = new List<FareData>().ToArray();
                    var lstvalue = await common.AddLccFlightInfoAsync(VNData.Result.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, VNData.Result.Session, listhanche, listCheckBaggageVn, listCheckBaggageVnJq);
                    lstFlightConvert.AddRange(lstvalue);
                }
            }
            if (VJDATA != null)
            {
                if (VJDATA.Result != null)
                {
                    var listhanche = new List<MonthSearch.HanchesohieuInfo>();
                    using (var service = new MonthSearch.MonthSearchSoapClient())
                    {
                        var srlisthanche = await service.GetlisthancheAsync("JQ");
                        if (srlisthanche != null)
                            if (srlisthanche.Body != null)
                                if (srlisthanche.Body.GetlisthancheResult != null)
                                    listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
                    }
                    if (VJDATA.Result.ListFareData == null)
                        VJDATA.Result.ListFareData = new List<FareData>().ToArray();
                    var lstvalue = await common.AddLccFlightInfoAsync(VJDATA.Result.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, VJDATA.Result.Session, listhanche, new List<BaggageVn>(), new List<BaggageVnJq>());
                    lstFlightConvert.AddRange(lstvalue);
                }
            }
            if (QHDATA != null)
            {
                if (QHDATA.Result != null)
                {
                    var listhanche = new List<MonthSearch.HanchesohieuInfo>();
                    using (var service = new MonthSearch.MonthSearchSoapClient())
                    {
                        var srlisthanche = await service.GetlisthancheAsync("QH");
                        if (srlisthanche != null)
                            if (srlisthanche.Body != null)
                                if (srlisthanche.Body.GetlisthancheResult != null)
                                    listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
                    }
                    if (QHDATA.Result.ListFareData == null)
                        QHDATA.Result.ListFareData = new List<FareData>().ToArray();
                    var lstvalue = await common.AddLccFlightInfoAsync(QHDATA.Result.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, QHDATA.Result.Session, listhanche, new List<BaggageVn>(), new List<BaggageVnJq>());
                    lstFlightConvert.AddRange(lstvalue);
                }
            }
            //if (JQDATA != null)
            //{
            //    if (JQDATA.Result != null)
            //    {
            //        var listhanche = new List<MonthSearch.HanchesohieuInfo>();
            //        using (var service = new MonthSearch.MonthSearchSoapClient())
            //        {
            //            var srlisthanche = await service.GetlisthancheAsync("JQ");
            //            if (srlisthanche != null)
            //                if (srlisthanche.Body != null)
            //                    if (srlisthanche.Body.GetlisthancheResult != null)
            //                        listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
            //        }
            //        var lstvalue = await common.AddLccFlightInfoJQAsync(JQDATA.Result.ListDepartFlight, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
            //                Infant ?? 0, JQDATA.Result.Session, listhanche);

            //        lstFlightConvert.AddRange(lstvalue);
            //        if (JQDATA.Result.ListFareData == null)
            //            JQDATA.Result.ListFareData = new List<VDMFareData>().ToArray();
            //        if (JQDATA.Result.ListReturnFlight != null)
            //        {
            //            if (JQDATA.Result.ListReturnFlight.Count() > 0)
            //            {
            //                lstvalue = await common.AddLccFlightInfoJQAsync(JQDATA.Result.ListReturnFlight, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
            //                Infant ?? 0, JQDATA.Result.Session, listhanche);

            //                lstFlightConvert.AddRange(lstvalue);
            //            }
            //        }
            //    }
            //}
            if (VUDATA != null)
            {
                if (VUDATA.Result != null)
                {
                    var listhanche = new List<MonthSearch.HanchesohieuInfo>();
                    using (var service = new MonthSearch.MonthSearchSoapClient())
                    {
                        var srlisthanche = await service.GetlisthancheAsync("VU");
                        if (srlisthanche != null)
                            if (srlisthanche.Body != null)
                                if (srlisthanche.Body.GetlisthancheResult != null)
                                    listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
                    }
                    if (VUDATA.Result.ListFareData == null)
                        VUDATA.Result.ListFareData = new List<FareData>().ToArray();
                    var lstvalue = await common.AddLccFlightInfoAsync(VUDATA.Result.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, VUDATA.Result.Session, listhanche, new List<BaggageVn>(), new List<BaggageVnJq>());
                    lstFlightConvert.AddRange(lstvalue);
                }
            }
            if (lstFlightConvert.Count() > 0)
            {
                // param.FlightSearchResult = data;
                param.DepartureFlights = new List<VDMFareDataInfo>();
                param.ReturnFlights = new List<VDMFareDataInfo>();
                var ListAirline = new List<AirlineInfo>();
                var ListAirlineCodeseach = lstFlightConvert.GroupBy(z => z.Airline).Select(group => new { Airline = group.Key }).ToList();
                foreach (var item in ListAirlineCodeseach)
                {
                    if (ListAirline.Count(z => z.Code == item.Airline) <= 0)
                    {
                        var Getairlineinfos = common.Getairlineinfo(item.Airline);
                        if (Getairlineinfos != null)
                        {
                            ListAirline.Add(Getairlineinfos);
                        }
                    }
                }
                param.DepartureFlights.AddRange(lstFlightConvert.Where(z => z.StartPoint == DepartureAirportCode));
                if (Itinerary > 1)
                {
                    param.ReturnFlights.AddRange(lstFlightConvert.Where(z => z.StartPoint == DestinationAirportCode));
                }
                param.countDepartureFlights = param.DepartureFlights.Count();
                param.countReturnFlights = param.ReturnFlights.Count();
                if (param.countDepartureFlights > 0)
                {
                    foreach (var item in ListAirline)
                    {
                        var getobjmin = param.DepartureFlights.Where(z => z.Airline == item.Code).OrderBy(z => z.TotalPrice).FirstOrDefault();
                        if (getobjmin != null)
                        {
                            item.GiareNhat = (getobjmin.FareAdt + getobjmin.TaxAdt + getobjmin.FeeAdt);
                        }
                    }
                }
                param.ListAirline = ListAirline;
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


            }
            #endregion trong nước
            HttpContext.Current.Session[Constant.SessionSeach] = param;
            return param;
        }
        private async Task<SearchResponse> SeachByAirline(SearchRequest request, int? Itinerary, string DepartureAirportCode, string DestinationAirportCode, DateTime? startdate, DateTime? enddate, string Airline)
        {
            List<FlightRequest> listFlight = new List<FlightRequest>
            {
                new FlightRequest
                {
                    StartPoint = DepartureAirportCode,
                    EndPoint = DestinationAirportCode,
                    DepartDate = (startdate ?? DateTime.Now).ToString("ddMMyyyy"),
                    Airline = Airline
                }
            };
            if (Itinerary > 1)
            {
                listFlight.Add(new FlightRequest
                {
                    StartPoint = DestinationAirportCode,
                    EndPoint = DepartureAirportCode,
                    DepartDate = (enddate ?? DateTime.Now).ToString("ddMMyyyy"),
                    Airline = Airline
                });
            }
            request.ListFlight = listFlight.ToArray();
            var data = await apiClient.SeachAsync(request);
            return data;
        }
        private async Task<VDMFlightSearchResult> SeachByAirlineJQ(int? Itinerary, string DepartureAirportCode, string DestinationAirportCode, DateTime? startdate, DateTime? enddate, int Adult, int Children, int Infant)
        {
            var Authentication = new VDMAuthentication { HeaderUser = HeaderUserJQ, HeaderPassword = HeaderPasswordJQ };
            VDMSeachParam paramseach = new VDMSeachParam()
            {
                Adult = Adult,
                Children = Children,
                Infant = Infant,
                Authentication = Authentication,
                DepartureAirportCode = DepartureAirportCode,
                DestinationAirportCode = DestinationAirportCode,
                Enddate = enddate ?? DateTime.Now,
                Startdate = startdate ?? DateTime.Now,
                Itinerary = Itinerary ?? 0,
                Password = HeaderPasswordJQ,
                Username = HeaderUserJQ
            };
            //  var data = await apiClient.JetstarSearchTicketDomtic(paramseach);
            return new VDMFlightSearchResult();
        }
        public async Task<SeachParam> SeachQuocTe(List<SettingUserInfo> listsetting, int? Itinerary, string DepartureAirportCode,
            string DestinationAirportCode, string DepartureDate, string ReturnDate,
            int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder,
            string StopNum)
        {
            StopNum = StopNum == null ? "" : StopNum;
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
                LstFareData = new List<VDMFareDataInfo>(),
                TrongNuoc = false,
                StopNum = StopNum
            };
            var ListAirlineCode = new List<string>();
            if (!string.IsNullOrEmpty(AirlineCode))
            {
                var spi = AirlineCode.Split(';');
                foreach (var s in spi)
                {
                    if (!string.IsNullOrEmpty(s))
                        ListAirlineCode.Add(s.Trim().ToUpper());
                }
            }
            var listStopNum = new List<int>();
            if (!string.IsNullOrEmpty(StopNum))
            {
                var spi = StopNum.Split(';');
                foreach (var s in spi)
                {
                    if (!string.IsNullOrEmpty(s))
                        listStopNum.Add(int.Parse(s));
                }
            }
            if (Itinerary == 1)
                ReturnDate = "";
            var startdate = Utility.ConvertToDate(DepartureDate);
            var enddate = Utility.ConvertToDate(ReturnDate);
            SearchRequest request = new SearchRequest
            {
                HeaderUser = HeaderUser,
                HeaderPass = HeaderPassword,
                AgentAccount = Username,
                AgentPassword = Password,
                LanguageCode = "vi",
                Adt = Adult ?? 0,
                Chd = Children ?? 0,
                Inf = Infant ?? 0,
                ViewMode = "default",
            };
            List<FlightRequest> listFlight = new List<FlightRequest>
            {
                new FlightRequest
                {
                    StartPoint = DepartureAirportCode,
                    EndPoint = DestinationAirportCode,
                    DepartDate = (startdate ?? DateTime.Now).ToString("ddMMyyyy"),
                    Airline = ""
                }
            };
            if (Itinerary > 1)
            {
                listFlight.Add(new FlightRequest
                {
                    StartPoint = DestinationAirportCode,
                    EndPoint = DepartureAirportCode,
                    DepartDate = (enddate ?? DateTime.Now).ToString("ddMMyyyy"),
                    Airline = ""
                });
            }
            request.ListFlight = listFlight.ToArray();


            var Authentication = new VDMAuthentication { HeaderUser = HeaderUserJQ, HeaderPassword = HeaderPasswordJQ };
            VDMSeachParam paramseach = new VDMSeachParam()
            {
                Adult = Adult ?? 0,
                Children = Children ?? 0,
                Infant = Infant ?? 0,
                Authentication = Authentication,
                DepartureAirportCode = DepartureAirportCode,
                DestinationAirportCode = DestinationAirportCode,
                Enddate = enddate ?? DateTime.Now,
                Startdate = startdate ?? DateTime.Now,
                Itinerary = Itinerary ?? 0,
                Password = HeaderPasswordJQ,
                Username = HeaderUserJQ
            };
            var data = apiClient.SeachAsync(request);
            //var dataJQ = apiClient.JetstarSearchTicketInternational(paramseach);
            //await Task.WhenAll(data, dataJQ);
            await Task.WhenAll(data);
            var listAirline = new List<AirlineInfo>();
            var ListStopNum = new List<int>();
            param.LstFareData = new List<VDMFareDataInfo>();
            param.ListAirline = listAirline;
            param.ListStopNum = ListStopNum;
            if (data != null)
            {
                if (data.Result != null)
                {

                    var listvalue = data.Result.ListFareData.Where(z => !Utility.lstAirport.Contains(z.Airline)).ToArray();
                    if (param.Itinerary > 1)
                        param.LstFareData = common.AddFareDataInfo(listsetting, listvalue, ref listAirline, ref ListStopNum, startdate, enddate, data.Result.Session, DepartureAirportCode, DestinationAirportCode, Itinerary ?? 0).Where(z => z.ListFlight.Count(c => c.StartPoint == param.DestinationAirportCode) > 0).ToList();
                    else
                        param.LstFareData = common.AddFareDataInfo(listsetting, listvalue, ref listAirline, ref ListStopNum, startdate, enddate, data.Result.Session, DepartureAirportCode, DestinationAirportCode, Itinerary ?? 0);
                }
            }


            //if (dataJQ != null)
            //{
            //    if (dataJQ.Result != null)
            //    {
            //        if (dataJQ.Result.ListFareData != null)
            //            param.LstFareData.AddRange(common.AddFareDataInfoJQ(listsetting, dataJQ.Result.ListFareData, ref listAirline, ref ListStopNum, startdate, enddate, dataJQ.Result.Session, DepartureAirportCode, DestinationAirportCode, Itinerary ?? 0));
            //    }
            //}
            if (param.LstFareData.Count > 0)
            {
                param.countDepartureFlights = param.LstFareData.Count();
                if (ListAirlineCode.Count > 0)
                    param.LstFareData =
                        param.LstFareData.Where(z => ListAirlineCode.Contains(z.Airline.Trim().ToUpper()))
                            .ToList();
                if (listStopNum.Count > 0)
                    param.LstFareData = param.LstFareData.Where(z => listStopNum.Contains(z.StopNum)).ToList();
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
            HttpContext.Current.Session[Constant.SessionSeach] = param;
            return param;
        }
        public string GetDieuKienQuocTe(string fareid, int inti, string seachParam)
        {
            string htmls = "<div> Điều kiện giá áp dụng chung của website </div>";
            var listfateData = new List<ApiClient.Models.FareDataInfo>();
            var listFlight = new List<ApiClient.Models.FlightInfo>();
            var SeachRd = (SeachParam)HttpContext.Current.Session[Constant.SessionSeach];
            var FareDataInfo = SeachRd.FareDataList.FirstOrDefault(z => z.FareDataIdValue == fareid);
            foreach (var item in FareDataInfo.ListFlight)
            {
                listFlight.Add(new ApiClient.Models.FlightInfo
                {
                    FlightValue = item.FlightValue,
                });
            }
            listfateData.Add(new ApiClient.Models.FareDataInfo
            {
                Session = seachParam,
                FareDataId = FareDataInfo.FareDataId,
                ListFlight = listFlight.ToArray(),

            });
            FareRuleRequest request = new FareRuleRequest
            {
                HeaderUser = HeaderUser,
                HeaderPass = HeaderPassword,
                AgentAccount = Username,
                AgentPassword = Password,
                LanguageCode = "vi",
                ListFareData = listfateData.ToArray(),
            };
            var fetinfo = apiClient.Getfarerules(request);
            if (fetinfo != null)
            {
                if (fetinfo.ListFareRules == null)
                    return "";
                if (fetinfo.ListFareRules.Count() > 0)
                {
                    foreach (var rulesGroup in fetinfo.ListFareRules)
                    {
                        foreach (var RulesText in rulesGroup.ListRulesGroup)
                        {
                            foreach (var value in RulesText.ListRulesText)
                            {
                                htmls += "<div>" + value + "</div>";
                            }
                        }
                    }
                }
            }
            return htmls;
        }
        public string bookve(SeachParam param, ref DateTime Expireddate, ref List<Booking> rads, List<SettingUserInfo> listsetting, VDMUser userInfor, FormCollection forminput)
        {

            string code = "";
            string user = Username;
            string pass = Password;
            string usertem = "";
            string passtem = "";
            var listbooking = new List<Booking>();
            try
            {
                var Authentication = new VDMAuthentication { HeaderUser = HeaderUserJQ, HeaderPassword = HeaderPasswordJQ };
                string DepartureFlightAirline = param.DepartureFlight.Airline;
                string ReturnFlightAirline = param.ReturnFlight != null ? param.ReturnFlight.Airline : "";
                if (DepartureFlightAirline == ReturnFlightAirline || string.IsNullOrEmpty(ReturnFlightAirline))
                {
                    if (DepartureFlightAirline == "JQ")
                    {

                        var BookingRequest = new VDMBookingRequest()
                        {
                            Authentication = Authentication,
                            Itinerary = param.DepartureFlight.ItineraryType,
                            SessionDepart = param.DepartureFlight.SessionId,
                            DepartFlightId = param.DepartureFlight.FareDataId,
                            ListPassengers = GetlistPassengerJQ(listsetting, param, userInfor, forminput).ToArray(),
                            ContactInfo = GetContactJQ(listsetting, param, userInfor),
                        };
                        if (!string.IsNullOrEmpty(ReturnFlightAirline))
                        {
                            BookingRequest.SessionReturn = param.ReturnFlight.SessionId;
                            BookingRequest.ReturnFlightId = param.ReturnFlight.FareDataId;
                        }
                        var databookJQ = apiClient.bookJetstart(BookingRequest);
                        if (databookJQ != null)
                        {
                            var objBooking = new Booking()
                            {
                                Airline = databookJQ.Airline,
                                BookingCode = databookJQ.BookingCode,
                                ErrorMessage = databookJQ.ErrorMessage,
                                ExpiryDate = databookJQ.ExpiryDate,
                            };
                            listbooking.Add(objBooking);
                        }
                    }
                    else
                    {
                        common.Gettaikhoan(param.DepartureFlight.Airline, param.DepartureFlight.StartPoint, param.DepartureFlight.EndPoint, ref usertem, ref passtem);
                        if (!string.IsNullOrEmpty(usertem) && !string.IsNullOrEmpty(passtem))
                        {
                            user = usertem;
                            pass = passtem;
                        }
                        var lstFareDataInfo = new List<ApiClient.Models.FareDataInfo>();
                        ApiClient.Models.FareDataInfo fareData = new ApiClient.Models.FareDataInfo
                        {
                            Session = param.DepartureFlight.SessionId,
                            FareDataId = param.DepartureFlight.FareDataId,
                            AutoIssue = false,
                            CAcode = (param.DepartureFlight.Airline == "VN" ? GetDettingSystemKey("CACode") : ""),
                            ListFlight = new ApiClient.Models.FlightInfo[]
                            {
                             new ApiClient.Models.FlightInfo { FlightValue = param.DepartureFlight.FlightValue }
                            }
                        };
                        lstFareDataInfo.Add(fareData);
                        if (param.ReturnFlight != null)
                        {
                            FareDataInfo fareDataReturn = new FareDataInfo
                            {
                                Session = param.ReturnFlight.SessionId,
                                FareDataId = param.ReturnFlight.FareDataId,
                                AutoIssue = false,
                                CAcode = (param.ReturnFlight.Airline == "VN" ? GetDettingSystemKey("CACode") : ""),
                                ListFlight = new ApiClient.Models.FlightInfo[]
                                {
                                    new ApiClient.Models.FlightInfo { FlightValue = param.ReturnFlight.FlightValue }
                                }
                            };
                            lstFareDataInfo.Add(fareDataReturn);
                        }
                        BookRequest request = new BookRequest
                        {
                            HeaderUser = HeaderUser,
                            HeaderPass = HeaderPassword,
                            AgentAccount = user,
                            AgentPassword = pass,
                            LanguageCode = "vi",
                            BookType = "book-normal",
                            UseAgentContact = false,
                            Contact = GetContact(listsetting, param, userInfor),
                            ListPassenger = GetlistPassenger(listsetting, param, userInfor, forminput).ToArray(),
                            ListFareData = lstFareDataInfo.ToArray(),
                        };
                        var book = apiClient.Book(request);

                        saveBookResponse.save(request, book);
                        listbooking.AddRange(book.ListBooking);
                    }
                }
                else if (param.ReturnFlight != null)
                {
                    ///  book chiều đi
                    if (param.DepartureFlight.Airline == "JQ")
                    {
                        var cus = new VDMBookingRequestCustom()
                        {
                            Authentication = Authentication,
                            Itinerary = 1,
                            StartPoint = param.DepartureFlight.StartPoint,
                            EndPoint = param.DepartureFlight.EndPoint,
                            DepartDate = param.DepartureFlight.StartDate,
                            Adt = param.Adult ?? 0,
                            Chd = param.Children ?? 0,
                            Inf = param.Infant ?? 0,
                            AirlineDepart = param.DepartureFlight.Airline,
                            FlightNumberDepart = param.DepartureFlight.FlightNumber,
                            CompareOnBaseFare = true,
                            PriceDepart = param.DepartureFlight.FareAdtNet,
                            SessionDepart = param.DepartureFlight.SessionId,
                            DepartFlightId = param.DepartureFlight.FareDataId,
                            ListPassengers = GetlistPassengerJQ(listsetting, param, userInfor, forminput).ToArray(),
                            ContactInfo = GetContactJQ(listsetting, param, userInfor),
                        };
                        var databookJQ = apiClient.bookJetstartCustom(cus);
                        if (databookJQ != null)
                        {
                            var objBooking = new Booking()
                            {
                                Airline = databookJQ.Airline,
                                BookingCode = databookJQ.BookingCode,
                                ErrorMessage = databookJQ.ErrorMessage,
                                ExpiryDate = databookJQ.ExpiryDate,
                            };
                            listbooking.Add(objBooking);
                        }


                    }
                    else
                    {
                        common.Gettaikhoan(param.DepartureFlight.Airline, param.DepartureFlight.StartPoint, param.DepartureFlight.EndPoint, ref usertem, ref passtem);
                        if (!string.IsNullOrEmpty(usertem) && !string.IsNullOrEmpty(passtem))
                        {
                            user = usertem;
                            pass = passtem;
                        }
                        var lstFareDataInfo = new List<ApiClient.Models.FareDataInfo>();
                        ApiClient.Models.FareDataInfo fareData = new ApiClient.Models.FareDataInfo
                        {
                            Session = param.DepartureFlight.SessionId,
                            FareDataId = param.DepartureFlight.FareDataId,
                            AutoIssue = false,
                            CAcode = (param.DepartureFlight.Airline == "VN" ? GetDettingSystemKey("CACode") : ""),
                            ListFlight = new ApiClient.Models.FlightInfo[]
                            {
                            new ApiClient.Models.FlightInfo { FlightValue = param.DepartureFlight.FlightValue }
                            }
                        };
                        lstFareDataInfo.Add(fareData);
                        BookRequest request = new BookRequest
                        {
                            HeaderUser = HeaderUser,
                            HeaderPass = HeaderPassword,
                            AgentAccount = user,
                            AgentPassword = pass,
                            LanguageCode = "vi",
                            BookType = "book-normal",
                            UseAgentContact = false,
                            Contact = GetContact(listsetting, param, userInfor),
                            ListPassenger = GetlistPassenger(listsetting, param, userInfor, forminput).ToArray(),
                            ListFareData = lstFareDataInfo.ToArray(),
                        };
                        foreach (var item in request.ListPassenger)
                        {
                            if (item.ListBaggage != null)
                            {
                                var listbagg = item.ListBaggage.Where(z => z.Route == (param.DepartureFlight.StartPoint + param.DepartureFlight.EndPoint));
                                item.ListBaggage = listbagg.ToArray();
                            }
                        }
                        var bookvaluechieudi = apiClient.Book(request);
                        saveBookResponse.save(request, bookvaluechieudi);
                        listbooking.AddRange(bookvaluechieudi.ListBooking);
                    }
                    /// Book chiều về 
                    if (param.ReturnFlight.Airline == "JQ")
                    {
                        var cus = new VDMBookingRequestCustom()
                        {
                            Authentication = Authentication,
                            Itinerary = 1,
                            StartPoint = param.ReturnFlight.StartPoint,
                            EndPoint = param.ReturnFlight.EndPoint,
                            DepartDate = param.ReturnFlight.StartDate,
                            Adt = param.Adult ?? 0,
                            Chd = param.Children ?? 0,
                            Inf = param.Infant ?? 0,
                            AirlineDepart = param.ReturnFlight.Airline,
                            FlightNumberDepart = param.ReturnFlight.FlightNumber,
                            CompareOnBaseFare = true,
                            PriceDepart = param.ReturnFlight.FareAdtNet,
                            SessionReturn = param.ReturnFlight.SessionId,
                            ReturnFlightId = param.ReturnFlight.FareDataId,
                            ListPassengers = GetlistPassengerJQ(listsetting, param, userInfor, forminput).ToArray(),
                            ContactInfo = GetContactJQ(listsetting, param, userInfor),
                        };
                        var databookJQ = apiClient.bookJetstartCustom(cus);
                        if (databookJQ != null)
                        {
                            var objBooking = new Booking()
                            {
                                Airline = databookJQ.Airline,
                                BookingCode = databookJQ.BookingCode,
                                ErrorMessage = databookJQ.ErrorMessage,
                                ExpiryDate = databookJQ.ExpiryDate,
                            };
                            listbooking.Add(objBooking);
                        }
                    }
                    else
                    {
                        common.Gettaikhoan(param.ReturnFlight.Airline, param.ReturnFlight.StartPoint, param.ReturnFlight.EndPoint, ref usertem, ref passtem);
                        if (!string.IsNullOrEmpty(usertem) && !string.IsNullOrEmpty(passtem))
                        {
                            user = usertem;
                            pass = passtem;
                        }
                        var lstFareDataInfoReturn = new List<ApiClient.Models.FareDataInfo>();
                        ApiClient.Models.FareDataInfo fareDataReturn = new ApiClient.Models.FareDataInfo
                        {
                            Session = param.ReturnFlight.SessionId,
                            FareDataId = param.ReturnFlight.FareDataId,
                            AutoIssue = false,
                            CAcode = (param.ReturnFlight.Airline == "VN" ? GetDettingSystemKey("CACode") : ""),
                            ListFlight = new ApiClient.Models.FlightInfo[]
                            {
                            new ApiClient.Models.FlightInfo { FlightValue = param.ReturnFlight.FlightValue }
                            }
                        };
                        lstFareDataInfoReturn.Add(fareDataReturn);
                        BookRequest requestReturn = new BookRequest
                        {
                            HeaderUser = HeaderUser,
                            HeaderPass = HeaderPassword,
                            AgentAccount = user,
                            AgentPassword = pass,
                            LanguageCode = "vi",
                            BookType = "book-normal",
                            UseAgentContact = false,
                            Contact = GetContact(listsetting, param, userInfor),
                            ListPassenger = GetlistPassenger(listsetting, param, userInfor, forminput).ToArray(),
                            ListFareData = lstFareDataInfoReturn.ToArray(),
                        };
                        foreach (var item in requestReturn.ListPassenger)
                        {
                            if (item.ListBaggage != null)
                            {
                                var listbagg = item.ListBaggage.Where(z => z.Route == (param.ReturnFlight.StartPoint + param.ReturnFlight.EndPoint));
                                item.ListBaggage = listbagg.ToArray();
                            }
                        }
                        var bookvaluechieuve = apiClient.Book(requestReturn);
                        saveBookResponse.save(requestReturn, bookvaluechieuve);
                        listbooking.AddRange(bookvaluechieuve.ListBooking);
                    }
                }
                if (listbooking.Count > 0)
                {
                    var objVN = listbooking.Where(z => z.Airline == "VN").FirstOrDefault();
                    if (objVN != null)
                    {
                        if (!string.IsNullOrEmpty(objVN.BookingCode))
                        {
                            var objContact = GetContact(listsetting, param, userInfor);
                            try
                            {
                                var requestsendmail = new SendMailRequest
                                {
                                    HeaderUser = HeaderUser,
                                    HeaderPass = HeaderPassword,
                                    AgentAccount = Username,
                                    AgentPassword = Password,
                                    Airline = "VN",
                                    Language = "vi",
                                    BookingCode = objVN.BookingCode,
                                    Email = objContact.Email,
                                };
                                var book = apiClient.SendMail(requestsendmail);
                            }
                            catch
                            {
                            }
                        }
                    }
                    foreach (var pnr in listbooking)
                    {
                        rads.Add(pnr);
                    }
                    foreach (var pnr in listbooking)
                    {
                        if (!string.IsNullOrEmpty(pnr.BookingCode))
                        {
                            code += pnr.BookingCode + "-";
                            Expireddate = pnr.ExpiryDate;
                            // rads.Add(pnr);
                        }
                        else
                        {
                            code = "FAIL";
                            Expireddate = pnr.ExpiryDate;
                            if (Expireddate < DateTime.Now)
                            {
                                pnr.ExpiryDate = DateTime.Now.AddHours(12);
                                Expireddate = DateTime.Now.AddHours(12);
                            }
                            break;
                        }
                    }
                    if (code != "FAIL" && code.Trim() != "-" && code.Trim() == "")
                    {
                        foreach (var pnr in listbooking)
                        {
                            if (param.ReturnFlight != null)
                            {
                                //if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "E" || param.DepartureFlight.FareClass == "P" || param.DepartureFlight.FareClass == "A" || param.DepartureFlight.FareClass == "T" || param.DepartureFlight.FareClass == "G" || param.ReturnFlight.FareClass == "T" || param.ReturnFlight.FareClass == "A" || param.ReturnFlight.FareClass == "E" || param.ReturnFlight.FareClass == "P" || param.ReturnFlight.FareClass == "G"))
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "A" || param.ReturnFlight.FareClass == "A"))

                                {
                                    pnr.ExpiryDate = DateTime.Now.AddHours(4);
                                }
                            }
                            else
                            {
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "A"))
                                {
                                    pnr.ExpiryDate = DateTime.Now.AddHours(4);
                                }
                            }
                        }
                        var datetime = listbooking.Min(z => z.ExpiryDate);
                        Expireddate = datetime;
                    }
                    if (code.Trim() == "-")
                        code = "";
                    if (!string.IsNullOrEmpty(code) && code != "FAIL")
                    {
                        foreach (var pnr in listbooking)
                        {
                            if (param.ReturnFlight != null)
                            {
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "A" || param.ReturnFlight.FareClass == "A"))
                                {
                                    pnr.ExpiryDate = DateTime.Now.AddHours(4);
                                }
                            }
                            else
                            {
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "A"))
                                {
                                    pnr.ExpiryDate = DateTime.Now.AddHours(4);
                                }
                            }
                        }
                        var datetime = listbooking.Min(z => z.ExpiryDate);
                        Expireddate = datetime;
                        code = code.Substring(0, code.Length - 1);
                    }
                }
                else
                {
                    code = "FAIL";
                    Expireddate = DateTime.Now.AddHours(24);
                    var listpnr = new List<Booking>();
                    listpnr.Add(new Booking { BookingCode = code, ErrorMessage = "Không thể đặt chỗ vào lúc này.", ExpiryDate = Expireddate });
                    rads = listpnr;
                }
            }
            catch (Exception ex)
            {
                code = "";
                Expireddate = DateTime.Now.AddHours(24);
                var listpnr = new List<Booking>();
                listpnr.Add(new Booking
                {
                    BookingCode = code,
                    ErrorMessage = ex.Message,
                    ExpiryDate = Expireddate
                });
                rads = listpnr;
            }
            if (code == "FAIL")
                code = "";
            return code != null ? code.Trim() : "";
        }
        public static string GetDettingSystemKey(string key)
        {
            var param = new SettingSystemParam() { Key = key };
            var biz = new SysSettingBiz();
            biz.GetInfoBykey(param);
            if (param.SettingSystemInfo != null)
                return param.SettingSystemInfo.Giatri;
            return "";
        }
        public string bookInternational(SeachParam param, ref DateTime Expireddate, ref Booking pnr, List<SettingUserInfo> listsetting, VDMUser userInfor, FormCollection forminput)
        {

            string code = "";
            var listbooking = new List<Booking>();
            try
            {
                if (!string.IsNullOrEmpty(param.FareDataIdValue))
                {

                    var DepartureFlight = param.FareDataInfo.ListFlight.First(z => z.FlightId == param.DepartureFlightId);
                    if (DepartureFlight != null)
                    {
                        if (Utility.lstAirport.Count(z => z == param.FareDataInfo.Airline) > 0)
                        {
                            var Authentication = new VDMAuthentication { HeaderUser = HeaderUserJQ, HeaderPassword = HeaderPasswordJQ };
                            var BookingRequest = new VDMBookingRequest()
                            {
                                Authentication = Authentication,
                                Itinerary = param.FareDataInfo.Itinerary,
                                BookType = "international",
                                SessionDepart = param.FareDataInfo.SessionId,
                                SessionReturn = param.FareDataInfo.SessionId,
                                FareDataId = param.FareDataInfo.FareDataId,
                                DepartFlightId = param.DepartureFlightId ?? 0,
                                ReturnFlightId = param.ReturnFlightId ?? 0,
                                ListPassengers = GetlistPassengerJQ(listsetting, param, userInfor, forminput).ToArray(),
                                ContactInfo = GetContactJQ(listsetting, param, userInfor),
                            };
                            var databookJQ = apiClient.bookJetstartInternational(BookingRequest);
                            if (databookJQ != null)
                            {
                                var objBooking = new Booking()
                                {
                                    Airline = databookJQ.Airline,
                                    BookingCode = databookJQ.BookingCode,
                                    ErrorMessage = databookJQ.ErrorMessage,
                                    ExpiryDate = databookJQ.ExpiryDate,
                                };
                                listbooking.Add(objBooking);
                            }
                        }
                        else
                        {
                            var lstFareDataInfo = new List<ApiClient.Models.FareDataInfo>();
                            FareDataInfo fareData = new FareDataInfo
                            {
                                Session = param.FareDataInfo.SessionId,
                                FareDataId = param.FareDataInfo.FareDataId,
                                AutoIssue = false
                            };
                            List<ApiClient.Models.FlightInfo> listFlight = new List<ApiClient.Models.FlightInfo>();

                            listFlight.Add(new ApiClient.Models.FlightInfo { FlightValue = DepartureFlight.FlightValue });
                            if (param.ReturnFlightId > 0)
                            {
                                var ReturnFlight = param.FareDataInfo.ListFlight.First(z => z.FlightId == param.ReturnFlightId);
                                if (ReturnFlight != null)
                                    listFlight.Add(new ApiClient.Models.FlightInfo { FlightValue = ReturnFlight.FlightValue });
                            }
                            fareData.ListFlight = listFlight.ToArray();
                            lstFareDataInfo.Add(fareData);
                            BookRequest request = new BookRequest
                            {
                                HeaderUser = HeaderUser,
                                HeaderPass = HeaderPassword,
                                AgentAccount = Username,
                                AgentPassword = Password,
                                LanguageCode = "vi",
                                BookType = "book-normal",
                                UseAgentContact = false,
                                Contact = GetContact(listsetting, param, userInfor),
                                ListPassenger = GetlistPassenger(listsetting, param, userInfor, forminput).ToArray(),
                                ListFareData = lstFareDataInfo.ToArray(),
                            };
                            var book = apiClient.Book(request);
                            listbooking.AddRange(book.ListBooking);
                        }
                        if (listbooking != null && listbooking.Count > 0)
                        {
                            foreach (var item in listbooking)
                            {
                                pnr = item;
                                code = item.BookingCode;
                                Expireddate = item.ExpiryDate;
                                break;
                            }

                        }
                        else
                        {
                            code = "FAIL";
                            Expireddate = DateTime.Now.AddHours(24);
                            pnr = new Booking();
                            pnr.ExpiryDate = Expireddate;
                        }
                        if (Expireddate < DateTime.Now)
                        {
                            Expireddate = DateTime.Now.AddHours(12);
                        }
                    }
                    else
                    {
                        code = "FAIL";
                        Expireddate = DateTime.Now.AddHours(24);
                        pnr = new Booking();
                        pnr.ExpiryDate = Expireddate;
                    }
                }
            }
            catch
            {
                code = "FAIL";
                Expireddate = DateTime.Now.AddHours(24);
                pnr = new Booking();
                pnr.ExpiryDate = Expireddate;
            }
            if (code == "FAIL")
                code = "";
            return code;
        }
        public List<Passenger> GetlistPassenger(List<SettingUserInfo> listsetting, SeachParam param, VDMUser userinfor, FormCollection form)
        {

            List<Passenger> listPax = new List<Passenger>();
            var index = 1;
            foreach (var passenger in param.Passengerlist)
            {
                var value = form.GetValue("txtDate" + (index - 1));
                if (value != null)
                    passenger.Birthday = Utility.ConvertToDate(value.AttemptedValue);
                var fullname = passenger.FullName.Split(' ');

                string LastName = "";
                for (int i = 1; i < fullname.Count(); i++)
                {
                    LastName += fullname[i] + " ";
                }
                var pax = new Passenger
                {
                    Index = index,
                    FirstName = fullname.Count() > 1 ? Utility.ConvertToUnsign(fullname[0]).Trim().ToUpper() : "",
                    LastName = Utility.ConvertToUnsign(LastName).Trim().ToUpper(),
                    Gender = passenger.Sex != Constant.Gender.Female,
                };
                switch (passenger.TypeID)
                {
                    case Constant.TypePassenger.NguoiLon:
                        {
                            pax.Type = "ADT";
                            pax.Birthday = passenger.Birthday.HasValue ? passenger.Birthday.Value.Date.ToString("ddMMyyyy") : "12031986";
                            break;
                        }
                    case Constant.TypePassenger.TreEm:
                        {
                            pax.Type = "CHD";
                            if (!string.IsNullOrEmpty(passenger.Nam) && !string.IsNullOrEmpty(passenger.Thang) && !string.IsNullOrEmpty(passenger.Ngay))
                            {
                                pax.Birthday = (new DateTime(int.Parse(passenger.Nam), int.Parse(passenger.Thang), int.Parse(passenger.Ngay))).ToString("ddMMyyyy");
                            }
                            else
                            {
                                pax.Birthday = (passenger.Birthday.HasValue ? passenger.Birthday.Value : DateTime.Now.AddYears(-3)).ToString("ddMMyyyy");
                            }
                            break;
                        }
                    case Constant.TypePassenger.EmBe:
                        {
                            pax.Type = "INF";
                            if (!string.IsNullOrEmpty(passenger.Nam) && !string.IsNullOrEmpty(passenger.Thang) && !string.IsNullOrEmpty(passenger.Ngay))
                            {
                                pax.Birthday = (new DateTime(int.Parse(passenger.Nam), int.Parse(passenger.Thang), int.Parse(passenger.Ngay))).ToString("ddMMyyyy");
                            }
                            else
                            {
                                pax.Birthday = (passenger.Birthday.HasValue ? passenger.Birthday.Value : DateTime.Now.AddYears(-3)).ToString("ddMMyyyy");
                            }
                            break;
                        }
                }
                List<Baggage> listBaggage = new List<Baggage>();
                if (!string.IsNullOrEmpty(passenger.BaggageDepartID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DepartureAirportCode + param.DestinationAirportCode) && z.Code == passenger.BaggageDepartID);
                    if (objpass != null)
                    {
                        listBaggage.Add(new Baggage()
                        {
                            Code = passenger.BaggageDepartID,
                            Name = passenger.BaggageDepartID,
                            Value = passenger.BaggageDepartID,
                            Route = objpass.Route,
                            Price = objpass.Price,
                            Currency = "VND"
                        });
                    }
                }
                if (!string.IsNullOrEmpty(passenger.BaggageReturnID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DestinationAirportCode + param.DepartureAirportCode) && z.Code == passenger.BaggageReturnID);
                    if (objpass != null)
                    {
                        listBaggage.Add(new Baggage()
                        {
                            Code = passenger.BaggageReturnID,
                            Name = passenger.BaggageReturnID,
                            Value = passenger.BaggageReturnID,
                            Route = objpass.Route,
                            Price = objpass.Price,
                            Currency = "VND"
                        });
                    }
                }
                pax.ListBaggage = listBaggage.ToArray();
                listPax.Add(pax);
                index++;
            }
            return listPax;
        }
        public Contact GetContact(List<SettingUserInfo> listsetting, SeachParam param, VDMUser userinfor)
        {
            var hoten = param.BkBooking.Name.Split(' ');
            string LastName = "";
            for (int i = 1; i < hoten.Count(); i++)
            {
                LastName += hoten[i] + " ";
            }
            var listsys = listsetting;
            var email = "";
            var objmail = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
            if (objmail != null)
            {
                email = objmail.Value;
            }
            if (userinfor != null)
            {
                listsys = CommonUI.GetSettingByUser(userinfor.UserName, false);
                var objmail1 = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
                if (objmail1 != null)
                {
                    if (!string.IsNullOrEmpty(objmail1.Value))
                        email = objmail1.Value;
                }
            }
            Contact contact = new Contact
            {
                Gender = true,
                FirstName = hoten.Count() > 1 ? Utility.ConvertToUnsign(hoten[0]) : "",
                LastName = Utility.ConvertToUnsign(LastName).Trim(),
                Phone = param.BkBooking.Phone,
                Email = (!string.IsNullOrEmpty(param.BkBooking.Email) ? param.BkBooking.Email : email),
                AgentPhone = param.BkBooking.Phone,
                AgentEmail = (!string.IsNullOrEmpty(email) ? email : param.BkBooking.Email),
                Address = !string.IsNullOrEmpty(param.BkBooking.Address) ? param.BkBooking.Address : "264E Lê Văn Sỹ P14 Q3 HCM",
            };
            return contact;
        }
        public List<VDMPassenger> GetlistPassengerJQ(List<SettingUserInfo> listsetting, SeachParam param, VDMUser userinfor, FormCollection form)
        {

            List<VDMPassenger> listPax = new List<VDMPassenger>();
            var index = 1;
            foreach (var passenger in param.Passengerlist)
            {
                var value = form.GetValue("txtDate" + (index - 1));
                if (value != null)
                    passenger.Birthday = Utility.ConvertToDate(value.AttemptedValue);
                var fullname = passenger.FullName.Split(' ');

                string LastName = "";
                for (int i = 1; i < fullname.Count(); i++)
                {
                    LastName += fullname[i] + " ";
                }
                var pax = new VDMPassenger
                {
                    Index = index,
                    FirstName = fullname.Count() > 1 ? Utility.ConvertToUnsign(fullname[0]).Trim().ToUpper() : "",
                    LastName = Utility.ConvertToUnsign(LastName).Trim().ToUpper(),
                    Gender = passenger.Sex != Constant.Gender.Female,
                };
                switch (passenger.TypeID)
                {
                    case Constant.TypePassenger.NguoiLon:
                        {
                            pax.Type = "ADT";
                            pax.Birthday = passenger.Birthday.HasValue
                                ? Utility.GetDateFormat(passenger.Birthday.Value.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy")
                                : Utility.GetDateFormat("12/03/1986", "dd/MM/yyyy");
                            break;
                        }
                    case Constant.TypePassenger.TreEm:
                        {
                            pax.Type = "CHD";
                            if (!string.IsNullOrEmpty(passenger.Nam) && !string.IsNullOrEmpty(passenger.Thang) && !string.IsNullOrEmpty(passenger.Ngay))
                            {
                                pax.Birthday = new DateTime(int.Parse(passenger.Nam), int.Parse(passenger.Thang), int.Parse(passenger.Ngay));
                            }
                            else
                            {
                                pax.Birthday = passenger.Birthday.HasValue ? passenger.Birthday.Value : DateTime.Now.AddYears(-3);
                            }
                            break;
                        }
                    case Constant.TypePassenger.EmBe:
                        {
                            pax.Type = "INF";
                            if (!string.IsNullOrEmpty(passenger.Nam) && !string.IsNullOrEmpty(passenger.Thang) && !string.IsNullOrEmpty(passenger.Ngay))
                            {
                                pax.Birthday = new DateTime(int.Parse(passenger.Nam), int.Parse(passenger.Thang), int.Parse(passenger.Ngay));
                            }
                            else
                            {
                                pax.Birthday = passenger.Birthday.HasValue ? passenger.Birthday.Value : DateTime.Now.AddYears(-3);
                            }
                            break;
                        }
                }

                if (!string.IsNullOrEmpty(passenger.BaggageDepartID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DepartureAirportCode + param.DestinationAirportCode) && z.Code == passenger.BaggageDepartID);
                    if (objpass != null)
                    {
                        if (Utility.lstAirport.Contains(objpass.Airline))
                        {
                            pax.BaggageDeparture = !string.IsNullOrEmpty(passenger.BaggageDepartID) ? passenger.BaggageDepartID : "";
                            pax.BaggageDepartValue = passenger.BaggageDepartID;
                            pax.BaggageDepartPrice = (decimal)objpass.Price;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(passenger.BaggageReturnID))
                {
                    var objpass = param.BaggageInfo.ListBaggage.FirstOrDefault(z => z.Route == (param.DestinationAirportCode + param.DepartureAirportCode) && z.Code == passenger.BaggageReturnID);
                    if (objpass != null)
                    {
                        if (Utility.lstAirport.Contains(objpass.Airline))
                        {
                            pax.BaggageReturn = !string.IsNullOrEmpty(passenger.BaggageReturnID) ? passenger.BaggageReturnID : "";
                            pax.BaggageReturnValue = passenger.BaggageReturnID;
                            pax.BaggageDepartPrice = (decimal)objpass.Price;
                        }
                    }
                }

                listPax.Add(pax);
                index++;
            }
            return listPax;
        }
        public VDMContact GetContactJQ(List<SettingUserInfo> listsetting, SeachParam param, VDMUser userinfor)
        {
            var hoten = param.BkBooking.Name.Split(' ');
            string LastName = "";
            for (int i = 1; i < hoten.Count(); i++)
            {
                LastName += hoten[i] + " ";
            }
            var listsys = listsetting;
            var email = "";
            var objmail = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
            if (objmail != null)
            {
                email = objmail.Value;
            }
            if (userinfor != null)
            {
                listsys = CommonUI.GetSettingByUser(userinfor.UserName, false);
                var objmail1 = listsys.FirstOrDefault(z => z.Name == "Email_NhanMailHang");
                if (objmail1 != null)
                {
                    if (!string.IsNullOrEmpty(objmail1.Value))
                        email = objmail1.Value;
                }
            }
            VDMContact contact = new VDMContact
            {
                Gender = true,
                FirstName = hoten.Count() > 1 ? Utility.ConvertToUnsign(hoten[0]) : "",
                LastName = Utility.ConvertToUnsign(LastName).Trim(),
                Phone = param.BkBooking.Phone,
                Email = (!string.IsNullOrEmpty(param.BkBooking.Email) ? param.BkBooking.Email : email),
                AgentPhone = param.BkBooking.Phone,
                AgentEmail = (!string.IsNullOrEmpty(email) ? email : param.BkBooking.Email),
                Address = !string.IsNullOrEmpty(param.BkBooking.Address) ? param.BkBooking.Address : "264E Lê Văn Sỹ P14 Q3 HCM",
            };
            return contact;
        }
        public SeachParam LoadBooktrongnuoc(AspNetUserInfo usersite, string CodeReturn, string CodeDepart, VDMUser userinfor)
        {
            var param = new SeachParam();
            param = (SeachParam)HttpContext.Current.Session[Constant.SessionSeach];
            var nguoilon = 0;
            var treem = 0;
            var embe = 0;
            var AirlineCode = "";
            var ItineraryType = 0;
            var DepartureSelectValue = "";
            var ReturnSelectValue = "";
            var ReturnAirlineCode = "";
            var DepartureSessionId = "";
            var ReturnSessionId = "";
            var SessionID = "";
            var DepartureFareDataId = 0;
            var ReturnFareDataId = 0;
            if (param.DepartureFlight != null)
            {
                SessionID = param.DepartureFlight.SessionId;
                DepartureSelectValue = param.DepartureFlight.FlightValue;
                ItineraryType = param.DepartureFlight.ItineraryType;
                DepartureSessionId = param.DepartureFlight.SessionId;
                AirlineCode = param.DepartureFlight.Airline;
                if (param.ReturnFlight != null)
                    ReturnAirlineCode = param.ReturnFlight.Airline;
                nguoilon = param.DepartureFlight.Adt;
                treem = param.DepartureFlight.Chd;
                embe = param.DepartureFlight.Inf;
                DepartureFareDataId = param.DepartureFlight.FareDataId;
            }
            if (param.ReturnFlight != null)
            {
                ReturnSessionId = param.ReturnFlight.SessionId;
                ReturnAirlineCode = param.ReturnFlight.Airline;
                ReturnSelectValue = param.ReturnFlight.FlightValue;
                ReturnFareDataId = param.ReturnFlight.FareDataId;
            }
            var Passengerlist = new List<BK_PassengerInfo>();
            for (int i = 0; i < nguoilon; i++)
            {
                var ckBaggage = new BK_PassengerInfo { TypeID = 1 };

                Passengerlist.Add(ckBaggage);
            }
            for (int i = 0; i < treem; i++)
            {
                var ckBaggage = new BK_PassengerInfo { TypeID = 2, Birthday = DateTime.Now.AddYears(-3) };
                ckBaggage.Listngay = new List<int>();
                ckBaggage.Listthang = new List<int>();
                ckBaggage.Listnamtreem = new List<int>();
                for (var j = 1; j <= 31; j++)
                {
                    ckBaggage.Listngay.Add(j);
                }
                for (var j = 1; j <= 12; j++)
                {

                    ckBaggage.Listthang.Add(j);
                }
                for (var j = DateTime.Now.AddYears(-12).Year; j <= DateTime.Now.Year; j++)
                {
                    ckBaggage.Listnamtreem.Add(j);
                }
                Passengerlist.Add(ckBaggage);
            }
            for (int i = 0; i < embe; i++)
            {
                var ckBaggage = new BK_PassengerInfo { TypeID = 3, Birthday = DateTime.Now.AddYears(-1) };
                ckBaggage.Listngay = new List<int>();
                ckBaggage.Listthang = new List<int>();
                ckBaggage.Listnamembe = new List<int>();
                for (var j = 1; j <= 31; j++)
                {
                    ckBaggage.Listngay.Add(j);
                }
                for (var j = 1; j <= 12; j++)
                {
                    ckBaggage.Listthang.Add(j);
                }
                for (var j = DateTime.Now.AddYears(-2).Year; j <= DateTime.Now.Year; j++)
                {
                    ckBaggage.Listnamembe.Add(j);
                }
                Passengerlist.Add(ckBaggage);
            }
            param.Passengerlist = Passengerlist;
            param.BaggageInfo = GetBaggageTrongNuoc(param, DepartureSessionId, DepartureFareDataId, DepartureSelectValue, ReturnFareDataId, ReturnSessionId, ReturnSelectValue, AirlineCode, ReturnAirlineCode);
            param.BkBooking = new BK_Booking { SessionID = SessionID };
            if (param.DepartureFlight != null)
            {
                if (userinfor != null)
                {
                    param.BkBooking.Name = userinfor.DisplayName;
                    param.BkBooking.Phone = userinfor.PhoneNumber;
                    param.BkBooking.Email = userinfor.Email;
                }
                else
                {
                    if (usersite != null)
                    {
                        param.BkBooking.Email = usersite.Email;
                    }
                }
                param.BkBooking.Status = 1;
                param.BkBooking.FromCity = param.DepartureFlight.StartPoint;
                param.BkBooking.ToCity = param.DepartureFlight.EndPoint;
                param.BkBooking.FlightDepart = param.DepartureFlight.FlightNumber;
            }
            if (param.ReturnFlight != null)
            {
                param.BkBooking.FlightReturn = param.ReturnFlight.FlightNumber;
            }
            return param;
        }
        public SeachParam LoadBookQuocTe(SeachParam param, VDMUser userinfor)
        {
            var Passengerlist = new List<BK_PassengerInfo>();
            for (int i = 0; i < param.Adult; i++)
            {
                var ckBaggage = new BK_PassengerInfo { TypeID = 1 };
                Passengerlist.Add(ckBaggage);
            }
            for (int i = 0; i < param.Children; i++)
            {
                var ckBaggage = new BK_PassengerInfo { TypeID = 2, Birthday = DateTime.Now.AddYears(-3) };
                ckBaggage.Listngay = new List<int>();
                ckBaggage.Listthang = new List<int>();
                ckBaggage.Listnamtreem = new List<int>();
                for (var j = 1; j <= 31; j++)
                {

                    ckBaggage.Listngay.Add(j);
                }
                for (var j = 1; j <= 12; j++)
                {

                    ckBaggage.Listthang.Add(j);
                }
                for (var j = DateTime.Now.AddYears(-12).Year; j <= DateTime.Now.Year; j++)
                {
                    ckBaggage.Listnamtreem.Add(j);
                }
                Passengerlist.Add(ckBaggage);
            }
            for (int i = 0; i < param.Infant; i++)
            {
                var ckBaggage = new BK_PassengerInfo { TypeID = 3, Birthday = DateTime.Now.AddYears(-1) };
                ckBaggage.Listngay = new List<int>();
                ckBaggage.Listthang = new List<int>();
                ckBaggage.Listnamembe = new List<int>();
                for (var j = 1; j <= 31; j++)
                {
                    ckBaggage.Listngay.Add(j);
                }
                for (var j = 1; j <= 12; j++)
                {
                    ckBaggage.Listthang.Add(j);
                }
                for (var j = DateTime.Now.AddYears(-2).Year; j <= DateTime.Now.Year; j++)
                {
                    ckBaggage.Listnamembe.Add(j);
                }
                Passengerlist.Add(ckBaggage);
            }
            param.Passengerlist = Passengerlist;
            param.BaggageInfo = GetBaggageQuocTe(param);
            param.BkBooking = new BK_Booking { SessionID = param.FareDataInfo.SessionId };
            if (userinfor != null)
            {
                param.BkBooking.Name = userinfor.DisplayName;
                param.BkBooking.Phone = userinfor.PhoneNumber;
                param.BkBooking.Email = userinfor.Email;
            }
            if (param.DepartureFlight != null)
            {


                param.BkBooking.Status = 1;
                param.BkBooking.FromCity = param.DepartureFlight.StartPoint;
                param.BkBooking.ToCity = param.DepartureFlight.EndPoint;
                param.BkBooking.FlightDepart = param.DepartureFlight.FlightNumber;
            }
            if (param.ReturnFlight != null)
            {
                param.BkBooking.FlightReturn = param.ReturnFlight.FlightNumber;
            }
            return param;
        }
        public BaggageResponse GetBaggageTrongNuoc(SeachParam param, string DepartureSessionId,
            int DepartureFareDataId, string DepartureSelectValue,
            int ReturnFareDataId, string ReturnSessionId, string ReturnSelectValue, string ArlineDeparture, string ArlineReturn)
        {
            var lstFareDataInfo = new List<FareDataInfo>();
            var baggageResponse = new BaggageResponse();
            var ListBaggage = new List<Baggage>();
            if (ArlineDeparture != "JQ")
            {
                FareDataInfo fareData = new FareDataInfo
                {
                    Session = DepartureSessionId,
                    FareDataId = DepartureFareDataId,
                    AutoIssue = false,
                    ListFlight = new FlightInfo[]
                    {
                            new FlightInfo { FlightValue = DepartureSelectValue }
                    }
                };
                if (param.DepartureFlight != null)
                {
                    if (ArlineDeparture == "VN")
                    {
                        if (param.DepartureFlight.Ishow23KgVN ?? false)
                            lstFareDataInfo.Add(fareData);
                    }
                    else
                    {
                        lstFareDataInfo.Add(fareData);
                    }
                }
            }
            if (ArlineDeparture == "JQ" && ArlineReturn != "JQ" && ArlineReturn != "")
            {
                FareDataInfo fareDataReturn = new FareDataInfo
                {
                    Session = ReturnSessionId,
                    FareDataId = ReturnFareDataId,
                    AutoIssue = false,
                    ListFlight = new ApiClient.Models.FlightInfo[]
                   {
                            new ApiClient.Models.FlightInfo { FlightValue = ReturnSelectValue }
                   }
                };
                if (param.ReturnFlight != null)
                {

                    if (ArlineReturn == "VN")
                    {
                        if (param.ReturnFlight.Ishow23KgVN ?? false)
                            lstFareDataInfo.Add(fareDataReturn);
                    }
                    else
                    {
                        lstFareDataInfo.Add(fareDataReturn);
                    }
                }

            }
            if (ArlineDeparture != "JQ" && ArlineReturn != "JQ" && ArlineReturn != "")
            {
                if (ReturnFareDataId > 0)
                {
                    FareDataInfo fareDataReturn = new FareDataInfo
                    {
                        Session = ReturnSessionId,
                        FareDataId = ReturnFareDataId,
                        AutoIssue = false,
                        ListFlight = new ApiClient.Models.FlightInfo[]
                        {
                            new ApiClient.Models.FlightInfo { FlightValue = ReturnSelectValue }
                        }
                    };
                    if (param.ReturnFlight != null)
                    {
                        if (ArlineReturn == "VN")
                        {
                            if (param.ReturnFlight.Ishow23KgVN ?? false)
                                lstFareDataInfo.Add(fareDataReturn);
                        }
                        else
                        {
                            lstFareDataInfo.Add(fareDataReturn);
                        }
                    }
                }
            }
            if (lstFareDataInfo.Count > 0)
            {
                BaggageRequest request = new BaggageRequest
                {
                    HeaderUser = HeaderUser,
                    HeaderPass = HeaderPassword,
                    AgentAccount = Username,
                    AgentPassword = Password,
                    LanguageCode = "vi",
                    ListFareData = lstFareDataInfo.ToArray()
                };
                var datadatacom = apiClient.Getbaggage(request);
                if (datadatacom != null)
                {
                    if (datadatacom.ListBaggage != null)
                        ListBaggage.AddRange(datadatacom.ListBaggage.ToList());
                }
            }
            if (ArlineDeparture == "JQ" || ArlineReturn == "JQ")
            {
                var Authentication = new VDMAuthentication
                {
                    HeaderUser = HeaderUserJQ,
                    HeaderPassword = HeaderPasswordJQ
                };
                var RequestInfo = new VDMRequestInfo()
                {
                    Authentication = Authentication,
                    AirlineCodeDepart = ArlineDeparture,
                    Itinerary = param.DepartureFlight.ItineraryType,
                    BookType = "domestic",
                    SessionDepart = DepartureSessionId,
                    DepartFlightId = DepartureFareDataId
                };
                if (ArlineDeparture != "JQ" && ArlineReturn == "JQ")
                {
                    RequestInfo.SessionDepart = ReturnSessionId;
                }
                if (param.ReturnFlight != null)
                {
                    RequestInfo.AirlineCodeReturn = ArlineReturn;
                    RequestInfo.SessionReturn = ReturnSessionId;
                    RequestInfo.ReturnFlightId = ReturnFareDataId;
                }
                var BaggagesJQ = apiClient.JetstarGetBaggages(RequestInfo);
                if (BaggagesJQ != null)
                {
                    if (BaggagesJQ.BaggageDepart != null && ArlineDeparture == "JQ")
                    {
                        var lst = (from n in BaggagesJQ.BaggageDepart
                                   select new Baggage
                                   {
                                       Airline = n.AirlineCode,
                                       Currency = n.Currency,
                                       Code = n.Code,
                                       Leg = 1,
                                       Name = n.Name,
                                       Price = n.Price,
                                       Route = param.DepartureFlight.StartPoint + param.DepartureFlight.EndPoint,
                                       Value = n.Value
                                   }).ToList();
                        ListBaggage.AddRange(lst);
                    }
                    if (BaggagesJQ.BaggageReturn != null && ArlineReturn == "JQ")
                    {
                        if (param.ReturnFlight != null)
                        {
                            var lst = (from n in BaggagesJQ.BaggageReturn
                                       select new Baggage
                                       {
                                           Airline = n.AirlineCode,
                                           Currency = n.Currency,
                                           Code = n.Code,
                                           Leg = 1,
                                           Name = n.Name,
                                           Price = n.Price,
                                           Route = param.ReturnFlight.StartPoint + param.ReturnFlight.EndPoint,
                                           Value = n.Value
                                       }).ToList();
                            ListBaggage.AddRange(lst);
                        }
                    }
                }
            }
            baggageResponse.ListBaggage = ListBaggage.ToArray();
            return baggageResponse;
        }
        public BaggageResponse GetBaggageQuocTe(SeachParam param)
        {
            var baggageResponse = new BaggageResponse();
            var ListBaggage = new List<Baggage>();
            var DepartureFlight = param.FareDataInfo.ListFlight.FirstOrDefault(z => z.FlightId == param.DepartureFlightId);
            if (DepartureFlight != null)
            {
                if (Utility.lstAirport.Contains(param.FareDataInfo.Airline))
                {
                    var Authentication = new VDMAuthentication
                    {
                        HeaderUser = HeaderUserJQ,
                        HeaderPassword = HeaderPasswordJQ
                    };
                    var RequestInfo = new VDMRequestInfo()
                    {
                        Authentication = Authentication,
                        Itinerary = param.FareDataInfo.Itinerary,
                        BookType = "international",
                        FareDataId = param.FareDataInfo.FareDataId,
                        AirlineCodeDepart = param.FareDataInfo.Airline,
                        SessionDepart = param.FareDataInfo.SessionId,
                        DepartFlightId = param.DepartureFlightId ?? 0,
                    };
                    if (param.FareDataInfo.Itinerary > 1)
                    {
                        RequestInfo.SessionReturn = param.FareDataInfo.SessionId;
                        RequestInfo.ReturnFlightId = param.ReturnFlightId ?? 0;
                    }
                    var BaggagesJQ = apiClient.JetstarGetBaggages(RequestInfo);
                    if (BaggagesJQ != null)
                    {
                        if (BaggagesJQ.BaggageDepart != null)
                        {
                            var lst = (from n in BaggagesJQ.BaggageDepart
                                       select new Baggage
                                       {
                                           Airline = n.AirlineCode,
                                           Currency = n.Currency,
                                           Code = n.Code,
                                           Leg = 1,
                                           Name = n.Name,
                                           Price = n.Price,
                                           Route = DepartureFlight.StartPoint + DepartureFlight.EndPoint,
                                           Value = n.Value
                                       }).ToList();
                            ListBaggage.AddRange(lst);
                        }
                        if (BaggagesJQ.BaggageReturn != null)
                        {
                            var ReturnFlight = param.FareDataInfo.ListFlight.FirstOrDefault(z => z.FlightId == param.ReturnFlightId);
                            if (ReturnFlight != null)
                            {
                                var lst = (from n in BaggagesJQ.BaggageReturn
                                           select new Baggage
                                           {
                                               Airline = n.AirlineCode,
                                               Currency = n.Currency,
                                               Code = n.Code,
                                               Leg = 1,
                                               Name = n.Name,
                                               Price = n.Price,
                                               Route = ReturnFlight.StartPoint + ReturnFlight.EndPoint,
                                               Value = n.Value
                                           }).ToList();
                                ListBaggage.AddRange(lst);
                            }
                        }
                    }
                    baggageResponse.ListBaggage = ListBaggage.ToArray();
                    return baggageResponse;
                }
                else
                {
                    var lstFareDataInfo = new List<FareDataInfo>();
                    List<ApiClient.Models.FlightInfo> lstFlightInfo = new List<ApiClient.Models.FlightInfo>();
                    ApiClient.Models.FareDataInfo fareData = new ApiClient.Models.FareDataInfo
                    {
                        Session = param.FareDataInfo.SessionId,
                        FareDataId = param.FareDataInfo.FareDataId,
                        AutoIssue = false,
                    };
                    lstFlightInfo.Add(new ApiClient.Models.FlightInfo { FlightValue = DepartureFlight.FlightValue });
                    if (param.Itinerary > 1)
                    {
                        var ReturnFlight = param.FareDataInfo.ListFlight.FirstOrDefault(z => z.FlightId == param.ReturnFlightId);
                        if (ReturnFlight != null)
                            lstFlightInfo.Add(new ApiClient.Models.FlightInfo { FlightValue = ReturnFlight.FlightValue });
                    }
                    fareData.ListFlight = lstFlightInfo.ToArray();
                    lstFareDataInfo.Add(fareData);
                    BaggageRequest request = new BaggageRequest
                    {
                        HeaderUser = HeaderUser,
                        HeaderPass = HeaderPassword,
                        AgentAccount = Username,
                        AgentPassword = Password,
                        LanguageCode = "vi",
                        ListFareData = lstFareDataInfo.ToArray()
                    };
                    baggageResponse = apiClient.Getbaggage(request);
                    return baggageResponse;
                }

            }
            return baggageResponse;
        }
        #region Vé tháng
        public SeachParam SeachVeThang(List<SettingUserInfo> listsetting, int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
            string StartMonth, string EndMonth,
            int? Adult, int? Children, int? Infant)
        {
            var param = new SeachParam
            {
                Itinerary = Itinerary.HasValue ? Itinerary.Value : 0,
                DepartureAirportCode = DepartureAirportCode,
                DestinationAirportCode = DestinationAirportCode,
                StartMonth = StartMonth,
                EndMonth = EndMonth,
                Adult = Adult.HasValue ? Adult.Value : 0,
                Children = Children.HasValue ? Children.Value : 0,
                Infant = Infant.HasValue ? Infant.Value : 0,
                TrongNuoc = true,
            };
            var StartMonthsp = StartMonth.Split('/');
            var starmountvalue = int.Parse(StartMonthsp[0]);
            var staryearvalue = int.Parse(StartMonthsp[1]);
            param.ReturnFlightsVeThang = new List<VeThangareDataInfo>();
            using (var service = new MonthSearch.MonthSearchSoapClient())
            {
                if (Itinerary > 1)
                {
                    var EndMonthsp = EndMonth.Split('/');
                    var endmountvalue = int.Parse(EndMonthsp[0]);
                    var endyearvalue = int.Parse(EndMonthsp[1]);
                    var listReturn = service.GetMonthlyTicket(endmountvalue, endyearvalue, DestinationAirportCode, DepartureAirportCode,
                        param.Adult.Value, param.Children.Value, param.Infant.Value, Uservethang, Pasvethang);

                    if (listReturn.Count() > 0)
                    {
                        foreach (var flightData in listReturn)
                        {
                            param.ReturnFlightsVeThang.Add(common.AddFlightFareInfoMonth(listsetting, flightData));
                        }
                    }
                }
                // chiều đi
                param.DepartureFlightsVeThang = new List<VeThangareDataInfo>();
                var listDeparture = service.GetMonthlyTicket(starmountvalue, staryearvalue, DepartureAirportCode, DestinationAirportCode, param.Adult.Value,
                    param.Children.Value, param.Infant.Value, Uservethang, Pasvethang);
                if (listDeparture.Count() > 0)
                {
                    foreach (var flightData in listDeparture)
                    {
                        param.DepartureFlightsVeThang.Add(common.AddFlightFareInfoMonth(listsetting, flightData));
                    }
                }
            }
            return param;
        }

        #endregion
    }
}