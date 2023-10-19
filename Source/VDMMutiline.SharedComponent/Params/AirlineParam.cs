using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class AirlineParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_airline Airline { get; set; }
        public List<tbl_airline> Airlines { get; set; }
        public AirlineInfo AirlineInfo { get; set; }
        public List<AirlineInfo> AirlineInfos { get; set; }
        public AirlineFilter AirlineFilter { get; set; }
        #endregion
    }
    public class AirlineFilter : BaseFilter
    {
        public int? Id { get; set; }
        public string Code { get; set; }
    }
}
