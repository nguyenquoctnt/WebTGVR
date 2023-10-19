using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BaggageParam : BaseParam
    {
        public int Id { get; set; }
        public tblBaggage Baggage { get; set; }
        public List<tblBaggage> Baggages { get; set; }
        public BaggageInfo BaggageInfo { get; set; }
        public List<BaggageInfo> BaggageInfos { get; set; }
        public BaggageFilter BaggageFilter { get; set; }
    }
    public class BaggageFilter : BaseFilter
    {
        public int? Id { get; set; }
        public string Code { get; set; }
    }
}