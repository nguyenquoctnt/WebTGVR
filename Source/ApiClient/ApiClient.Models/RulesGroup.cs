using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class RulesGroup
    {

        private string rulesTitleField;

        private string[] listRulesTextField;

       
        public string RulesTitle
        {
            get
            {
                return this.rulesTitleField;
            }
            set
            {
                this.rulesTitleField = value;
            }
        }

       
        public string[] ListRulesText
        {
            get
            {
                return this.listRulesTextField;
            }
            set
            {
                this.listRulesTextField = value;
            }
        }
    }
}
