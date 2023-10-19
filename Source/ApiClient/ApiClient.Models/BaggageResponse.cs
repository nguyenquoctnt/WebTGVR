using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class BaggageResponse : BaseResponse
    {

        private Baggage[] listBaggageField;

       
        public Baggage[] ListBaggage
        {
            get
            {
                return this.listBaggageField;
            }
            set
            {
                this.listBaggageField = value;
            }
        }
    }
}
