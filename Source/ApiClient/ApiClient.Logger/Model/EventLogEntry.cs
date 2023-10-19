using ApiClient.Logger.Enum;

namespace ApiClient.Logger.Model
{
    /// <summary>
    /// </summary>
    public class EventLogEntry
    {        
        public string Server { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public int ApplicationId { get; set; }
        public  string ApplicationName { get; set; }
        public string StackTrace { get; set; }
        public EvenTypes EventType { get; set; }
        public int UserId { get; set; }
    }
}
