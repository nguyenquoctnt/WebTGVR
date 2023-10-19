using Microsoft.AspNet.SignalR;

namespace VDMMutiline.Core.Signalr
{
    public class NotificationAllUser
    {
        public NotificationAllUser()
        {
        }

        /// <summary>
        /// Them 1 ham notify cho 1 hien tuong nao do
        /// </summary>
        public void NotifyAllUser(string subject, string body, string url)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UserNotififyHub>();
            context.Clients.All.notifyAllUser(subject, body, url);
        }
    }
}