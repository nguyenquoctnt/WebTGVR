using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class FareRules
    {

        private string routeField;

        private string fareBasisField;

        private RulesGroup[] listRulesGroupField;

       
        public string Route
        {
            get
            {
                return this.routeField;
            }
            set
            {
                this.routeField = value;
            }
        }

       
        public string FareBasis
        {
            get
            {
                return this.fareBasisField;
            }
            set
            {
                this.fareBasisField = value;
            }
        }

       
        public RulesGroup[] ListRulesGroup
        {
            get
            {
                return this.listRulesGroupField;
            }
            set
            {
                this.listRulesGroupField = value;
            }
        }
    }
}
