using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FareStatus
    {

        private bool statusField;

        private string remarkField;

        private double priceField;

        private double differenceField;

        private FareData fareDataField;

       
        public bool Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

       
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }

       
        public double Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

       
        public double Difference
        {
            get
            {
                return this.differenceField;
            }
            set
            {
                this.differenceField = value;
            }
        }

       
        public FareData FareData
        {
            get
            {
                return this.fareDataField;
            }
            set
            {
                this.fareDataField = value;
            }
        }
    }
}
