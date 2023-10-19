using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class MinPrice
    {

        private System.DateTime departDateField;

        private FareData[] listFareDataField;

       
        public System.DateTime DepartDate
        {
            get
            {
                return this.departDateField;
            }
            set
            {
                this.departDateField = value;
            }
        }

       
        public FareData[] ListFareData
        {
            get
            {
                return this.listFareDataField;
            }
            set
            {
                this.listFareDataField = value;
            }
        }
    }
}
