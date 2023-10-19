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

namespace VDMMutiline.SeachAndBook
{
    public class SeachAndBookCommon : BaseSeach
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        public SeachParam seachTrongNuoc(List<SettingUserInfo> listsetting, int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
            string DepartureDate, string ReturnDate,
            int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            HttpContext.Current.Session["SeachRd"] = null;
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
                FlightSearchResult = new SearchResponse(),
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
            var data = apiClient.Seach(request);
            if (data != null)
            {
                param.FlightSearchResult = data;
                param.DepartureFlights = new List<VDMFareDataInfo>();
                param.ReturnFlights = new List<VDMFareDataInfo>();
                var ListAirline = new List<AirlineInfo>();
                if (param.FlightSearchResult.ListFareData == null)
                    param.FlightSearchResult.ListFareData = new List<FareData>().ToArray();
                if (param.FlightSearchResult.ListFareData != null)
                {
                    var listhanche = new List<MonthSearch.HanchesohieuInfo>();
                    var ListAirlineCodeseach = param.FlightSearchResult.ListFareData.GroupBy(z => z.Airline).Select(group => new { Airline = group.Key }).ToList();
                    foreach (var item in ListAirlineCodeseach)
                    {
                        if (ListAirline.Count(z => z.Code == item.Airline) <= 0)
                        {
                            var Getairlineinfos = Getairlineinfo(item.Airline);
                            if (Getairlineinfos != null)
                            {
                                using (var service = new MonthSearch.MonthSearchSoapClient())
                                {
                                    listhanche.AddRange(service.Getlisthanche(item.Airline).ToList());
                                }
                                ListAirline.Add(Getairlineinfos);
                            }
                        }
                    }
                    var lstFlightConvert = AddLccFlightInfo(param.FlightSearchResult.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                     Infant ?? 0, param.FlightSearchResult.Session, listhanche);
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

            }
            #endregion trong nước
            HttpContext.Current.Session["SeachRd"] = null;
            HttpContext.Current.Session["SeachRd"] = param;
            return param;
        }

        public async Task<SeachParam> seachTrongNuocAsync(List<SettingUserInfo> listsetting, int? Itinerary, string DepartureAirportCode, string DestinationAirportCode,
         string DepartureDate, string ReturnDate,
         int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            HttpContext.Current.Session["SeachRd"] = null;
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
                FlightSearchResult = new SearchResponse(),
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
            var data = await apiClient.SeachAsync(request);


            if (data != null)
            {
                param.FlightSearchResult = data;
                param.DepartureFlights = new List<VDMFareDataInfo>();
                param.ReturnFlights = new List<VDMFareDataInfo>();
                var ListAirline = new List<AirlineInfo>();
                if (param.FlightSearchResult.ListFareData == null)
                    param.FlightSearchResult.ListFareData = new List<FareData>().ToArray();
                if (param.FlightSearchResult.ListFareData != null)
                {
                    var listhanche = new List<MonthSearch.HanchesohieuInfo>();
                    var ListAirlineCodeseach = param.FlightSearchResult.ListFareData.GroupBy(z => z.Airline).Select(group => new { Airline = group.Key }).ToList();
                    foreach (var item in ListAirlineCodeseach)
                    {
                        if (ListAirline.Count(z => z.Code == item.Airline) <= 0)
                        {
                            var Getairlineinfos = Getairlineinfo(item.Airline);
                            if (Getairlineinfos != null)
                            {
                                using (var service = new MonthSearch.MonthSearchSoapClient())
                                {
                                    var srlisthanche = await service.GetlisthancheAsync(item.Airline);
                                    if (srlisthanche != null)
                                        if (srlisthanche.Body != null)
                                            if (srlisthanche.Body.GetlisthancheResult != null)
                                                listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
                                }
                                ListAirline.Add(Getairlineinfos);
                            }
                        }
                    }
                    var lstFlightConvert = await AddLccFlightInfoAsync(param.FlightSearchResult.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                     Infant ?? 0, param.FlightSearchResult.Session, listhanche);
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

            }
            #endregion trong nước
            HttpContext.Current.Session["SeachRd"] = null;
            HttpContext.Current.Session["SeachRd"] = param;
            return param;
        }
        public SeachParam SeachQuocTe(List<SettingUserInfo> listsetting, int? Itinerary, string DepartureAirportCode,
            string DestinationAirportCode, string DepartureDate, string ReturnDate,
            int? Adult, int? Children, int? Infant, string AirlineCode, string Currency, string older, string typeolder,
            string StopNum)
        {
            StopNum = StopNum == null ? "" : StopNum;
            HttpContext.Current.Session["SeachRd"] = null;
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
                FareDatalist = new List<VDMFareDataInfo>(),
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
            var data = apiClient.Seach(request);

            if (data != null)
            {
                param.FlightSearchResult = data;
                var listAirline = new List<AirlineInfo>();
                var ListStopNum = new List<int>();
                if (param.FlightSearchResult.ListFareData == null)
                    param.FlightSearchResult.ListFareData = new List<FareData>().ToArray();
                param.FareDatalist = AddFareDataInfo(listsetting, data.ListFareData, ref listAirline, ref ListStopNum, startdate, enddate, data.Session, DepartureAirportCode, DestinationAirportCode, Itinerary ?? 0);
                param.ListAirline = listAirline;
                param.ListStopNum = ListStopNum;
                if (ListAirlineCode.Count > 0)
                    param.FareDatalist =
                        param.FareDatalist.Where(z => ListAirlineCode.Contains(z.Airline.Trim().ToUpper()))
                            .ToList();
                if (listStopNum.Count > 0)
                    param.FareDatalist = param.FareDatalist.Where(z => listStopNum.Contains(z.StopNum)).ToList();
                switch (older)
                {
                    case Constant.Olderve.Gia:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.FareDatalist = param.FareDatalist.OrderBy(z => z.TotalPrice).ToList();

                            }
                            else
                            {
                                param.FareDatalist = param.FareDatalist.OrderByDescending(z => z.TotalPrice).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.Thoigianbay:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.FareDatalist = param.FareDatalist.OrderBy(z => z.Duration).ToList();

                            }
                            else
                            {
                                param.FareDatalist = param.FareDatalist.OrderByDescending(z => z.Duration).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.HangHangkhong:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.FareDatalist = param.FareDatalist.OrderBy(z => z.Airline).ToList();
                            }
                            else
                            {
                                param.FareDatalist = param.FareDatalist.OrderByDescending(z => z.Airline).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.GioCatCanh:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.FareDatalist = param.FareDatalist.OrderBy(z => z.StartDate).ToList();
                            }
                            else
                            {
                                param.FareDatalist = param.FareDatalist.OrderByDescending(z => z.StartDate).ToList();
                            }
                            break;
                        }
                    case Constant.Olderve.GioHaCanh:
                        {
                            if (typeolder == Constant.Typeolder.Ascending)
                            {
                                param.FareDatalist = param.FareDatalist.OrderBy(z => z.EndDate).ToList();
                            }
                            else
                            {
                                param.FareDatalist = param.FareDatalist.OrderByDescending(z => z.EndDate).ToList();
                            }
                            break;
                        }
                    default:
                        {
                            param.FareDatalist = param.FareDatalist.OrderBy(z => z.TotalPrice).ToList();
                            break;
                        }
                }
                HttpContext.Current.Session["SeachRd"] = param;
            }

            return param;
        }
        public string GetDieuKienQuocTe(int fareid, int inti, string seachParam)
        {
            string htmls = "<div> Điều kiện giá áp dụng chung của website </div>";
            var listfateData = new List<ApiClient.Models.FareDataInfo>();
            var listFlight = new List<ApiClient.Models.FlightInfo>();
            var SeachRd = (SeachParam)HttpContext.Current.Session["SeachRd"];
            var FareDataInfo = SeachRd.FareDatalist.FirstOrDefault(z => z.FareDataId == fareid);
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
                FareDataId = fareid,
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
        public double GetPriceCongThem(List<SettingUserInfo> listinput, string codeAl)
        {
            double value = 0;
            switch (codeAl.Trim().ToUpper())
            {
                case "JQ":
                    {
                        var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeJetstar".Trim().ToUpper());
                        if (obj != null)
                        {
                            var dou = Utility.ConvertTodouble(obj.Value);
                            value = dou.HasValue ? dou.Value : 0;
                        }
                        break;
                    }
                case "VJ":
                    {
                        var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietJet".Trim().ToUpper());
                        if (obj != null)
                        {
                            var dou = Utility.ConvertTodouble(obj.Value);
                            value = dou.HasValue ? dou.Value : 0;
                        }
                        break;
                    }
                case "VN":
                    {
                        var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietnamAirline".Trim().ToUpper());
                        if (obj != null)
                        {
                            var dou = Utility.ConvertTodouble(obj.Value);
                            value = dou.HasValue ? dou.Value : 0;
                        }
                        break;
                    }
                case "QH":
                    {
                        var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeBamboo".Trim().ToUpper());
                        if (obj != null)
                        {
                            var dou = Utility.ConvertTodouble(obj.Value);
                            value = dou.HasValue ? dou.Value : 0;
                        }
                        break;
                    }
            }
            return value;
        }
        public double GetPriceCongThemhangG(List<SettingUserInfo> listinput, string codeAl, string classG)
        {
            double value = 0;
            if (!string.IsNullOrEmpty(classG))
            {
                if (codeAl.Trim().ToUpper() == "VN" && classG.Trim().ToUpper() == "G")
                {
                    var obj = listinput.FirstOrDefault(z =>
                        z.Name.Trim().ToUpper() == "FeeVietnamAirline_G".Trim().ToUpper());
                    if (obj != null)
                    {
                        var dou = Utility.ConvertTodouble(obj.Value);
                        value = dou.HasValue ? dou.Value : 0;
                    }
                }
            }
            return value;
        }
        private double GetPriceCongThemQuocTe(List<SettingUserInfo> listinput, string codehang)
        {
            double value = 0;
            if (codehang == "VN")
            {
                var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietnamAirlineInter".Trim().ToUpper());
                if (obj != null)
                {
                    var dou = Utility.ConvertTodouble(obj.Value);
                    value = dou.HasValue ? dou.Value : 0;
                }
            }
            else if (codehang == "VJ")
            {
                var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietJetInter".Trim().ToUpper());
                if (obj != null)
                {
                    var dou = Utility.ConvertTodouble(obj.Value);
                    value = dou.HasValue ? dou.Value : 0;
                }
            }
            else if (codehang == "JQ")
            {
                var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeJetstarInter".Trim().ToUpper());
                if (obj != null)
                {
                    var dou = Utility.ConvertTodouble(obj.Value);
                    value = dou.HasValue ? dou.Value : 0;
                }
            }
            else
            {
                var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeInternational".Trim().ToUpper());
                if (obj != null)
                {
                    var dou = Utility.ConvertTodouble(obj.Value);
                    value = dou.HasValue ? dou.Value : 0;
                }
            }
            return value;
        }
        public AirlineInfo Getairlineinfo(string code)
        {
            var dao = new AirlineDao();
            return dao.GetbyCode(code);
        }
        public async Task<IEnumerable<VDMFareDataInfo>> AddLccFlightInfoAsync(ApiClient.Models.FareData[] list, int ItineraryType,
            List<SettingUserInfo> listsetting, int nguoilon, int treem, int embe,
            string SessionId, List<MonthSearch.HanchesohieuInfo> listhanche)
        {
            var listvalue = new List<VDMFareDataInfo>();
            foreach (var objlist in list.OrderBy(z => z.FareAdt))
            {
                var getgiacongthem = GetPriceCongThem(listsetting, objlist.Airline);
                var objFlightInfoFirst = objlist.ListFlight.FirstOrDefault();
                var objFlightInfoLast = objlist.ListFlight.LastOrDefault();
                if (objFlightInfoFirst != null && objFlightInfoLast != null)
                {
                    VDMFareDataInfo ck = new VDMFareDataInfo();
                    ck.FareClass = (objFlightInfoFirst.FareClass ?? "").ToUpper();
                    ck.StartDate = objFlightInfoFirst.StartDate;
                    ck.EndDate = objFlightInfoLast.EndDate;
                    ck.StartPoint = objFlightInfoFirst.StartPoint;
                    ck.EndPoint = objFlightInfoLast.EndPoint;
                    ck.FlightNumber = objFlightInfoFirst.FlightNumber;
                    ck.FareClass = objFlightInfoFirst.FareClass;
                    ck.GroupClass = objFlightInfoFirst.GroupClass;
                    ck.FlightValue = objFlightInfoFirst.FlightValue;
                    var listhancheobj = listhanche.FirstOrDefault(z => z.Hangve == ck.FareClass
                        && z.StartPoind == ck.StartPoint && z.EndPoind == ck.EndPoint && Equals(z.Priced, objlist.FareAdt));
                    if (listhancheobj != null)
                        continue;
                    var check = listvalue.FirstOrDefault(z => z.FlightNumber == ck.FlightNumber && z.StartDate == ck.StartDate);
                    if (check != null)
                    {

                    }
                    if (check == null)
                    {

                        if (objlist.Airline == "VN")
                        {
                            getgiacongthem = getgiacongthem +
                                             GetPriceCongThemhangG(listsetting, objlist.Airline, ck.FareClass);
                        }
                        double giacongthemphantramAdt = 0;
                        double giacongthemphantramChd = 0;
                        ck.SessionId = SessionId;
                        ck.FareDataId = objlist.FareDataId;
                        ck.ItineraryType = ItineraryType;
                        ck.Airline = objlist.Airline;
                        ck.Duration = objFlightInfoFirst.Duration;
                        ck.Currency = objlist.Currency;
                        ck.Adt = objlist.Adt;
                        ck.Chd = objlist.Chd;
                        ck.Inf = objlist.Inf;
                        ck.FareAdtNet = objlist.FareAdt;
                        ck.FareAdt = Utility.Ceiling(objlist.FareAdt, 1000);
                        ck.FareChd = Utility.Ceiling(objlist.FareChd, 1000);
                        ck.FareInf = Utility.Ceiling(objlist.FareInf, 1000);
                        ck.FeeAdt = Utility.Ceiling(objlist.FeeAdt + getgiacongthem + giacongthemphantramAdt, 1000);
                        ck.FeeChd = Utility.Ceiling(objlist.FeeChd + getgiacongthem + giacongthemphantramChd, 1000);
                        ck.FeeInf = Utility.Ceiling(objlist.FeeInf, 1000);
                        ck.TaxAdt = Utility.Ceiling(objlist.TaxAdt, 1000);
                        ck.TaxChd = Utility.Ceiling(objlist.TaxChd, 1000);
                        ck.TaxInf = Utility.Ceiling(objlist.TaxInf, 1000);
                        ck.PriceBeforNet = objlist.TotalPrice;
                        ck.PriceBefor = Utility.Ceiling(objlist.TotalPrice, 1000);
                        ck.TotalPrice = Utility.Ceiling(objlist.TotalPrice + (getgiacongthem * objlist.Adt) + (getgiacongthem * objlist.Chd) + (giacongthemphantramAdt * objlist.Adt) + (giacongthemphantramChd * objlist.Chd), 1000);
                        ck.ListFlight = objlist.ListFlight;
                        ck.Promo = objlist.Promo;

                        listvalue.Add(ck);
                    }

                }
            }
            return listvalue;
        }
        public IEnumerable<VDMFareDataInfo> AddLccFlightInfo(ApiClient.Models.FareData[] list, int ItineraryType,
         List<SettingUserInfo> listsetting, int nguoilon, int treem, int embe,
         string SessionId, List<MonthSearch.HanchesohieuInfo> listhanche)
        {
            var listvalue = new List<VDMFareDataInfo>();
            foreach (var objlist in list.OrderBy(z => z.FareAdt))
            {
                var getgiacongthem = GetPriceCongThem(listsetting, objlist.Airline);
                var objFlightInfoFirst = objlist.ListFlight.FirstOrDefault();
                var objFlightInfoLast = objlist.ListFlight.LastOrDefault();
                if (objFlightInfoFirst != null && objFlightInfoLast != null)
                {
                    VDMFareDataInfo ck = new VDMFareDataInfo();
                    ck.FareClass = (objFlightInfoFirst.FareClass ?? "").ToUpper();
                    ck.StartDate = objFlightInfoFirst.StartDate;
                    ck.EndDate = objFlightInfoLast.EndDate;
                    ck.StartPoint = objFlightInfoFirst.StartPoint;
                    ck.EndPoint = objFlightInfoLast.EndPoint;
                    ck.FlightNumber = objFlightInfoFirst.FlightNumber;
                    ck.FareClass = objFlightInfoFirst.FareClass;
                    ck.GroupClass = objFlightInfoFirst.GroupClass;
                    ck.FlightValue = objFlightInfoFirst.FlightValue;
                    var listhancheobj = listhanche.FirstOrDefault(z => z.Hangve == ck.FareClass
                        && z.StartPoind == ck.StartPoint && z.EndPoind == ck.EndPoint && Equals(z.Priced, objlist.FareAdt));
                    if (listhancheobj != null)
                        continue;
                    var check = listvalue.FirstOrDefault(z => z.FlightNumber == ck.FlightNumber && z.StartDate == ck.StartDate);
                    if (check == null)
                    {

                        if (objlist.Airline == "VN")
                        {
                            getgiacongthem = getgiacongthem +
                                             GetPriceCongThemhangG(listsetting, objlist.Airline, ck.FareClass);
                        }
                        double giacongthemphantramAdt = 0;
                        double giacongthemphantramChd = 0;
                        ck.SessionId = SessionId;
                        ck.FareDataId = objlist.FareDataId;
                        ck.ItineraryType = ItineraryType;
                        ck.Airline = objlist.Airline;
                        ck.Duration = objFlightInfoFirst.Duration;
                        ck.Currency = objlist.Currency;
                        ck.Adt = objlist.Adt;
                        ck.Chd = objlist.Chd;
                        ck.Inf = objlist.Inf;
                        ck.FareAdtNet = objlist.FareAdt;
                        ck.FareAdt = Utility.Ceiling(objlist.FareAdt, 1000);
                        ck.FareChd = Utility.Ceiling(objlist.FareChd, 1000);
                        ck.FareInf = Utility.Ceiling(objlist.FareInf, 1000);
                        ck.FeeAdt = Utility.Ceiling(objlist.FeeAdt + getgiacongthem + giacongthemphantramAdt, 1000);
                        ck.FeeChd = Utility.Ceiling(objlist.FeeChd + getgiacongthem + giacongthemphantramChd, 1000);
                        ck.FeeInf = Utility.Ceiling(objlist.FeeInf, 1000);
                        ck.TaxAdt = Utility.Ceiling(objlist.TaxAdt, 1000);
                        ck.TaxChd = Utility.Ceiling(objlist.TaxChd, 1000);
                        ck.TaxInf = Utility.Ceiling(objlist.TaxInf, 1000);
                        ck.PriceBeforNet = objlist.TotalPrice;
                        ck.PriceBefor = Utility.Ceiling(objlist.TotalPrice, 1000);
                        ck.TotalPrice = Utility.Ceiling(objlist.TotalPrice + (getgiacongthem * objlist.Adt) + (getgiacongthem * objlist.Chd) + (giacongthemphantramAdt * objlist.Adt) + (giacongthemphantramChd * objlist.Chd), 1000);
                        ck.ListFlight = objlist.ListFlight;
                        ck.Promo = objlist.Promo;

                        listvalue.Add(ck);
                    }

                }
            }
            return listvalue;
        }
        public List<VDMFareDataInfo> AddFareDataInfo(List<SettingUserInfo> listsetting, ApiClient.Models.FareData[] list, ref List<AirlineInfo> listairline, ref List<int> ListStopNum, DateTime? DepartureDate, DateTime? ReturnDate, string SessionId, string FromCity, string ToCity, int? Itinerary)
        {
            List<VDMFareDataInfo> listrt = new List<VDMFareDataInfo>();
            foreach (var fareData in list)
            {

                double giacongthemchieudi = 0;
                double giacongthemchieuve = 0;
                string code = "";
                int StopNum = 0;
                foreach (var fareDataInfo in fareData.ListFlight)
                {

                    code = fareDataInfo.Airline;
                    StopNum = fareDataInfo.StopNum;
                    break;
                }
                foreach (var fareDataInfo in fareData.ListFlight.Where(z => z.StartPoint == FromCity))
                {
                    var giamtam = GetPriceCongThemQuocTe(listsetting, fareDataInfo.Airline);
                    if (giamtam > giacongthemchieudi)
                        giacongthemchieudi = giamtam;
                }
                if (Itinerary > 1)
                {
                    foreach (var fareDataInfo in fareData.ListFlight.Where(z => z.StartPoint == ToCity))
                    {
                        var giamtam = GetPriceCongThemQuocTe(listsetting, fareDataInfo.Airline);
                        if (giamtam > giacongthemchieuve)
                            giacongthemchieuve = giamtam;
                    }
                }
                var giacongthem = giacongthemchieudi + giacongthemchieuve;
                double giacongthemphantramAdt = 0;
                double giacongthemphantramChd = 0;
                if (!ListStopNum.Contains(StopNum))
                {
                    ListStopNum.Add(StopNum);
                }
                var objFlightInfoFirst = fareData.ListFlight.FirstOrDefault();
                var objFlightInfoLast = fareData.ListFlight.LastOrDefault();
                if (objFlightInfoFirst != null && objFlightInfoLast != null)
                {
                    VDMFareDataInfo ck = new VDMFareDataInfo();
                    ck.SessionId = SessionId;
                    ck.FareClass = (objFlightInfoFirst.FareClass ?? "").ToUpper();
                    ck.StartDate = objFlightInfoFirst.StartDate;
                    ck.EndDate = objFlightInfoFirst.EndDate;
                    ck.StartPoint = objFlightInfoFirst.StartPoint;
                    ck.EndPoint = objFlightInfoFirst.EndPoint;
                    ck.FlightNumber = objFlightInfoFirst.FlightNumber;
                    ck.FareClass = objFlightInfoFirst.FareClass;
                    ck.GroupClass = objFlightInfoFirst.GroupClass;
                    ck.Airline = code;
                    ck.StopNum = StopNum;
                    ck.Duration = fareData.ListFlight.Sum(z => z.Duration);
                    ck.FareDataId = fareData.FareDataId;
                    ck.Itinerary = fareData.Itinerary;
                    ck.LastTkDt = fareData.LastTkDt;
                    ck.Currency = fareData.Currency;
                    ck.HasChangedClass = fareData.HasChangedClass;
                    ck.Adt = fareData.Adt;
                    ck.Chd = fareData.Chd;
                    ck.Inf = fareData.Inf;
                    ck.FeeAdt = Utility.Ceiling(fareData.FeeAdt + (giacongthem) + giacongthemphantramAdt, 1000);
                    ck.FeeChd = Utility.Ceiling(fareData.FeeChd + (giacongthem) + giacongthemphantramChd, 1000);
                    ck.FeeInf = Utility.Ceiling(fareData.FeeInf, 1000);
                    //FeeInfant = Utility.Ceiling(fareData.FeeInfant + (giacongthem * fareData.Infant), 1000),
                    ck.FareAdt = Utility.Ceiling(fareData.FareAdt, 1000) + Utility.Ceiling(fareData.FeeAdt + (giacongthem) + giacongthemphantramAdt, 1000) + Utility.Ceiling(fareData.TaxAdt, 1000);
                    ck.FareChd = Utility.Ceiling(fareData.FareChd, 1000) + Utility.Ceiling(fareData.FeeChd + (giacongthem) + giacongthemphantramChd, 1000) + Utility.Ceiling(fareData.TaxChd, 1000);
                    ck.FareInf = Utility.Ceiling(fareData.FareInf, 1000) + Utility.Ceiling(fareData.FeeInf, 1000) + Utility.Ceiling(fareData.TaxInf, 1000);
                    ck.PriceBefor = Utility.Ceiling(fareData.TotalPrice, 1000);
                    ck.PriceBeforNet = fareData.TotalPrice;
                    //   ck.TotalPrice = Utility.Ceiling(fareData.TotalPrice + (giacongthem * fareData.Adt) + (giacongthem * fareData.Chd) + (giacongthem * fareData.Inf) + (giacongthemphantramAdt * fareData.Adt) + (giacongthemphantramChd * fareData.Chd), 1000);
                    ck.TotalPrice = Utility.Ceiling(fareData.TotalPrice + (giacongthem * fareData.Adt) + (giacongthem * fareData.Chd) + (giacongthemphantramAdt * fareData.Adt) + (giacongthemphantramChd * fareData.Chd), 1000);

                    ck.TaxAdt = Utility.Ceiling(fareData.TaxAdt, 1000);
                    ck.TaxChd = Utility.Ceiling(fareData.TaxChd, 1000);
                    ck.TaxInf = Utility.Ceiling(fareData.TaxInf, 1000);
                    ck.ListFlight = fareData.ListFlight;
                    ck.System = fareData.System;
                    listrt.Add(ck);
                    if (listairline.Count(z => z.Code == code) <= 0)
                    {
                        var Getairlineinfos = Getairlineinfo(code);
                        if (Getairlineinfos != null)
                        {
                            Getairlineinfos.GiareNhat = ck.FareAdt;
                            listairline.Add(Getairlineinfos);
                        }
                    }
                    else
                    {
                        var obj = listairline.FirstOrDefault(z => z.Code == code);
                        if (obj != null)
                        {
                            if (obj.GiareNhat > ck.FareAdt)
                            {
                                obj.GiareNhat = ck.FareAdt;
                            }
                        }
                    }
                }
            }
            return listrt;
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
                string DepartureFlightAirline = param.DepartureFlight.Airline;
                string ReturnFlightAirline = param.ReturnFlight != null ? param.ReturnFlight.Airline : "";
                if (DepartureFlightAirline == ReturnFlightAirline || string.IsNullOrEmpty(ReturnFlightAirline))
                {
                    Gettaikhoan(param.DepartureFlight.Airline, param.DepartureFlight.StartPoint, param.DepartureFlight.EndPoint, ref usertem, ref passtem);
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
                    listbooking.AddRange(book.ListBooking);
                }
                else if (param.ReturnFlight != null)
                {

                    ///  book chiều đi
                    Gettaikhoan(param.DepartureFlight.Airline, param.DepartureFlight.StartPoint, param.DepartureFlight.EndPoint, ref usertem, ref passtem);
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
                    listbooking.AddRange(bookvaluechieudi.ListBooking);
                    /// Book chiều về 
                    Gettaikhoan(param.ReturnFlight.Airline, param.ReturnFlight.StartPoint, param.ReturnFlight.EndPoint, ref usertem, ref passtem);
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
                    listbooking.AddRange(bookvaluechieuve.ListBooking);
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
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "E" || param.DepartureFlight.FareClass == "P" || param.DepartureFlight.FareClass == "A" || param.DepartureFlight.FareClass == "T" || param.DepartureFlight.FareClass == "G" || param.ReturnFlight.FareClass == "T" || param.ReturnFlight.FareClass == "A" || param.ReturnFlight.FareClass == "E" || param.ReturnFlight.FareClass == "P" || param.ReturnFlight.FareClass == "G"))
                                {
                                    pnr.ExpiryDate = DateTime.Now.AddHours(4);
                                }
                            }
                            else
                            {
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "E" || param.DepartureFlight.FareClass == "P" || param.DepartureFlight.FareClass == "A" || param.DepartureFlight.FareClass == "T" || param.DepartureFlight.FareClass == "G"))
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
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "E" || param.DepartureFlight.FareClass == "P" || param.DepartureFlight.FareClass == "A" || param.DepartureFlight.FareClass == "T" || param.DepartureFlight.FareClass == "G" || param.ReturnFlight.FareClass == "T" || param.ReturnFlight.FareClass == "A" || param.ReturnFlight.FareClass == "E" || param.ReturnFlight.FareClass == "P" || param.ReturnFlight.FareClass == "G"))
                                {
                                    pnr.ExpiryDate = DateTime.Now.AddHours(4);
                                }
                            }
                            else
                            {
                                if (pnr.Airline == "VN" && (param.DepartureFlight.FareClass == "E" || param.DepartureFlight.FareClass == "P" || param.DepartureFlight.FareClass == "A" || param.DepartureFlight.FareClass == "T" || param.DepartureFlight.FareClass == "G"))
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
                listpnr.Add(new Booking { BookingCode = code, ErrorMessage = ex.Message, ExpiryDate = Expireddate });
                rads = listpnr;
            }
            if (code == "FAIL")
                code = "";
            return code != null ? code.Trim() : "";
        }
        public string bookInternational(SeachParam param, ref DateTime Expireddate, ref Booking pnr, List<SettingUserInfo> listsetting, VDMUser userInfor, FormCollection forminput)
        {

            string code = "";
            var listbooking = new List<Booking>();
            try
            {
                if (param.FareDataId.HasValue)
                {
                    var DepartureFlight = param.FareDataInfo.ListFlight.First(z => z.FlightId == param.DepartureFlightId);
                    if (DepartureFlight != null)
                    {
                        var lstFareDataInfo = new List<ApiClient.Models.FareDataInfo>();
                        FareDataInfo fareData = new FareDataInfo
                        {
                            Session = param.FareDataInfo.SessionId,
                            FareDataId = param.FareDataId ?? 0,
                            AutoIssue = false
                        };
                        List<ApiClient.Models.FlightInfo> listFlight = new List<ApiClient.Models.FlightInfo>();

                        listFlight.Add(new ApiClient.Models.FlightInfo { FlightValue = DepartureFlight.FlightValue });
                        if (param.FlightSearchResult.Itinerary > 1)
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
                Email = (!string.IsNullOrEmpty(email) ? email : param.BkBooking.Email),
                Address = !string.IsNullOrEmpty(param.BkBooking.Address) ? param.BkBooking.Address : "264E Lê Văn Sỹ P14 Q3 HCM",
            };
            return contact;
        }
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
                    item.SessionID = param.FlightSearchResult.Session;
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
                    item.SessionID = param.FlightSearchResult.Session;
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
                    if (param.DepartureFlight.FareClass != "BAMBOOECO")
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

                    dbItem.BaggageDepartValue = (item.TypeID == Constant.TypePassenger.EmBe ? "10" : "23");
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
                    if (param.ReturnFlight.FareClass != "BAMBOOECO")
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
                    dbItem.BaggageReturnValue = (item.TypeID == Constant.TypePassenger.EmBe ? "10" : "23");
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
        public SeachParam LoadBooktrongnuoc(string CodeReturn, string CodeDepart, VDMUser userinfor)
        {
            var param = new SeachParam();
            param = (SeachParam)HttpContext.Current.Session["SeachRd"];
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
                SessionID = param.DepartureFlight.SessionAll;
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

            var lstFareDataInfo = new List<ApiClient.Models.FareDataInfo>();
            ApiClient.Models.FareDataInfo fareData = new ApiClient.Models.FareDataInfo
            {
                Session = DepartureSessionId,
                FareDataId = DepartureFareDataId,
                AutoIssue = false,
                ListFlight = new ApiClient.Models.FlightInfo[]
                {
                            new ApiClient.Models.FlightInfo { FlightValue = DepartureSelectValue }
                }
            };
            lstFareDataInfo.Add(fareData);
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
                lstFareDataInfo.Add(fareDataReturn);
            }
            BaggageRequest request = new BaggageRequest
            {
                HeaderUser = HeaderUser,
                HeaderPass = HeaderPassword,
                AgentAccount = Username,
                AgentPassword = Password,
                LanguageCode = "vi",
                ListFareData = lstFareDataInfo.ToArray()
            };
            param.BaggageInfo = apiClient.Getbaggage(request);
            param.BkBooking = new BK_Booking { SessionID = SessionID };
            if (param.DepartureFlight != null)
            {
                if (userinfor != null)
                {
                    param.BkBooking.Name = userinfor.DisplayName;
                    param.BkBooking.Phone = userinfor.PhoneNumber;
                    param.BkBooking.Email = userinfor.Email;
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

            var lstFareDataInfo = new List<ApiClient.Models.FareDataInfo>();

            var DepartureFlight = param.FareDataInfo.ListFlight.FirstOrDefault(z => z.FlightId == param.DepartureFlightId);
            if (DepartureFlight != null)
            {
                List<ApiClient.Models.FlightInfo> lstFlightInfo = new List<ApiClient.Models.FlightInfo>();
                ApiClient.Models.FareDataInfo fareData = new ApiClient.Models.FareDataInfo
                {
                    Session = param.FlightSearchResult.Session,
                    FareDataId = param.FareDataId ?? 0,
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
                param.BaggageInfo = apiClient.Getbaggage(request);
            }
            param.BkBooking = new BK_Booking { SessionID = param.FlightSearchResult.Session };
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
        #region send mail
        public void sendsms(int bkingid, VDMUser userinfor)
        {
            //if (userinfor == null)
            //{
            //    var BkBooking = new BK_BookingInfo();
            //    var bookingdao = new BK_BookingDao();
            //    BkBooking = bookingdao.GetInfo(bkingid);
            //    if (BkBooking != null)
            //    {
            //        var userdao = new AspNetUsersBiz();
            //        var obj = userdao.GetInfoByEmail(BkBooking.Email);
            //        if (obj == null)
            //        {
            //            string sdt =
            //                BkBooking.Phone.Trim()
            //                    .Replace(" ", "")
            //                    .Replace("-", "")
            //                    .Replace(",", "")
            //                    .Replace(".", "")
            //                    .Trim();
            //            var SMSDAO = new SMSDAO();
            //            var check = SMSDAO.checkguitin(sdt);
            //            if (check)
            //            {
            //                string dtout = "";
            //                if (!string.IsNullOrEmpty(sdt))
            //                {
            //                    var kytudau = sdt.Substring(0, 1);
            //                    if (kytudau == "0")
            //                        dtout = "84" + sdt.Remove(0, 1);
            //                    else if (kytudau != "8")
            //                    {
            //                        dtout = "84" + sdt;
            //                    }
            //                    else
            //                    {
            //                        dtout = sdt;
            //                    }
            //                }
            //                string noidung = "";
            //                noidung += "XAC NHAN DAT VE&#10;";
            //                noidung += ("Code: " + BkBooking.BookCode + "&#10;");
            //                var dao = new BK_TicketDao();
            //                var lstve = dao.GetListByBooking(BkBooking.ID);
            //                var objdi = lstve.FirstOrDefault(z => z.TypeID == Constant.Typeve.LuotDi);
            //                if (objdi != null)
            //                {
            //                    var PassengerDao = new BK_PassengerDao();
            //                    var PassengerParam = new BK_PassengerParam() { ticketID = objdi.ID };
            //                    PassengerDao.Getbyticket(PassengerParam);
            //                    foreach (var item in PassengerParam.BK_PassengerInfos)
            //                    {
            //                        noidung += (Utility.ConvertToUnsign(item.FirstName + " " + item.Name).ToUpper() +
            //                                    " (" +
            //                                    ((!string.IsNullOrEmpty(item.BaggageDepartValue)
            //                                        ? item.BaggageDepartValue
            //                                        : "0") + "kg") +
            //                                    ((!string.IsNullOrEmpty(item.BaggageReturnValue)
            //                                        ? " - " + item.BaggageReturnValue
            //                                        : " - 0") + "kg") + ")&#10;");
            //                    }
            //                    var Departure = objdi;
            //                    noidung += (Departure.FlightNo + " " + Departure.StartDate.Value.ToString("dd/MM") + " " +
            //                                Departure.StartDate.Value.ToString("HH:mm") + " " + Departure.FromCity + " " +
            //                                Departure.ToCity + "&#10;");
            //                }
            //                var objve = lstve.FirstOrDefault(z => z.TypeID == Constant.Typeve.LuotVe);
            //                if (objve != null)
            //                {
            //                    var Departure = objve;
            //                    noidung += (Departure.FlightNo + " " + Departure.StartDate.Value.ToString("dd/MM") + " " +
            //                                Departure.StartDate.Value.ToString("HH:mm") + " " + Departure.FromCity + " " +
            //                                Departure.ToCity + "&#10;");
            //                }

            //                noidung += ("Xtay:7Kg");
            //                noidung += "&#10;Vui long den som truoc 90p";
            //                noidung += ("&#10;Gia:" + (BkBooking.TotalPrice.HasValue
            //                    ? BkBooking.TotalPrice.Value.ToString("n0")
            //                    : "0"));
            //                noidung += "&#10;Tinh trang: CHO THANH TOAN";
            //                noidung += ("&#10;Vui long thanh toan truoc:" +
            //                            (BkBooking.Expireddate.HasValue
            //                                ? BkBooking.Expireddate.Value.ToString("dd/MM/yyyy HH:mm")
            //                                : ""));
            //                noidung += "&#10;Lhe: 19007287";
            //                noidung += "&#10;Tks for fly with us.&#10;http://thegioivere.net";
            //                SendSMS.MainSend(noidung, dtout,
            //                    DateTime.Now.ToString("dd/MM/yy HH:mm:ss.fff")
            //                        .Replace(".", "")
            //                        .Replace("/", "")
            //                        .Replace(":", "") + BkBooking.ID, BkBooking.BookCode, BkBooking.ID);
            //            }
            //        }
            //    }
            //}

        }
        public void sendsmsQT(int bkingid)
        {
            //if (Session["SessionUserinfo"] == null)
            //{
            //    var BkBooking = new BK_BookingInfo();
            //    var bookingdao = new BK_BookingDao();
            //    BkBooking = bookingdao.GetInfo(bkingid);
            //    if (BkBooking != null)
            //    {
            //        var userdao = new UserDao();
            //        var obj = userdao.Getbyemail(BkBooking.Email);
            //        if (obj == null)
            //        {
            //            string sdt =
            //                BkBooking.Phone.Trim()
            //                    .Replace(" ", "")
            //                    .Replace("-", "")
            //                    .Replace(",", "")
            //                    .Replace(".", "")
            //                    .Trim();
            //            var SMSDAO = new SMSDAO();
            //            var check = SMSDAO.checkguitin(sdt);
            //            if (check)
            //            {
            //                string dtout = "";
            //                if (!string.IsNullOrEmpty(sdt))
            //                {
            //                    var kytudau = sdt.Substring(0, 1);
            //                    if (kytudau == "0")
            //                        dtout = "84" + sdt.Remove(0, 1);
            //                    else if (kytudau != "8")
            //                    {
            //                        dtout = "84" + sdt;
            //                    }
            //                    else
            //                    {
            //                        dtout = sdt;
            //                    }
            //                }
            //                string noidung = "";
            //                noidung += "XAC NHAN DAT VE&#10;";
            //                noidung += ("Code: " + BkBooking.BookCode + "&#10;");
            //                var dao = new BK_TicketDao();
            //                var lstve = dao.GetListByBooking(BkBooking.ID);
            //                var objdi = lstve.FirstOrDefault(z => z.TypeID == Constant.Typeve.LuotDi);
            //                if (objdi != null)
            //                {
            //                    var PassengerDao = new BK_PassengerDao();
            //                    var PassengerParam = new BK_PassengerParam() { ticketID = objdi.ID };
            //                    PassengerDao.Getbyticket(PassengerParam);
            //                    foreach (var item in PassengerParam.BK_PassengerInfos)
            //                    {
            //                        noidung += (Utility.ConvertToUnsign(item.FirstName + " " + item.Name).ToUpper() + "&#10;");
            //                    }
            //                    var Departure = objdi;
            //                    noidung += (Departure.FlightNo + " " + Departure.StartDate.Value.ToString("dd/MM") + " " +
            //                                Departure.StartDate.Value.ToString("HH:mm") + " " + Departure.FromCity + " " +
            //                                Departure.ToCity + "&#10;");
            //                }
            //                var objve = lstve.FirstOrDefault(z => z.TypeID == Constant.Typeve.LuotVe);
            //                if (objve != null)
            //                {
            //                    var Departure = objve;
            //                    noidung += (Departure.FlightNo + " " + Departure.StartDate.Value.ToString("dd/MM") + " " +
            //                                Departure.StartDate.Value.ToString("HH:mm") + " " + Departure.FromCity + " " +
            //                                Departure.ToCity + "&#10;");
            //                }

            //                noidung += ("Xtay:7Kg");
            //                noidung += "&#10;Vui long den som truoc 90p";
            //                noidung += ("&#10;Gia:" + (BkBooking.TotalPrice.HasValue
            //                    ? BkBooking.TotalPrice.Value.ToString("n0")
            //                    : "0"));
            //                noidung += "&#10;Tinh trang: CHO THANH TOAN";
            //                noidung += ("&#10;Vui long thanh toan truoc:" +
            //                            (BkBooking.Expireddate.HasValue
            //                                ? BkBooking.Expireddate.Value.ToString("dd/MM/yyyy HH:mm")
            //                                : ""));
            //                noidung += "&#10;Lhe: 19007287";
            //                noidung += "&#10;Tks for fly with us.&#10;http://thegioivere.net";
            //                SendSMS.MainSend(noidung, dtout,
            //                    DateTime.Now.ToString("dd/MM/yy HH:mm:ss.fff")
            //                        .Replace(".", "")
            //                        .Replace("/", "")
            //                        .Replace(":", "") + BkBooking.ID, BkBooking.BookCode, BkBooking.ID);
            //            }
            //        }
            //    }
            //}

        }
        public void sendmail(string Bookcode, List<SettingUserInfo> listseting, int? iduser)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(Bookcode);
            if (inforbk != null)
            {

                //string urlsource = HttpContext.Current.Server.MapPath("~/TeampleateVe/TeampletaveSendmail.html");
                var tieude = "Xác nhận đặt vé | mã đặt chỗ " + inforbk.BookCode + " | " + inforbk.Name + " | " + inforbk.FromCity + " " + inforbk.ToCity;
                var list = new List<MailAddress> { new MailAddress(inforbk.Email) };

                string html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.VeXacNhanHanhTrinh, iduser);
                string filename = "";
                filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                filename += "_" + inforbk.BookCode;
                filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                filename += "_" + inforbk.FromCity + inforbk.ToCity;
                filename += ".pdf";
                var export = new ExportPDF();
                string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, Constant.TemPleateHTMLID.PDF, iduser);
                var file = export.ExportPdfReturnStream(htmlPDF);
                var att = new Attachment(file, filename, "application/pdf");

                var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");

                MailUtily.sendmail(listseting, tieude, html
                    , list,
                    att, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
            }
        }
        public void sendmailFail(int Bookid, List<SettingUserInfo> listseting, VDMUser userInfo, int? IdUser)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(Bookid);
            if (inforbk != null)
            {
                var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");
                // string urlsource = HttpContext.Current.Server.MapPath("~/TeampleateVe/TeampletaveSendmail.html");
                var titel = listseting.FirstOrDefault(z => z.Name == "PRT_COMPANY_NAME");
                string by = "";
                if (userInfo != null)
                {
                    by = userInfo.DisplayName;
                    by = (titel != null ? titel.Value + " - " : "");
                }
                else
                    by = (titel != null ? titel.Value + " - " : "");
                var ticketdao = new BK_TicketDao();
                string hang = "";
                var lichticket = ticketdao.GetlistbybookingId(inforbk.ID);
                var hangdi = "";
                var hangve = "";
                foreach (var item in lichticket)
                {
                    if (item.TypeID == Constant.Typeve.LuotDi)
                    {
                        hangdi = item.Code;
                    }
                    else if (item.TypeID == Constant.Typeve.LuotVe)
                    {
                        hangve = item.Code;
                    }
                }
                hang = hangdi;
                if (lichticket.Count > 1)
                {
                    if (hangdi != hangve)
                    {
                        hang = hangdi + "-" + hangve;
                    }
                    else
                        hang = hangdi;
                }
                var tieude = "Đặt vé thất bại |" + inforbk.rCode + " |  " + hang + " |  " + inforbk.Name + " | " + inforbk.Phone + " | " + inforbk.Email + " | " + inforbk.FromCity + " " + inforbk.ToCity + " | by " + by.Replace("-", "");
                string html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.VeXacNhanHanhTrinh, IdUser);

                var list = new List<MailAddress> { new MailAddress(fromAddress), new MailAddress(inforbk.Email) };
                string filename = "";
                filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                filename += "_" + inforbk.BookCode;
                filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                filename += "_" + inforbk.FromCity + inforbk.ToCity;
                filename += ".pdf";
                var export = new ExportPDF();
                string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, Constant.TemPleateHTMLID.PDF, IdUser);
                var file = export.ExportPdfReturnStream(htmlPDF);

                var att = new Attachment(file, filename, "application/pdf");
                MailUtily.sendmail(listseting, tieude, html
                   , list,
                   att, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
            }
        }
        #endregion
        private void Gettaikhoan(string hang, string startpoind, string endpoind, ref string username, ref string pass)
        {
            using (var service = new MonthSearch.MonthSearchSoapClient())
            {
                var obj = service.GetlistCauHinhTaiKhoan(hang, startpoind, endpoind, "admin", "admin@123");
                if (obj != null)
                {
                    username = obj.Username;
                    pass = obj.Pass;
                }
            }
        }
        #region Vé tháng
        public VeThangareDataInfo AddFlightFareInfoMonth(List<SettingUserInfo> listsetting, MonthSearch.FlightFareInfo list)
        {
            var listvalue = new VeThangareDataInfo();
            var objlist = list;
            var getgiacongthem = GetPriceCongThem(listsetting, objlist.AirlineCode);
            var item = new VeThangareDataInfo
            {
                SessionAll = objlist.SessionAll,
                DebugID = objlist.DebugID,
                ItineraryType = objlist.ItineraryType.Value,
                Airline = objlist.AirlineCode,
                StartDate = objlist.StartDate.Value,
                EndDate = objlist.EndDate.Value,
                StartPoint = objlist.StartPoint,
                EndPoint = objlist.EndPoint,
                Stops = objlist.Stops.Value,
                Duration = objlist.Duration.Value,
                SelectValue = objlist.SelectValue,
                Currency = objlist.Currency,
                Chd = objlist.Child.Value,
                Inf = objlist.Infant.Value,
                FareAdt = Utility.Ceiling(objlist.PriceAdult.Value, 1000),
                FareChd = Utility.Ceiling(objlist.PriceChild.Value, 1000),
                FareInf = Utility.Ceiling(objlist.PriceInfant.Value, 1000),
                FeeAdt = Utility.Ceiling(objlist.FeeAdult.Value + getgiacongthem, 1000),
                FeeChd = Utility.Ceiling(objlist.FeeChild.Value + getgiacongthem, 1000),
                FeeInf = Utility.Ceiling(objlist.FeeInfant.Value + getgiacongthem, 1000),
                TaxAdt = Utility.Ceiling(objlist.TaxAdult.Value, 1000),
                TaxChd = Utility.Ceiling(objlist.TaxChild.Value, 1000),
                TaxInf = Utility.Ceiling(objlist.TaxInfant.Value, 1000),
                PriceBefor = Utility.Ceiling(objlist.TotalPrice.Value, 1000),
                TotalPrice =
                    Utility.Ceiling(
                        objlist.TotalPrice.Value + (getgiacongthem * objlist.Adult.Value) + (getgiacongthem * objlist.Child.Value) +
                        (getgiacongthem * objlist.Infant.Value), 1000),
                GroupClass = objlist.GroupClass,
                FareClass = objlist.FareClass,
                FlightNumber = objlist.FltNumb,
                ListSegment = objlist.AvailFlight.Select(tblAvailFlight => new VeThangSegment()
                {
                    StartTime = tblAvailFlight.StartDate.Value,
                    EndTime = tblAvailFlight.EndDate.Value,
                    StartPoint = tblAvailFlight.StartPoint,
                    EndPoint = tblAvailFlight.EndPoint,
                    FlightNumber = tblAvailFlight.FlightNumber,
                    Plane = tblAvailFlight.Plane,
                    Duration = tblAvailFlight.Duration.Value,
                    Class = tblAvailFlight.Class,
                }).ToList(),
            };
            return item;
        }
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
                            param.ReturnFlightsVeThang.Add(AddFlightFareInfoMonth(listsetting, flightData));
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
                        param.DepartureFlightsVeThang.Add(AddFlightFareInfoMonth(listsetting, flightData));
                    }
                }
            }
            return param;
        }
        public static List<MonthSearch.FlightFareInfo> Getlistbythang(DateTime dtInput, string start, string end)
        {

            string Uservethang = System.Configuration.ConfigurationSettings.AppSettings.Get("Uservethang");
            if (!string.IsNullOrEmpty(Uservethang))
                Uservethang = Utility.Decrypt(Uservethang, true);
            string Pasvethang = System.Configuration.ConfigurationSettings.AppSettings.Get("Pasvethang");
            if (!string.IsNullOrEmpty(Pasvethang))
                Pasvethang = Utility.Decrypt(Pasvethang, true);
            using (var service = new MonthSearch.MonthSearchSoapClient())
            {
                var objlist = new List<MonthSearch.FlightFareInfo>();
                var dateaddtru3 = dtInput.AddDays(-3);
                var dateaddcong3 = dtInput.AddDays(3);
                if(dateaddtru3.Month != dtInput.Month)
                {
                    objlist.AddRange(service.GetMonthlyTicket(dateaddtru3.Month, dateaddtru3.Year, start, end, 1, 1, 1, Uservethang, Pasvethang));
                }
                else if (dateaddcong3.Month != dtInput.Month)
                {
                    objlist.AddRange(service.GetMonthlyTicket(dateaddcong3.Month, dateaddcong3.Year, start, end, 1, 1, 1, Uservethang, Pasvethang));
                }
                objlist.AddRange(service.GetMonthlyTicket(dtInput.Month, dtInput.Year, start, end, 1, 1, 1, Uservethang, Pasvethang));
                objlist = objlist.Where(z => z.StartDate.Value.Date >= dateaddtru3.AddDays(-1) && z.StartDate.Value.Date <= dateaddcong3.AddDays(1)).ToList();
                foreach (var item in objlist)
                {
                    var VDMUser = (VDMUser)HttpContext.Current.Session["CurrentUser"];
                    var listsetting = CommonUI.GetSettingByDomainAndUser(SiteMuti.Getsitename(), VDMUser);
                    double getgiacongthem = 0;
                    switch (item.AirlineCode.Trim().ToUpper())
                    {
                        case "JQ":
                            {
                                var obj = listsetting.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeJetstar".Trim().ToUpper());
                                if (obj != null)
                                {
                                    var dou = Utility.ConvertTodouble(obj.Value);
                                    getgiacongthem = dou.HasValue ? dou.Value : 0;
                                }
                                break;
                            }
                        case "VJ":
                            {
                                var obj = listsetting.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietJet".Trim().ToUpper());
                                if (obj != null)
                                {
                                    var dou = Utility.ConvertTodouble(obj.Value);
                                    getgiacongthem = dou.HasValue ? dou.Value : 0;
                                }
                                break;
                            }
                        case "VN":
                            {
                                var obj = listsetting.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietnamAirline".Trim().ToUpper());
                                if (obj != null)
                                {
                                    var dou = Utility.ConvertTodouble(obj.Value);
                                    getgiacongthem = dou.HasValue ? dou.Value : 0;
                                }
                                break;
                            }
                        case "QH":
                            {
                                var obj = listsetting.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeBamboo".Trim().ToUpper());
                                if (obj != null)
                                {
                                    var dou = Utility.ConvertTodouble(obj.Value);
                                    getgiacongthem = dou.HasValue ? dou.Value : 0;
                                }
                                break;
                            }
                    }
                    item.PriceAdult = Utility.Ceiling(item.PriceAdult ?? 0, 1000);
                    item.FeeAdult = Utility.Ceiling((item.FeeAdult ?? 0) + getgiacongthem, 1000);
                    item.TaxAdult = Utility.Ceiling(item.TaxAdult ?? 0, 1000);
                }
                return objlist.ToList();
            }
        }

        #endregion
    }
}