using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMMutiline.Ultilities
{
    public static class Station
    {
        public static bool CheckQuocTe(string codedi, string codeve)
        {
            var apiclient = new ApiClient.Common.ApiClient();
            bool quocte = false;
            if (!string.IsNullOrEmpty(codedi))
            {
                var objdi = apiclient.GetAirportByCode(codedi);
                if (objdi != null)
                {
                    if (objdi.CountryCode != "VN")
                    {
                        quocte = true;
                        return quocte;
                    }
                }
            }
            if (!string.IsNullOrEmpty(codeve))
            {
                var objdi = apiclient.GetAirportByCode(codeve);
                if (objdi != null)
                {
                    if (objdi.CountryCode != "VN")
                    {
                        quocte = true;
                        return quocte;
                    }
                }
            }
            return quocte;
        }
        public static ApiClient.Models.Airport Getbycode(string code)
        {
            var apiclient = new ApiClient.Common.ApiClient();
            var obj = apiclient.GetAirportByCode(code);
            return obj;
        }
        public static List<ApiClient.Models.Airport> SearchAirport(string code)
        {
            var apiclient = new ApiClient.Common.ApiClient();
            var obj = apiclient.SearchAirport(code);
            return obj;
        }
        public static List<ApiClient.Models.Airport> GetAirportByListCode(string code)
        {
            var apiclient = new ApiClient.Common.ApiClient();
            var obj = apiclient.GetAirportByListCode(code);
            return obj;
        }
    }
}