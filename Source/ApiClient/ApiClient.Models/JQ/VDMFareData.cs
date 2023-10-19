using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMFareData
    {
        public int FareDataId { get; set; }
        public string PlatingCarrier { get; set; }
        public int Itinerary { get; set; }
        public int Adt { get; set; }
        public int Chd { get; set; }
        public int Inf { get; set; }
        public double FareAdt { get; set; }
        public double FareChd
        {
            get; set;

        }
        public double FareInf
        {
            get; set;
        }
        public double TaxAdt
        {
            get; set;
        }
        public double TaxChd
        {
            get; set;
        }
        public double TaxInf
        {
            get; set;
        }
        public double FeeAdt
        {
            get; set;

        }
        public double FeeChd
        {
            get; set;
        }
        public double FeeInf
        {
            get; set;
        }
        public double ServiceFeeAdt
        {
            get; set;
        }
        public double ServiceFeeChd
        {
            get; set;
        }
        public double ServiceFeeInf
        {
            get; set;
        }
        public double TotalPrice
        {
            get; set;
        }
        public double TotalNetPrice
        {
            get; set;
        }
        public bool Promo
        {
            get; set;
        }
        public string Currency
        {
            get; set;
        }
        public string System
        {
            get; set;
        }
        public string Cookies
        {
            get; set;
        }
        public bool HasChangedClass
        {
            get; set;
        }
        public System.DateTime LastTkDt
        {
            get; set;
        }
        public VDMFlight[] ListDepartFlight
        {
            get; set;
        }
        public VDMFlight[] ListReturnFlight
        {
            get; set;
        }
        public string[] ListDepartureRulesInfo
        {
            get; set;
        }
        public string[] ListReturnRulesInfo
        {
            get; set;
        }
    }
}