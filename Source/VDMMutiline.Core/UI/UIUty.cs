using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.Ultilities;
using VDMMutiline.SharedComponent.EntityInfo;

using System.Web.Mvc;
using VDMMutiline.SharedComponent.Params;
using System.Net.Mail;
using VDMMutiline.SharedComponent.Constants;

namespace VDMMutiline.Core.UI
{
    public class UIUty
    {
        public static string GetTimeString(int durationInMinute)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(durationInMinute);

            if (timeSpan.Hours == 1 && timeSpan.Minutes == 1)
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
            else if (timeSpan.Hours > 1 && timeSpan.Minutes > 1)
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
            else if (timeSpan.Hours > 1 && timeSpan.Minutes < 1)
                return timeSpan.Hours + " h";
            else if (timeSpan.Hours < 1 && timeSpan.Minutes > 1)
                return timeSpan.Minutes + " m";
            else if (timeSpan.Hours == 1 && timeSpan.Minutes > 1)
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
            else if (timeSpan.Hours == 1 && timeSpan.Minutes == 0)
                return timeSpan.Hours + " h";
            else if (timeSpan.Hours == 0 && timeSpan.Minutes == 1)
                return timeSpan.Minutes + " m";
            else
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
        }
        public static string GetTimeString(double durationInMinute)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(durationInMinute);

