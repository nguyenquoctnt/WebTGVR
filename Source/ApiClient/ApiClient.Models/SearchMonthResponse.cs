using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public  class SearchMonthResponse : BaseResponse
    {
        private MinPrice[] listMinPriceField;

       
        public MinPrice[] ListMinPrice
        {
            get
            {
                return this.listMinPriceField;
            }
            set
            {
                this.listMinPriceField = value;
            }
        }
    }
}
