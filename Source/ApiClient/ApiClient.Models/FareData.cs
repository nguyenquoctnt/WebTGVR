using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FareData
    {

        private int fareDataIdField;

        private string airlineField;

        private int itineraryField;

        private int legField;

        private bool promoField;

        private string currencyField;

        private string systemField;

        private int adtField;

        private int chdField;

        private int infField;

        private double fareAdtField;

        private double fareChdField;

        private double fareInfField;

        private double taxAdtField;

        private double taxChdField;

        private double taxInfField;

        private double feeAdtField;

        private double feeChdField;

        private double feeInfField;

        private double serviceFeeAdtField;

        private double serviceFeeChdField;

        private double serviceFeeInfField;

        private double totalNetPriceField;

        private double totalServiceFeeField;

        private double totalPriceField;

        private Flight[] listFlightField;

        private string sessionField;

        private bool autoIssueField;

        private bool hasChangedClassField;

        private System.DateTime lastTkDtField;

        private string[] listXmlRulesInfoField;

       
        public int FareDataId
        {
            get
            {
                return this.fareDataIdField;
            }
            set
            {
                this.fareDataIdField = value;
            }
        }

       
        public string Airline
        {
            get
            {
                return this.airlineField;
            }
            set
            {
                this.airlineField = value;
            }
        }

       
        public int Itinerary
        {
            get
            {
                return this.itineraryField;
            }
            set
            {
                this.itineraryField = value;
            }
        }

       
        public int Leg
        {
            get
            {
                return this.legField;
            }
            set
            {
                this.legField = value;
            }
        }

       
        public bool Promo
        {
            get
            {
                return this.promoField;
            }
            set
            {
                this.promoField = value;
            }
        }
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }
        public string System
        {
            get
            {
                return this.systemField;
            }
            set
            {
                this.systemField = value;
            }
        }
        public int Adt
        {
            get
            {
                return this.adtField;
            }
            set
            {
                this.adtField = value;
            }
        }
        public int Chd
        {
            get
            {
                return this.chdField;
            }
            set
            {
                this.chdField = value;
            }
        }
        public int Inf
        {
            get
            {
                return this.infField;
            }
            set
            {
                this.infField = value;
            }
        }
        public double FareAdt
        {
            get
            {
                return this.fareAdtField;
            }
            set
            {
                this.fareAdtField = value;
            }
        }
        public double FareChd
        {
            get
            {
                return this.fareChdField;
            }
            set
            {
                this.fareChdField = value;
            }
        }
        public double FareInf
        {
            get
            {
                return this.fareInfField;
            }
            set
            {
                this.fareInfField = value;
            }
        }
        public double TaxAdt
        {
            get
            {
                return this.taxAdtField;
            }
            set
            {
                this.taxAdtField = value;
            }
        }
        public double TaxChd
        {
            get
            {
                return this.taxChdField;
            }
            set
            {
                this.taxChdField = value;
            }
        }
        public double TaxInf
        {
            get
            {
                return this.taxInfField;
            }
            set
            {
                this.taxInfField = value;
            }
        }
        public double FeeAdt
        {
            get
            {
                return this.feeAdtField;
            }
            set
            {
                this.feeAdtField = value;
            }
        }
        public double FeeChd
        {
            get
            {
                return this.feeChdField;
            }
            set
            {
                this.feeChdField = value;
            }
        }
        public double FeeInf
        {
            get
            {
                return this.feeInfField;
            }
            set
            {
                this.feeInfField = value;
            }
        }
        public double ServiceFeeAdt
        {
            get
            {
                return this.serviceFeeAdtField;
            }
            set
            {
                this.serviceFeeAdtField = value;
            }
        }
        public double ServiceFeeChd
        {
            get
            {
                return this.serviceFeeChdField;
            }
            set
            {
                this.serviceFeeChdField = value;
            }
        }
        public double ServiceFeeInf
        {
            get
            {
                return this.serviceFeeInfField;
            }
            set
            {
                this.serviceFeeInfField = value;
            }
        }
        public double TotalNetPrice
        {
            get
            {
                return this.totalNetPriceField;
            }
            set
            {
                this.totalNetPriceField = value;
            }
        }
        public double TotalServiceFee
        {
            get
            {
                return this.totalServiceFeeField;
            }
            set
            {
                this.totalServiceFeeField = value;
            }
        }
        public double TotalPrice
        {
            get
            {
                return this.totalPriceField;
            }
            set
            {
                this.totalPriceField = value;
            }
        }
        public double TotalDiscount { get; set; }
        public double TotalCommission { get; set; }
        public Flight[] ListFlight
        {
            get
            {
                return this.listFlightField;
            }
            set
            {
                this.listFlightField = value;
            }
        }
        public string Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        public bool AutoIssue
        {
            get
            {
                return this.autoIssueField;
            }
            set
            {
                this.autoIssueField = value;
            }
        }
        public bool HasChangedClass
        {
            get
            {
                return this.hasChangedClassField;
            }
            set
            {
                this.hasChangedClassField = value;
            }
        }
        public System.DateTime LastTkDt
        {
            get
            {
                return this.lastTkDtField;
            }
            set
            {
                this.lastTkDtField = value;
            }
        }
        public string[] ListXmlRulesInfo
        {
            get
            {
                return this.listXmlRulesInfoField;
            }
            set
            {
                this.listXmlRulesInfoField = value;
            }
        }
    }
}
