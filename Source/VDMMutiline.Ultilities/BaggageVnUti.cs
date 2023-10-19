using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.Ultilities;

namespace WebProject.Ultilities
{
    public static class BaggageVnUti
    {
        public static List<ApiClient.Models.BaggageVn> GetBaggageVn(string StartPoind, string EndPoind)
        {
            var apiclient = new ApiClient.Common.ApiClient();
            var obj = apiclient.GetBaggageVn(StartPoind, EndPoind);
            return obj;
        }
        public static List<ApiClient.Models.BaggageVnJq> GetBaggageVnJq(string StartPoind, string EndPoind)
        {
            var apiclient = new ApiClient.Common.ApiClient();
            var obj = apiclient.GetBaggageVnJq(StartPoind, EndPoind);
            return obj;
        }
        public static bool CheckBaggageVn(List<ApiClient.Models.BaggageVn> lst, string FlightNum, string FareClass, decimal? priceFare)
        {
            if (string.IsNullOrEmpty(FlightNum))
                return true;
            if (lst.Where(z => z.PriceFare == priceFare && z.FareClass.Contains(FareClass + ";")).Count() > 0)
                return false;
            if (lst.Where(z => z.FlightNum == FlightNum && z.FareClass.Contains(FareClass + ";")).Count() > 0)
                return false;
            return true;
        }
        public static bool CheckBaggageVn(List<ApiClient.Models.BaggageVn> lst, string FlightNum, string FareClass, decimal priceFare)
        {
            var listhangVe = new List<string> { "A", "P", "G" };
            if (listhangVe.Any(z => z == FareClass))
            {
                return false;
            }
            if (string.IsNullOrEmpty(FlightNum))
                return true;
            if (lst.Where(z => z.PriceFare == priceFare && z.FareClass.Contains(FareClass + ";")).Count() > 0)
                return false;
            if (lst.Where(z => z.FlightNum == FlightNum && z.FareClass.Contains(FareClass + ";")).Count() > 0)
                return false;
            return true;
        }
        public static bool CheckBaggageVnJq(List<ApiClient.Models.BaggageVnJq> lst, string FlightNum)
        {
            if (string.IsNullOrEmpty(FlightNum))
                return false;
            var FlightNumvalue = FlightNum.Replace("VN", "").Trim();
            if (IsNumeric(FlightNumvalue))
            {
                var FlightNumvl = Utility.ConvertToInt(FlightNumvalue) ?? 0;
                var objcheck = (from en in lst
                                where
                                (en.FlightNumFrom <= FlightNumvl && en.FlightNumTo >= FlightNumvl) || en.FlightNum == FlightNum
                                select en
                               ).ToList();
                if (objcheck.Count() > 0)
                    return true;
            }
            else
            {
                return false;
            }
            return false;
        }
        public static bool CheckBaggage0KgVnJq(List<ApiClient.Models.BaggageVnJq> lst, string FlightNum, string FareClass)
        {
            if (string.IsNullOrEmpty(FlightNum))
                return false;
            var FlightNumvalue = FlightNum.Replace("VN", "").Trim();
            if (IsNumeric(FlightNumvalue))
            {
                var FlightNumvl = Utility.ConvertToInt(FlightNumvalue) ?? 0;
                var objcheck = (from en in lst
                                where
                                ((en.FlightNumFrom <= FlightNumvl && en.FlightNumTo >= FlightNumvl) || en.FlightNum == FlightNum)
                                && en.FareClass0Kg.Contains(FareClass + ";")
                                select en
                               ).ToList();
                if (objcheck.Count() > 0)
                    return true;
            }
            else
            {
                return false;
            }
            return false;
        }
        public static bool IsNumeric(object Expression)
        {
            double retNum;
            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

    }
}
