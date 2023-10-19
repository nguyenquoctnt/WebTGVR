using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace VDMMutiline.Core.Signalr
{
    public class UserNotififyHub : Hub
    {
        public override Task OnConnected()
        {
            string UserID = Context.QueryString["UserID"];
            if (!string.IsNullOrEmpty(UserID))
            {
                Groups.Add(Context.ConnectionId, UserID);
            }
            return base.OnConnected();
        }
    }
}
