using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class EventLogParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public EventLog EventLog { get; set; }

        public List<EventLog> EventLogs { get; set; }

        public EventLogInfo EventLogInfo { get; set; }

        public List<EventLogInfo> EventLogInfos { get; set; }

        public EventLogFilter EventLogFilter { get; set; }
        #endregion
    }
    public class EventLogFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}