using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class VerifyResponse : BaseResponse
    {

        private FareStatus[] listFareStatusField;

       
        public FareStatus[] ListFareStatus
        {
            get
            {
                return this.listFareStatusField;
            }
            set
            {
                this.listFareStatusField = value;
            }
        }
    }
}
