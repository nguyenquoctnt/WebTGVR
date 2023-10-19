using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FareRuleResponse : BaseResponse
    {
        private FareRules[] listFareRulesField;
        public FareRules[] ListFareRules
        {
            get
            {
                return this.listFareRulesField;
            }
            set
            {
                this.listFareRulesField = value;
            }
        }
    }

}
