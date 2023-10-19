using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BK_PassengerParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public int BookingID { get; set; }
        public int ticketID { get; set; }
        public int? Veid { get; set; }
        public BK_Passenger BK_Passenger { get; set; }
        public List<BK_Passenger> BK_Passengers { get; set; }
        public BK_PassengerInfo BK_PassengerInfo { get; set; }
        public List<BK_PassengerInfo> BK_PassengerInfos { get; set; }
        public BK_PassengerFilter BK_PassengerFilter { get; set; }
        #endregion
    }
    public class BK_PassengerFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? vitri { get; set; }
    }
}
