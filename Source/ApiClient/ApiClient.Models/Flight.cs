using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class Flight
    {

        private int flightIdField;
        private int legField;

        private string airlineField;

        private string startPointField;

        private string endPointField;

        private System.DateTime startDateField;

        private System.DateTime endDateField;

        private string flightNumberField;

        private int stopNumField;

        private bool hasDownStopField;

        private int durationField;

        private bool noRefundField;

        private string groupClassField;

        private string fareClassField;

        private bool promoField;

        private string flightValueField;

        private Segment[] listSegmentField;

        private string defaultField;

        private double totalPriceField;

        private double baseFareField;

        private double priceAdtField;

        private string selectValueField;
        public int FlightId
        {
            get
            {
                return this.flightIdField;
            }
            set
            {
                this.flightIdField = value;
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
        public string Operating { get; set; }
        public string StartPoint
        {
            get
            {
                return this.startPointField;
            }
            set
            {
                this.startPointField = value;
            }
        }
        public string EndPoint
        {
            get
            {
                return this.endPointField;
            }
            set
            {
                this.endPointField = value;
            }
        }
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
        public string StartDt { get; set; }
        public string EndDt { get; set; }
        public string FlightNumber
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = value;
            }
        }
        public int StopNum
        {
            get
            {
                return this.stopNumField;
            }
            set
            {
                this.stopNumField = value;
            }
        }
        public bool HasDownStop
        {
            get
            {
                return this.hasDownStopField;
            }
            set
            {
                this.hasDownStopField = value;
            }
        }
        public int Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }
        public bool NoRefund
        {
            get
            {
                return this.noRefundField;
            }
            set
            {
                this.noRefundField = value;
            }
        }
        public string GroupClass
        {
            get
            {
                return this.groupClassField;
            }
            set
            {
                this.groupClassField = value;
            }
        }
        public string FareClass
        {
            get
            {
                return this.fareClassField;
            }
            set
            {
                this.fareClassField = value;
            }
        }
        public string FareBasis { get; set; }
        public int ? SeatRemain { get; set; }
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
        public string FlightValue
        {
            get
            {
                return this.flightValueField;
            }
            set
            {
                this.flightValueField = value;
            }
        }
        public Segment[] ListSegment
        {
            get
            {
                return this.listSegmentField;
            }
            set
            {
                this.listSegmentField = value;
            }
        }
        public string Default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
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
        public double BaseFare
        {
            get
            {
                return this.baseFareField;
            }
            set
            {
                this.baseFareField = value;
            }
        }
        public double PriceAdt
        {
            get
            {
                return this.priceAdtField;
            }
            set
            {
                this.priceAdtField = value;
            }
        }
        public string SelectValue
        {
            get
            {
                return this.selectValueField;
            }
            set
            {
                this.selectValueField = value;
            }
        }
    }

}
