using ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Ultilities;
using WebProject.Ultilities;

namespace VDMMutiline.APIExport.Common
{
    public class CommonTicket
    {


        public double GetPriceCongThem(List<SettingUserInfo> listinput, string codeAl, string Hangve)
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
                        if (Hangve.ToUpper().Trim() == "PROMO")
                        {
                            var objPROMO = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietJet_Promo".Trim().ToUpper());
                            if (objPROMO != null)
                            {
                                var dou = Utility.ConvertTodouble(objPROMO.Value);
                                value += dou.HasValue ? dou.Value : 0;

                            }
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
                        value = 0;
                        var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeBamboo".Trim().ToUpper());
                        if (obj != null)
                        {
                            var dou = Utility.ConvertTodouble(obj.Value);
                            value = dou.HasValue ? dou.Value : 0;

                        }
                        if (Hangve.ToUpper().Trim() == "BAMBOOECO")
                        {
                            var objBAMBOOECO = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeBamboo_Eco".Trim().ToUpper());
                            if (objBAMBOOECO != null)
                            {
                                var dou = Utility.ConvertTodouble(objBAMBOOECO.Value);
                                value += dou.HasValue ? dou.Value : 0;

                            }
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
        public double GetPriceCongThemQuocTe(List<SettingUserInfo> listinput, string codehang)
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
            else if (codehang == "JQ" || codehang == "3K" || codehang == "BL")
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
                if (dateaddtru3.Month != dtInput.Month)
                {
                    objlist.AddRange(service.GetMonthlyTicket(dateaddtru3.Month, dateaddtru3.Year, start, end, 1, 1, 1, Uservethang, Pasvethang));
                }
                else if (dateaddcong3.Month != dtInput.Month)
                {
                    objlist.AddRange(service.GetMonthlyTicket(dateaddcong3.Month, dateaddcong3.Year, start, end, 1, 1, 1, Uservethang, Pasvethang));
                }
                objlist.AddRange(service.GetMonthlyTicket(dtInput.Month, dtInput.Year, start, end, 1, 1, 1, Uservethang, Pasvethang));
                objlist = objlist.Where(z => z.StartDate.Value.Date >= dateaddtru3.AddDays(-1) && z.StartDate.Value.Date <= dateaddcong3.AddDays(1)).ToList();
                return objlist.ToList();
            }
        }
        public async Task<IEnumerable<VDMFareDataInfo>> AddLccFlightInfoAsync(ApiClient.Models.FareData[] list, int ItineraryType,
           List<SettingUserInfo> listsetting, int nguoilon, int treem, int embe,
           string SessionId, List<MonthSearch.HanchesohieuInfo> listhanche, List<BaggageVn> lstBaggageVn)
        {
            var listvalue = new List<VDMFareDataInfo>();
            foreach (var objlist in list.OrderBy(z => z.FareAdt))
            {

                var objFlightInfoFirst = objlist.ListFlight.FirstOrDefault();
                var objFlightInfoLast = objlist.ListFlight.LastOrDefault();
                if (objFlightInfoFirst != null && objFlightInfoLast != null)
                {
                    var hangve = objFlightInfoFirst.FareClass;
                    if (objlist.Airline == "VJ")
                    {
                        if (objlist.Promo)
                        {
                            hangve = "PROMO";
                        }
                    }
                    var getgiacongthem = GetPriceCongThem(listsetting, objlist.Airline, hangve);
                    VDMFareDataInfo ck = new VDMFareDataInfo();
                    ck.FareDataIdValue = objlist.Airline + objlist.FareDataId;
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
                        ck.StopNum = objFlightInfoFirst.StopNum;
                        ck.Ishow23KgVN = true;
                        if (objlist.Airline == "VN")
                        {
                            ck.Ishow23KgVN = BaggageVnUti.CheckBaggageVn(lstBaggageVn, ck.FlightNumber, ck.FareClass, (decimal)ck.FareAdtNet);
                        }
                        listvalue.Add(ck);
                    }

                }
            }
            return listvalue;
        }
        public async Task<IEnumerable<VDMFareDataInfo>> AddLccFlightInfoJQAsync(ApiClient.Models.VDMLccFlight[] list, int ItineraryType,
           List<SettingUserInfo> listsetting, int nguoilon, int treem, int embe,
           string SessionId, List<MonthSearch.HanchesohieuInfo> listhanche)
        {
            var listvalue = new List<VDMFareDataInfo>();
            foreach (var objlist in list.OrderBy(z => z.FareAdt))
            {

                var hangve = objlist.FareClass;
                var getgiacongthem = GetPriceCongThem(listsetting, objlist.Airline, hangve);
                VDMFareDataInfo ck = new VDMFareDataInfo();
                ck.FareClass = (objlist.FareClass ?? "").ToUpper();
                ck.FareDataIdValue = "JQ" + objlist.FlightId;
                ck.StartDate = objlist.StartDate;
                ck.EndDate = objlist.EndDate;
                ck.StartPoint = objlist.StartPoint;
                ck.EndPoint = objlist.EndPoint;
                ck.FlightNumber = objlist.FlightNumber;
                ck.FareClass = objlist.FareClass;
                ck.GroupClass = objlist.GroupClass;
                ck.FlightValue = "JQ" + objlist.FlightId;
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
                    ck.FareDataId = objlist.FlightId;
                    ck.ItineraryType = ItineraryType;
                    ck.Airline = objlist.Airline;
                    ck.Duration = objlist.Duration;
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
                    ck.Promo = objlist.Promo;
                    ck.StopNum = objlist.ListSegment.Count() - 1;
                    var FlightInfo = new Flight()
                    {
                        BaseFare = objlist.FareAdt,
                        TotalPrice = objlist.TotalPrice,
                        Default = "1",
                        ListSegment = (from n in objlist.ListSegment
                                       select new Segment
                                       {
                                           Plane = n.Plane,
                                           Class = n.Class,
                                           Duration = n.Duration,
                                           FlightNumber = n.FlightNumber,
                                           EndTime = n.EndTime,
                                           StartTime = n.StartTime,
                                           EndPoint = n.EndPoint,
                                           StartPoint = n.StartPoint,
                                           Airline = n.Airline,
                                       }).ToArray(),
                        Promo = objlist.Promo,
                        FareClass = objlist.FareClass,
                        GroupClass = objlist.GroupClass,
                        NoRefund = true,
                        PriceAdt = objlist.FareAdt,
                        Duration = objlist.Duration,
                        StopNum = objlist.ListSegment.Count() - 1,
                        FlightNumber = objlist.FlightNumber,
                        EndDate = objlist.EndDate,
                        StartDate = objlist.StartDate,
                        EndPoint = objlist.EndPoint,
                        StartPoint = objlist.StartPoint,
                        Airline = "JQ",
                    };
                    var ListFlight = new List<Flight>();
                    ListFlight.Add(FlightInfo);
                    ck.ListFlight = ListFlight.ToArray();
                    ck.Ishow23KgVN = true;
                    listvalue.Add(ck);
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
                var objFlightInfoFirst = objlist.ListFlight.FirstOrDefault();
                var objFlightInfoLast = objlist.ListFlight.LastOrDefault();
                if (objFlightInfoFirst != null && objFlightInfoLast != null)
                {
                    var hangve = objFlightInfoFirst.FareClass;
                    if (objlist.Airline == "VJ")
                    {
                        if (objlist.Promo)
                        {
                            hangve = "PROMO";
                        }
                    }
                    var getgiacongthem = GetPriceCongThem(listsetting, objlist.Airline, hangve);

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
                        ck.FareDataIdValue = (objlist.Airline + objlist.FareDataId);
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

                if (fareData.ListFlight.Where(z => Utility.lstAirport.Contains(z.Airline)).Count() > 0)
                {
                    continue;
                }
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
                    ck.FareDataIdValue = (fareData.Airline + fareData.FareDataId);
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
                    //if (listairline.Count(z => z.Code == code) <= 0)
                    //{
                    //    var Getairlineinfos = Getairlineinfo(code);
                    //    if (Getairlineinfos != null)
                    //    {
                    //        Getairlineinfos.GiareNhat = ck.FareAdt;
                    //        listairline.Add(Getairlineinfos);
                    //    }
                    //}
                    //else
                    //{
                    //    var obj = listairline.FirstOrDefault(z => z.Code == code);
                    //    if (obj != null)
                    //    {
                    //        if (obj.GiareNhat > ck.FareAdt)
                    //        {
                    //            obj.GiareNhat = ck.FareAdt;
                    //        }
                    //    }
                    //}
                }
            }
            return listrt;
        }
        public List<VDMFareDataInfo> AddFareDataInfoJQ(List<SettingUserInfo> listsetting, ApiClient.Models.VDMFareData[] list, ref List<AirlineInfo> listairline, ref List<int> ListStopNum, DateTime? DepartureDate, DateTime? ReturnDate, string SessionId, string FromCity, string ToCity, int? Itinerary)
        {
            List<VDMFareDataInfo> listrt = new List<VDMFareDataInfo>();
            foreach (var fareData in list)
            {

                double giacongthemchieudi = 0;
                double giacongthemchieuve = 0;
                string code = "";
                int StopNum = 0;
                foreach (var fareDataInfo in fareData.ListDepartFlight)
                {

                    code = fareDataInfo.Airline;
                    StopNum = fareDataInfo.StopNum;
                    break;
                }
                foreach (var fareDataInfo in fareData.ListDepartFlight)
                {
                    var giamtam = GetPriceCongThemQuocTe(listsetting, fareDataInfo.Airline);
                    if (giamtam > giacongthemchieudi)
                        giacongthemchieudi = giamtam;
                }
                if (Itinerary > 1)
                {
                    if (fareData.ListReturnFlight != null)
                    {
                        foreach (var fareDataInfo in fareData.ListReturnFlight)
                        {
                            var giamtam = GetPriceCongThemQuocTe(listsetting, fareDataInfo.Airline);
                            if (giamtam > giacongthemchieuve)
                                giacongthemchieuve = giamtam;
                        }
                    }
                }
                var giacongthem = giacongthemchieudi + giacongthemchieuve;
                double giacongthemphantramAdt = 0;
                double giacongthemphantramChd = 0;
                if (!ListStopNum.Contains(StopNum))
                {
                    ListStopNum.Add(StopNum);
                }
                //var objFlightInfoFirst = fareData.ListFlight.FirstOrDefault();
                //var objFlightInfoLast = fareData.ListFlight.LastOrDefault();
                //if (objFlightInfoFirst != null && objFlightInfoLast != null)
                //{
                VDMFareDataInfo ck = new VDMFareDataInfo();
                ck.SessionId = SessionId;
                // ck.FareClass = (fareData ?? "").ToUpper();
                //ck.StartDate = fareData.StartDate;
                //ck.EndDate = fareData.EndDate;
                //ck.StartPoint = fareData.StartPoint;
                //ck.EndPoint = fareData.EndPoint;
                //ck.FlightNumber = fareData.FlightNumber;
                //ck.FareClass = fareData.FareClass;
                //ck.GroupClass = fareData.GroupClass;
                ck.Airline = code;
                ck.StopNum = StopNum;
                ck.Duration = fareData.ListDepartFlight.Sum(z => z.Duration);
                ck.FareDataId = fareData.FareDataId;
                ck.FareDataIdValue = ("DTC" + fareData.FareDataId);
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
                var ListFlight = new List<Flight>();
                if (fareData.ListDepartFlight != null)
                {
                    foreach (var itemFlight in fareData.ListDepartFlight)
                    {
                        var objSegment = itemFlight.ListSegment.FirstOrDefault();
                        var FlightInfo = new Flight()
                        {
                            BaseFare = fareData.FareAdt,
                            TotalPrice = fareData.TotalPrice,
                            Default = "1",
                            ListSegment = (from n in itemFlight.ListSegment
                                           select new Segment
                                           {
                                               Plane = n.Plane,
                                               Class = n.Class,
                                               Duration = n.Duration,
                                               FlightNumber = n.FlightNumber,
                                               EndTime = n.EndTime,
                                               StartTime = n.StartTime,
                                               EndPoint = n.EndPoint,
                                               StartPoint = n.StartPoint,
                                               Airline = n.Airline,
                                           }).ToArray(),
                            Promo = fareData.Promo,
                            NoRefund = true,
                            PriceAdt = fareData.FareAdt,
                            Duration = itemFlight.Duration,
                            StopNum = itemFlight.ListSegment.Count() - 1,
                            FlightNumber = objSegment != null ? objSegment.FlightNumber : "",
                            EndDate = itemFlight.EndDate,
                            StartDate = itemFlight.StartDate,
                            EndPoint = itemFlight.EndPoint,
                            StartPoint = itemFlight.StartPoint,
                            Airline = itemFlight.Airline,
                            FlightId = itemFlight.FlightId,

                        };
                        ListFlight.Add(FlightInfo);
                    }
                }
                if (fareData.ListReturnFlight != null)
                {
                    foreach (var itemFlight in fareData.ListReturnFlight)
                    {
                        var objSegment = itemFlight.ListSegment.FirstOrDefault();
                        var FlightInfo = new Flight()
                        {
                            BaseFare = fareData.FareAdt,
                            TotalPrice = fareData.TotalPrice,
                            Default = "1",
                            ListSegment = (from n in itemFlight.ListSegment
                                           select new Segment
                                           {
                                               Plane = n.Plane,
                                               Class = n.Class,
                                               Duration = n.Duration,
                                               FlightNumber = n.FlightNumber,
                                               EndTime = n.EndTime,
                                               StartTime = n.StartTime,
                                               EndPoint = n.EndPoint,
                                               StartPoint = n.StartPoint,
                                               Airline = n.Airline,
                                           }).ToArray(),
                            Promo = fareData.Promo,
                            NoRefund = true,
                            PriceAdt = fareData.FareAdt,
                            Duration = itemFlight.Duration,
                            StopNum = itemFlight.ListSegment.Count() - 1,
                            FlightNumber = objSegment != null ? objSegment.FlightNumber : "",
                            EndDate = itemFlight.EndDate,
                            StartDate = itemFlight.StartDate,
                            EndPoint = itemFlight.EndPoint,
                            StartPoint = itemFlight.StartPoint,
                            Airline = itemFlight.Airline,
                            FlightId = itemFlight.FlightId,
                        };
                        ListFlight.Add(FlightInfo);
                    }
                }
                ck.ListFlight = ListFlight.ToArray();
                ck.System = fareData.System;
                listrt.Add(ck);
                //if (listairline.Count(z => z.Code == code) <= 0)
                //{
                //    var Getairlineinfos = Getairlineinfo(code);
                //    if (Getairlineinfos != null)
                //    {
                //        Getairlineinfos.GiareNhat = ck.FareAdt;
                //        listairline.Add(Getairlineinfos);
                //    }
                //}
                //else
                //{
                //    var obj = listairline.FirstOrDefault(z => z.Code == code);
                //    if (obj != null)
                //    {
                //        if (obj.GiareNhat > ck.FareAdt)
                //        {
                //            obj.GiareNhat = ck.FareAdt;
                //        }
                //    }
                //}
                //}
            }
            return listrt;
        }
        public VeThangareDataInfo AddFlightFareInfoMonth(List<SettingUserInfo> listsetting, MonthSearch.FlightFareInfo list)
        {
            var listvalue = new VeThangareDataInfo();
            var objlist = list;
            var hangve = objlist.FareClass;
            if (objlist.AirlineCode == "VJ")
            {
                if (objlist.Promo == true)
                {
                    hangve = "PROMO";
                }
            }
            var getgiacongthem = GetPriceCongThem(listsetting, objlist.AirlineCode, hangve);
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
    }
}