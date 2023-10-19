using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;
using ApiClient.Models;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VDMMutiline.APIExport.Common;

namespace VDMMutiline.APIExport
{
    public class SeachAndBookCommon : BaseSeach
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        CommonTicket common = new CommonTicket();
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
            var JQDATA = SeachByAirlineJQ(Itinerary, DepartureAirportCode, DestinationAirportCode, startdate, enddate, request.Adt, request.Chd, request.Inf);
            await Task.WhenAll(VNData, JQDATA, VJDATA, QHDATA);
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
                    if (Itinerary > 1)
                    {
                        listCheckBaggageVn.AddRange(apiClient.GetBaggageVn(DestinationAirportCode, DepartureAirportCode));
                    }
                    if (VNData.Result.ListFareData == null)
                        VNData.Result.ListFareData = new List<FareData>().ToArray();
                    var lstvalue = await common.AddLccFlightInfoAsync(VNData.Result.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, VNData.Result.Session, listhanche, listCheckBaggageVn);
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
                            Infant ?? 0, VJDATA.Result.Session, listhanche, new List<BaggageVn>());
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
                        var srlisthanche = await service.GetlisthancheAsync("JQ");
                        if (srlisthanche != null)
                            if (srlisthanche.Body != null)
                                if (srlisthanche.Body.GetlisthancheResult != null)
                                    listhanche.AddRange(srlisthanche.Body.GetlisthancheResult.ToList());
                    }
                    if (QHDATA.Result.ListFareData == null)
                        QHDATA.Result.ListFareData = new List<FareData>().ToArray();
                    var lstvalue = await common.AddLccFlightInfoAsync(QHDATA.Result.ListFareData, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, QHDATA.Result.Session, listhanche, new List<BaggageVn>());
                    lstFlightConvert.AddRange(lstvalue);
                }
            }
            if (JQDATA != null)
            {
                if (JQDATA.Result != null)
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
                    var lstvalue = await common.AddLccFlightInfoJQAsync(JQDATA.Result.ListDepartFlight, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, JQDATA.Result.Session, listhanche);

                    lstFlightConvert.AddRange(lstvalue);
                    if (JQDATA.Result.ListFareData == null)
                        JQDATA.Result.ListFareData = new List<VDMFareData>().ToArray();
                    if (JQDATA.Result.ListReturnFlight != null)
                    {
                        if (JQDATA.Result.ListReturnFlight.Count() > 0)
                        {
                            lstvalue = await common.AddLccFlightInfoJQAsync(JQDATA.Result.ListReturnFlight, Itinerary ?? 0, listsetting, Adult ?? 0, Children ?? 0,
                            Infant ?? 0, JQDATA.Result.Session, listhanche);

                            lstFlightConvert.AddRange(lstvalue);
                        }
                    }
                }
            }

            if (lstFlightConvert.Count() > 0)
            {
                // param.FlightSearchResult = data;
                param.DepartureFlights = new List<VDMFareDataInfo>();
                param.ReturnFlights = new List<VDMFareDataInfo>();
                var ListAirline = new List<AirlineInfo>();
                var ListAirlineCodeseach = lstFlightConvert.GroupBy(z => z.Airline).Select(group => new { Airline = group.Key }).ToList();
                //foreach (var item in ListAirlineCodeseach)
                //{
                //    if (ListAirline.Count(z => z.Code == item.Airline) <= 0)
                //    {
                //        var Getairlineinfos = common.Getairlineinfo(item.Airline);
                //        if (Getairlineinfos != null)
                //        {
                //            ListAirline.Add(Getairlineinfos);
                //        }
                //    }
                //}
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
            //HttpContext.Current.Session[Constant.SessionSeach] = param;
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
            var data = await apiClient.JetstarSearchTicketDomtic(paramseach);
            return data;
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
            var dataJQ = apiClient.JetstarSearchTicketInternational(paramseach);
            await Task.WhenAll(data, dataJQ);

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


            if (dataJQ != null)
            {
                if (dataJQ.Result != null)
                {
                    if (dataJQ.Result.ListFareData != null)
                        param.LstFareData.AddRange(common.AddFareDataInfoJQ(listsetting, dataJQ.Result.ListFareData, ref listAirline, ref ListStopNum, startdate, enddate, dataJQ.Result.Session, DepartureAirportCode, DestinationAirportCode, Itinerary ?? 0));
                }
            }
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