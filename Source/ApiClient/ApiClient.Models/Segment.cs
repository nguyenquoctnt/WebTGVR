using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class Segment
    {

        private int idField;

        private string airlineField;

        private string startPointField;

        private string endPointField;

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        private string flightNumberField;

        private int durationField;

        private string classField;

        private string planeField;

        private string startTerminalField;

        private string endTerminalField;

        private bool hasStopField;

        private string stopPointField;

        private double stopTimeField;

        private bool dayChangeField;

        private bool stopOvernightField;

        private bool changeStationField;

        private bool changeAirportField;

        private bool lastItemField;

        private string handBaggageField;

        /// <remarks/>
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        /// <remarks/>
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
        public string MarketingAirline { get; set; }
        public string OperatingAirline { get; set; }
        /// <remarks/>
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
        /// <remarks/>
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
        /// <remarks/>
        public System.DateTime StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }
        /// <remarks/>
        public System.DateTime EndTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
        public string StartTm { get; set; }
        public string EndTm { get; set; }
        /// <remarks/>
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
        /// <remarks/>
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
        /// <remarks/>
        public string Class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
            }
        }
        /// <remarks/>
        public string Plane
        {
            get
            {
                return this.planeField;
            }
            set
            {
                this.planeField = value;
            }
        }
        public int Seat { get; set; }
        /// <remarks/>
        public string StartTerminal
        {
            get
            {
                return this.startTerminalField;
            }
            set
            {
                this.startTerminalField = value;
            }
        }
        /// <remarks/>
        public string EndTerminal
        {
            get
            {
                return this.endTerminalField;
            }
            set
            {
                this.endTerminalField = value;
            }
        }
        /// <remarks/>
        public bool HasStop
        {
            get
            {
                return this.hasStopField;
            }
            set
            {
                this.hasStopField = value;
            }
        }
        /// <remarks/>
        public string StopPoint
        {
            get
            {
                return this.stopPointField;
            }
            set
            {
                this.stopPointField = value;
            }
        }
        /// <remarks/>
        public double StopTime
        {
            get
            {
                return this.stopTimeField;
            }
            set
            {
                this.stopTimeField = value;
            }
        }
        /// <remarks/>
        public bool DayChange
        {
            get
            {
                return this.dayChangeField;
            }
            set
            {
                this.dayChangeField = value;
            }
        }
        /// <remarks/>
        public bool StopOvernight
        {
            get
            {
                return this.stopOvernightField;
            }
            set
            {
                this.stopOvernightField = value;
            }
        }
        /// <remarks/>
        public bool ChangeStation
        {
            get
            {
                return this.changeStationField;
            }
            set
            {
                this.changeStationField = value;
            }
        }
        /// <remarks/>
        public bool ChangeAirport
        {
            get
            {
                return this.changeAirportField;
            }
            set
            {
                this.changeAirportField = value;
            }
        }
        /// <remarks/>
        public bool LastItem
        {
            get
            {
                return this.lastItemField;
            }
            set
            {
                this.lastItemField = value;
            }
        }
        /// <remarks/>
        public string HandBaggage
        {
            get
            {
                return this.handBaggageField;
            }
            set
            {
                this.handBaggageField = value;
            }
        }
        public string AllowanceBaggage { get; set; }
    }
}
