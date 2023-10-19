using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient.Models
{
    public class VDMBaggageInfo
    {
        public VDMBaggage[] BaggageDepart
        {
            get;set;
        }

        
        public VDMBaggage[] BaggageReturn
        {
            get; set;
        }

    }
}