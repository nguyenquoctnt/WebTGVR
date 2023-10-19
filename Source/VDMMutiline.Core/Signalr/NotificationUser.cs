using Microsoft.AspNet.SignalR;

namespace VDMMutiline.Core.Signalr
{
    public class NotificationUser
    {
        private string UserID;
        public NotificationUser(string userID)
        {
            UserID = userID;
        }

        /// <summary>
        /// Them 1 ham notify cho 1 hien tuong nao do
        /// </summary>
        public void NotifyUser(string subject, string body, string url)
        {
            if (UserID != null)
            {
                var notificationUser = GlobalHost.ConnectionManager.GetHubContext<UserNotififyHub>();
                notificationUser.Clients.Group(UserID).notifyUser(subject, body, url);
            }
        }

        public void NotifyUserLogout()
        {
            if (UserID != null)
            {
                var notificationUser = GlobalHost.ConnectionManager.GetHubContext<UserNotififyHub>();
                notificationUser.Clients.Group(UserID).notifyUserLogout();
            }
        }
    }
}