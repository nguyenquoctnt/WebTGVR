using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class VerifyRequest : BaseRequest {
        
        private FareDataInfo[] listFareDataField;
        public FareDataInfo[] ListFareData {
            get {
                return this.listFareDataField;
            }
            set {
                this.listFareDataField = value;
            }
        }
    }
}