            if (timeSpan.Hours == 1 && timeSpan.Minutes == 1)
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
            else if (timeSpan.Hours > 1 && timeSpan.Minutes > 1)
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
            else if (timeSpan.Hours > 1 && timeSpan.Minutes < 1)
                return timeSpan.Hours + " h";
            else if (timeSpan.Hours < 1 && timeSpan.Minutes > 1)
                return timeSpan.Minutes + " m";
            else if (timeSpan.Hours == 1 && timeSpan.Minutes > 1)
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
            else if (timeSpan.Hours == 1 && timeSpan.Minutes == 0)
                return timeSpan.Hours + " h";
            else if (timeSpan.Hours == 0 && timeSpan.Minutes == 1)
                return timeSpan.Minutes + " m";
            else
                return timeSpan.Hours + " h " + timeSpan.Minutes + " m";
        }
        public static string GetImagebyHang(string Hang)
        {
            string link = "";
            var dao = new AirlineDao();
            var obj = dao.GetbyCode(Hang);
            if (obj != null)
            {
                link = obj.Pictures;
            }
            return link;
        }
        public static decimal ChuyenDoiTienTe(double sotien)
        {
            decimal valuetiente = 0;
            //var cookie = HttpContext.Current.Request.Cookies["Language"];
            //string culture = string.Empty;
            //if (cookie == null)
            //{
            //    culture = "Vi";
            //}
            //else
            //    culture = cookie.Value;
            //if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
            //{
            //    culture = "Vi";
            //}
            //if (culture.ToLower().CompareTo("en-us") == 0)
            //{
            //    culture = "en-us";
            //}
            //if (culture == "en-us")
            //{
            //    var dao = new SettingDao();
            //    var list = dao.GetSetingUsingUser(0);
            //    var obj = list.FirstOrDefault(z => z.Key.Trim().ToUpper() == "Dollar".Trim().ToUpper());

            //    if (obj != null)
            //    {
            //        var value = Utility.ConvertToDecimal(obj.Value);
            //        if (value != null)
            //        {
            //            valuetiente = Math.Round((Convert.ToDecimal(sotien) / value.Value), 1);
            //        }
            //    }
            //}
            //else
            valuetiente = Convert.ToDecimal(sotien);
            return valuetiente;
        }
        public static decimal ChuyenDoiTienTe(decimal sotien, List<SettingUserInfo> list)
        {
            return sotien;
            decimal valuetiente = 0;
            var cookie = HttpContext.Current.Request.Cookies["Language"];
            string culture = string.Empty;
            if (cookie == null)
            {
                culture = "Vi";
            }
            else
                culture = cookie.Value;
            if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
            {
                culture = "Vi";
            }
            if (culture.ToLower().CompareTo("en-us") == 0)
            {
                culture = "en-us";
            }
            if (culture == "en-us")
            {

                var obj = list.FirstOrDefault(z => z.Name.Trim().ToUpper() == "Dollar".Trim().ToUpper());

                if (obj != null)
                {
                    var value = Utility.ConvertToDecimal(obj.Value);
                    if (value != null)
                    {
                        valuetiente = Math.Round((Convert.ToDecimal(sotien) / value.Value), 1);
                    }
                }
            }
            else
                valuetiente = Convert.ToDecimal(sotien);
            return valuetiente;
        }
        public static string GetDonViTienTe()
        {
            //var cookie = HttpContext.Current.Request.Cookies["Language"];
            //string culture = string.Empty;
            //if (cookie == null)
            //{
            //    culture = "Vi";
            //}
            //else
            //    culture = cookie.Value;
            //if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
            //{
            //    culture = "Vi";
            //}
            //if (culture.ToLower().CompareTo("en-us") == 0)
            //{
            //    culture = "en-us";
            //}
            //if (culture == "en-us")
            //{
            //    return "USD";
            //}
            //else
            return "VNĐ";
        }
        public static string GetNamebyHang(string Hang)
        {
            string link = "";
            var dao = new AirlineDao();
            var obj = dao.GetbyCode(Hang);
            if (obj != null)
            {
                link = obj.Name;
            }
            return link;
        }
        public static List<tbl_Language> GetDropNgonNgu()
        {
            var dao = new Languaghedao();
            return dao.GetList();
        }
        public static string GetNameDatetime(DateTime? dateinput)
        {
            string names = "";
            if (dateinput != null)
            {
                names = getnameday(dateinput) + " " + dateinput.Value.Day + " " + getmountday(dateinput) + " " + dateinput.Value.Year;
            }
            return names;
        }
        public static string getnameday(DateTime? dateinput)
        {
            string names = "";
            if (dateinput != null)
            {
                var cookie = HttpContext.Current.Request.Cookies["Language"];
                string culture = string.Empty;
                if (cookie == null)
                {
                    culture = "Vi";
                }
                else
                    culture = cookie.Value;
                if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "Vi";
                }
                if (culture.ToLower().CompareTo("en-us") == 0)
                {
                    culture = "en-us";
                }
                switch (dateinput.Value.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        names = culture == "Vi" ? "Thứ hai" : "Monday";
                        break;
                    case DayOfWeek.Tuesday:
                        names = culture == "Vi" ? "Thứ ba" : "Tuesday";
                        break;
                    case DayOfWeek.Wednesday:
                        names = culture == "Vi" ? "Thứ tư" : "Wednesday";
                        break;
                    case DayOfWeek.Thursday:
                        names = culture == "Vi" ? "Thứ năm" : "Thursday";
                        break;
                    case DayOfWeek.Friday:
                        names = culture == "Vi" ? "Thứ sáu" : "Friday";
                        break;
                    case DayOfWeek.Saturday:
                        names = culture == "Vi" ? "Thứ bảy" : "Saturday";
                        break;
                    case DayOfWeek.Sunday:
                        names = culture == "Vi" ? "Chủ nhật" : "Sunday";
                        break;
                }
            }
            return names;
        }
        public static string getmountday(DateTime? dateinput)
        {
            string names = "";
            if (dateinput != null)
            {
                var cookie = HttpContext.Current.Request.Cookies["Language"];
                string culture = string.Empty;
                if (cookie == null)
                {
                    culture = "Vi";
                }
                else
                    culture = cookie.Value;
                if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "Vi";
                }
                if (culture.ToLower().CompareTo("en-us") == 0)
                {
                    culture = "en-us";
                }
                switch (dateinput.Value.Month)
                {
                    case 1:
                        names = culture == "Vi" ? "Tháng Giêng" : "January";
                        break;
                    case 2:
                        names = culture == "Vi" ? "Tháng Hai" : "February";
                        break;
                    case 3:
                        names = culture == "Vi" ? "Tháng Ba" : "March";
                        break;
                    case 4:
                        names = culture == "Vi" ? "Tháng Tư" : "April";
                        break;
                    case 5:
                        names = culture == "Vi" ? "Tháng Năm" : "May";
                        break;
                    case 6:
                        names = culture == "Vi" ? "Tháng Sáu" : "June";
                        break;
                    case 7:
                        names = culture == "Vi" ? "Tháng Bảy" : "July";
                        break;
                    case 8:
                        names = culture == "Vi" ? "Tháng Tám" : "August";
                        break;
                    case 9:
                        names = culture == "Vi" ? "Tháng Chín" : "September";
                        break;
                    case 10:
                        names = culture == "Vi" ? "Tháng Mười" : "October";
                        break;
                    case 11:
                        names = culture == "Vi" ? "Tháng Mười Một" : "November";
                        break;
                    case 12:
                        names = culture == "Vi" ? "Tháng Mười Hai" : "December";
                        break;
                }
            }
            return names;

        }
        public static string MonthName(int m)
        {
            string res;
            switch (m)
            {
                case 1:
                    res = "Jan";
                    break;
                case 2:
                    res = "Feb";
                    break;
                case 3:
                    res = "Mar";
                    break;
                case 4:
                    res = "Apr";
                    break;
                case 5:
                    res = "May";
                    break;
                case 6:
                    res = "Jun";
                    break;
                case 7:
                    res = "Jul";
                    break;
                case 8:
                    res = "Aug";
                    break;
                case 9:
                    res = "Sep";
                    break;
                case 10:
                    res = "Oct";
                    break;
                case 11:
                    res = "Nov";
                    break;
                case 12:
                    res = "Dec";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }
        public static string GetHanhLyKyGui(string Hang, string Hangve, bool isShow23kgVN, bool IsVNJQ, bool IsVNJQ0Kg)
        {
            string link = "";
            switch (Hang.Trim().ToUpper())
            {
                case "JQ":
                    link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: Chọn mua bước sau</li><li>Thay đổi chuyến bay: Tùy theo điều kiện vé được phép thay đổi thu phí + chênh lệch giá vé (nếu có)</li><li>Nâng Hạng: Tùy theo điều kiện vé được phép thay đổi, thu phí + chênh lệch giá vé (nếu có)</li><li>Đổi Hành Trình: Tùy theo điều kiện vé được phép thay đổi, thu phí + chênh lệch giá vé (nếu có)</li><li>Đổi Tên Hành Khách: Tùy theo điều kiện vé được phép thay đổi, thu phí + chênh lệch giá vé (nếu có)</li></ul>";
                    break;
                case "VJ":
                    link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: Chọn mua bước sau</li><li>Điều kiện vé</li><li>Đổi Tên Hành khách:	Thu phí (hạng promo (khuyến mãi) không được phép)</li><li>Hoàn vé:	Không được phép </li><li>Đổi Hành trình:	Thu phí + chênh lệch giá vé (nếu có)</li><li>Bảo lưu:	Không được phép</li><li>Đổi ngày bay:	Thu phí + chênh lệch giá vé (nếu có)</li><li>Nâng hạng: Tùy theo quy định của hãng, thu phí + chênh lệch giá vé</li><li>Thời hạn thay đổi:	Trước 01 ngày so với ngày khởi hành</li></ul>";
                    break;
                case "VN":
                    if (IsVNJQ)
                    {
                        link = "<ul><li>Hành lý xách tay: 7kg </li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                    }
                    else if(!isShow23kgVN)
                    {
                        link = "<ul><li>Hành lý xách tay: 10kg + 2kg phụ kiện </li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                    }
                    else
                    {
                        link = "<ul><li>Hành lý xách tay: 10kg + 2kg phụ kiện </li><li>Hành lý ký gửi: 1 kiện 23 kg</li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                    }
                    break;
                case "QH":
                    {
                        if (Hangve == null)
                            Hangve = "";
                        if (Hangve.ToUpper().Trim() == "BAMBOOECO" || Hangve.ToUpper().Trim() == "ECONOMYSAVERMAX")
                            link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: Mua thêm sau</li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                        else
                            link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: 20 kg</li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                        break;
                    }
            }
            return link;
        }
        public static string GetHanhLyKyGuikhuyenmai(string Hang, string Hangve, bool isShow23kgVN, bool IsVNJQ, bool IsVNJQ0Kg)
        {
            string link = "";
            switch (Hang.Trim().ToUpper())
            {
                case "JQ":
                    link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: Chọn mua bước sau</li><li>Thay đổi chuyến bay: Tùy theo điều kiện vé được phép thay đổi thu phí + chênh lệch giá vé (nếu có)</li><li>Nâng Hạng: Tùy theo điều kiện vé được phép thay đổi, thu phí + chênh lệch giá vé (nếu có)</li><li>Đổi Hành Trình: Tùy theo điều kiện vé được phép thay đổi, thu phí + chênh lệch giá vé (nếu có)</li><li>Đổi Tên Hành Khách: Tùy theo điều kiện vé được phép thay đổi, thu phí + chênh lệch giá vé (nếu có)</li></ul>";
                    break;
                case "VJ":
                    link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: Chọn mua bước sau</li><li>Đổi Tên Hành khách:	Thu phí (hạng promo (khuyến mãi) không được phép)</li><li>Hoàn vé:	Không được phép </li><li>Đổi Hành trình:	Thu phí + chênh lệch giá vé (nếu có)</li><li>Bảo lưu:	Không được phép</li><li>Đổi ngày bay:	Thu phí + chênh lệch giá vé (nếu có)</li><li>Nâng hạng: Tùy theo quy định của hãng, thu phí + chênh lệch giá vé</li><li>Thời hạn thay đổi:	Trước 01 ngày so với ngày khởi hành</li></ul>";
                    break;
                case "VN":
                    if (IsVNJQ)
                    {
                        link = "<ul><li>Hành lý xách tay: 7kg </li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                    }
                    else if(!isShow23kgVN)
                    {
                        link = "<ul><li>Hành lý xách tay: 10kg + 2kg phụ kiện </li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi 300.000 + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí 600.000 + chênh lệch giá vé (nếu có).</li></ul>";
                    }
                    else
                    {
                        link = "<ul><li>Hành lý xách tay: 10kg + 2kg phụ kiện </li><li>Hành lý ký gửi: 1 kiện 23 kg</li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi 300.000 + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí 600.000 + chênh lệch giá vé (nếu có).</li></ul>";
                    }
                    break;
                case "QH":
                    {
                        if (Hangve == null)
                            Hangve = "";
                        if (Hangve.ToUpper().Trim() == "BAMBOOECO" || Hangve.ToUpper().Trim() == "ECONOMYSAVERMAX")
                            link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: Mua thêm sau</li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                        else
                            link = "<ul><li>Hành lý xách tay: 07 kg</li><li>Hành lý ký gửi: 20 kg</li><li>Đổi Tên Hành khách: Không được phép</li><li>Hoàn/hủy vé: Tùy theo quy định của hãng, Thu phí hoàn vé (nếu được phép hoàn/hủy)</li><li>Thay đổi chặng bay/hành trình:	Trước ngày khởi hành: Được phép, thu phí đổi + chênh lệch giá vé (nếu có). Từ ngày khởi hành trở đi: Được phép, phí + chênh lệch giá vé (nếu có).</li></ul>";
                        break;
                    }
            }
            return link;
        }
        public static string HtmlBlock(string key)
        {
            var dao = new HMTLBlockDao();
            var obj = dao.GetbyKey(key);
            if (obj != null)
                return obj.HtmlblockConten;
            else return "";
        }

        public static DateTime GetFirstDayOfMonth(DateTime dtInput)
        {
            var dtResult = dtInput;
            dtResult = dtResult.AddDays((-dtResult.Day) + 1);
            return dtResult;
        }
        public static DateTime GetFirstDayOfMonth(int iMonth, int year)
        {
            var dtResult = new DateTime(year, iMonth, 1);
            dtResult = dtResult.AddDays((-dtResult.Day) + 1);
            return dtResult;
        }
        public static DateTime GetLastDayOfMonth(DateTime dtInput)
        {
            var dtResult = dtInput;
            dtResult = dtResult.AddMonths(1);
            dtResult = dtResult.AddDays(-(dtResult.Day));
            return dtResult;
        }
        public static DateTime GetLastDayOfMonth(int iMonth, int year)
        {
            var dtResult = new DateTime(year, iMonth, 1);
            dtResult = dtResult.AddMonths(1);
            dtResult = dtResult.AddDays(-(dtResult.Day));
            return dtResult;
        }
        //public static List<MonthSearch.FlightFareInfo> Getlistbythang(DateTime dtInput, string start, string end)
        //{
        //    string Uservethang = System.Configuration.ConfigurationSettings.AppSettings.Get("Uservethang");
        //    if (!string.IsNullOrEmpty(Uservethang))
        //        Uservethang = Utility.Decrypt(Uservethang, true);
        //    string Pasvethang = System.Configuration.ConfigurationSettings.AppSettings.Get("Pasvethang");
        //    if (!string.IsNullOrEmpty(Pasvethang))
        //        Pasvethang = Utility.Decrypt(Pasvethang, true);
        //    using (var service = new MonthSearch.MonthSearchSoapClient())
        //    {
        //        var objlist = service.GetMonthlyTicket(dtInput.Month, dtInput.Year, start, end, 1, 1, 1, Uservethang, Pasvethang);
        //        return objlist.ToList();
        //    }
        //}
        //public static MonthSearch.FlightFareInfo Getgiarenhat(DateTime dtInput, List<MonthSearch.FlightFareInfo> objlist)
        //{
        //    if (objlist != null)
        //    {
        //        var listsetting = GetSeting();
        //        var listSetingSystem = GetSetingSystem();
        //        var getgiacongthem = GetPriceCongThem(listsetting, objlist.AirlineCode) + GetPriceCongThemhangG(listSetingSystem, objlist.AirlineCode, objlist.FareClass);
        //        var item = new MonthSearch.FlightFareInfo
        //        {
        //            StartPoint = objlist.StartPoint,
        //            EndPoint = objlist.EndPoint,
        //            SelectValue = objlist.SelectValue,
        //            Currency = objlist.Currency,
        //            PriceAdult = Utility.Ceiling(objlist.PriceAdult.Value, 1000),
        //            PriceChild = Utility.Ceiling(objlist.PriceChild.Value, 1000),
        //            PriceInfant = Utility.Ceiling(objlist.PriceInfant.Value, 1000),
        //            FeeAdult = Utility.Ceiling(objlist.FeeAdult.Value + getgiacongthem, 1000),
        //            FeeChild = Utility.Ceiling(objlist.FeeChild.Value + getgiacongthem, 1000),
        //            FeeInfant = Utility.Ceiling(objlist.FeeInfant.Value, 1000),
        //            TaxAdult = Utility.Ceiling(objlist.TaxAdult.Value, 1000),
        //            TaxChild = Utility.Ceiling(objlist.TaxChild.Value, 1000),
        //            TaxInfant = Utility.Ceiling(objlist.TaxInfant.Value, 1000),
        //            PriceBefor = Utility.Ceiling(objlist.TotalPrice.Value, 1000),
        //            TotalPrice = Utility.Ceiling(objlist.TotalPrice.Value + getgiacongthem, 1000),
        //            GroupClass = objlist.GroupClass,
        //            FareClass = objlist.FareClass,
        //            FltNumb = objlist.FltNumb,
        //        };
        //        return item;

        //    }
        //    return null;

        //}
        private static double GetPriceCongThem(List<SettingUserInfo> listinput, string codeAl)
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
            }
            return value;
        }
        private static double GetPriceCongThemhangG(List<SettingUserInfo> listinput, string codeAl, string classG)
        {
            double value = 0;
            if (codeAl.Trim().ToUpper() == "VN" && classG.Trim().ToUpper() == "G")
            {
                var obj = listinput.FirstOrDefault(z => z.Name.Trim().ToUpper() == "FeeVietnamAirline_G".Trim().ToUpper());
                if (obj != null)
                {
                    var dou = Utility.ConvertTodouble(obj.Value);
                    value = dou.HasValue ? dou.Value : 0;
                }
            }
            return value;
        }
        public static string catgia(double so)
        {
            var gia = so.ToString("n0");
            var gialent = gia.Length;
            return gialent > 4 ? gia.Remove(gialent - 4, 4) : gia;
        }
        public static string GetsettingByKey(List<SettingUserInfo> ListSetting, string key)
        {
            var obj = ListSetting.FirstOrDefault(z => z.Name == key);
            if (obj != null)
                return obj.Value;
            return "";
        }
        public static List<MailAddress> GetMailCC(List<SettingUserInfo> ListSetting)
        {
            List<MailAddress> ccmail = new List<MailAddress>();
            try
            {
                var liststringmail = GetsettingByKey(ListSetting, "EmailSender_CCList");
                if (!string.IsNullOrEmpty(liststringmail))
                {
                    var spil = liststringmail.Split(',');
                    foreach (var s in spil)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            ccmail.Add(new MailAddress(s));
                        }
                    }
                }
            }
            catch
            {

            }
            return ccmail;
        }
        public static string getnamedayshos(DateTime? dateinput)
        {
            string names = "";
            if (dateinput != null)
            {
                var cookie = HttpContext.Current.Request.Cookies["Language"];
                string culture = string.Empty;
                if (cookie == null)
                {
                    culture = "Vi";
                }
                else
                    culture = cookie.Value;
                if (culture.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "Vi";
                }
                if (culture.ToLower().CompareTo("en-us") == 0)
                {
                    culture = "en-us";
                }
                culture = "Vi";
                switch (dateinput.Value.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        names = culture == "Vi" ? "T2" : "Mon";
                        break;
                    case DayOfWeek.Tuesday:
                        names = culture == "Vi" ? "T3" : "Tue";
                        break;
                    case DayOfWeek.Wednesday:
                        names = culture == "Vi" ? "T4" : "Wed";
                        break;
                    case DayOfWeek.Thursday:
                        names = culture == "Vi" ? "T5" : "Thu";
                        break;
                    case DayOfWeek.Friday:
                        names = culture == "Vi" ? "T6" : "Fri";
                        break;
                    case DayOfWeek.Saturday:
                        names = culture == "Vi" ? "T7" : "Sa";
                        break;
                    case DayOfWeek.Sunday:
                        names = culture == "Vi" ? "CN" : "Sun";
                        break;
                }
            }
            return names;
        }
        public static int GetIdPdfTempleate(BK_BookingInfo BookingInfo)
        {
            if (BookingInfo != null)
            {
                if (!Station.CheckQuocTe(BookingInfo.FromCity, BookingInfo.ToCity))
                {
                    var daoticket = new BK_TicketDao();
                    var listTicket = daoticket.GetListByBooking(BookingInfo.ID);
                    if (listTicket.Count(z => z.Code != "VJ") > 0)
                        return Constant.TemPleateHTMLID.PDFALL;
                    else return Constant.TemPleateHTMLID.PDFVJ;
                }
                else
                {
                    return Constant.TemPleateHTMLID.PDFALL;
                }
            }
            return Constant.TemPleateHTMLID.PDFALL;
        }

    }
}