using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Common.Constants
{
    public class FunctionDTC
    {
        public static readonly string DTCUrlRoot = ConfigurationManager.AppSettings["DTCUrl"];
        public static readonly string DTCDatabaseUrlRoot = ConfigurationManager.AppSettings["DTCDatabaseUrl"];
        
        public static readonly string search = DTCUrlRoot + "/flights/search";
        public static readonly string searchmin = DTCUrlRoot + "/flights/searchmin";
        public static readonly string searchmonth = DTCUrlRoot + "/flights/searchmonth";
        public static readonly string getbaggage = DTCUrlRoot + "/flights/getbaggage";
        public static readonly string getfarerules = DTCUrlRoot + "/flights/getfarerules";
        public static readonly string verifyflight = DTCUrlRoot + "/flights/verifyflight";
        public static readonly string book = DTCUrlRoot + "/flights/book";
        public static readonly string rebook = DTCUrlRoot + "/flights/rebook";
        public static readonly string issue = DTCUrlRoot + "/flights/issue";
        public static readonly string voids = DTCUrlRoot + "/flights/void";
        public static readonly string cancel = DTCUrlRoot + "/flights/cancel";
        public static readonly string retrieve = DTCUrlRoot + "/flights/retrieve";
        public static readonly string sendmail = DTCUrlRoot + "/flights/sendmail";
        public static readonly string addmembership = DTCUrlRoot + "/flights/addmembership";
        public static readonly string changepassenger = DTCUrlRoot + "/flights/changepassenger";
        public static readonly string splitpassenger = DTCUrlRoot + "/flights/splitpassenger";
        public static readonly string addinfant = DTCUrlRoot + "/flights/addinfant";
        public static readonly string addonservice = DTCUrlRoot + "/flights/addonservice";
        public static readonly string changeFlight = DTCUrlRoot + "/flights/changeFlight";
        public static readonly string openreport = DTCUrlRoot + "/flights/openreport";
        public static readonly string closereport = DTCUrlRoot + "/flights/closereport";
        public static readonly string bookingGetbyid = DTCDatabaseUrlRoot + "/booking/getbyid";
        public static readonly string bookingDetailGetbybookingid = DTCDatabaseUrlRoot + "/bookingdetail/getbybookingid";
        public static readonly string bookingPassengerGetbybookingid = DTCDatabaseUrlRoot + "/bookingpassenger/getbybookingid";
        public static readonly string bookingbaggageGetbybookingid = DTCDatabaseUrlRoot + "/bookingbaggage/getbybookingid";

    }
    public class FunctionJQ
    {
        public static readonly string JQUrlRoot = ConfigurationManager.AppSettings["JQUrl"];
        public static readonly string SearchTicketDomtic = JQUrlRoot + "/api/Jetstar/SearchTicketDomtic";
        public static readonly string SearchTicketInternational = JQUrlRoot + "/api/Jetstar/SearchTicketInternational";
        public static readonly string GetBaggages = JQUrlRoot + "/api/Jetstar/GetBaggages";
        public static readonly string getFareRule = JQUrlRoot + "/api/Jetstar/getFareRule";
        public static readonly string bookJetstart = JQUrlRoot + "/api/Jetstar/bookJetstart";
        public static readonly string bookJetstartInternational = JQUrlRoot + "/api/Jetstar/bookJetstartInternational";
        public static readonly string bookJetstartCustom = JQUrlRoot + "/api/Jetstar/bookJetstartCustom";
    }
    public class FunctionSysCommon
    {
        public static readonly string SysCommonUrl = ConfigurationManager.AppSettings["SysCommonUrl"];
        public static readonly string SearchAirport = SysCommonUrl+ "/api/Airport/SearchAirport";
        public static readonly string GetAirportByCode = SysCommonUrl + "/api/Airport/GetAirportByCode";
        public static readonly string GetAirportByListCode = SysCommonUrl + "/api/Airport/GetAirportByListCode";
        public static readonly string GetBaggageVn = SysCommonUrl + "/api/Baggage/GetBaggageVn";
        public static readonly string GetBaggageVnJq = SysCommonUrl + "/api/Baggage/GetBaggageVnJq";
    }
}
