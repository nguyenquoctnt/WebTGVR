using VDMMutiline.SharedComponent.Entities;
using static VDMMutiline.SharedComponent.Constants.Constant;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class EventLogInfo : EventLog
    {
        public LogLevel Level
        {
            get
            {
                return (LogLevel)LogLevel;
            }
        }
        public LogType TypeLog
        {
            get
            {
                return (LogType)LogType;
            }
        }
        public string LogTypeName
        {
            get
            {
                return TypeLog.ToString();
            }
        }
    }
}