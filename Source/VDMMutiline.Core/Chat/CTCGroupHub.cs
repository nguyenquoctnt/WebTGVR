using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Entities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Core.Chat
{
    [HubName("ctcGroupHub")]
    public class CTCGroupHub : Hub
    {
        private AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
        private MessageRoomBiz msgRoomBiz = new MessageRoomBiz();
        private MessageBiz msgBiz = new MessageBiz();

        [HubMethodName("broadCastMessage")]
        public void BroadCastMessage(String msgFrom, String msg)
        {
            var defaultRoom = msgRoomBiz.GetDefaultRoomInfo();
            var senderUser = aspnetUsersBiz.GetInfoByLoginName(msgFrom);
            var id = Context.ConnectionId;
            string[] Exceptional = new string[0];

            // insert to db

            Message msginsert = new Message()
            {
                RoomId = defaultRoom.Id,
                ContentChat = msg,
                IdNguoigui = senderUser.Id,
                TenNguoigui = senderUser.DisplayName,
                NgayGui = DateTime.Now,
                Daxoa = false,
                IP = Context.Request.GetHttpContext().Request.ServerVariables["REMOTE_ADDR"],
                Agent = Context.Request.GetHttpContext().Request.UserAgent
            };
            msgBiz.Insert(msginsert);
            // send to client
            Clients.Group(defaultRoom.Tenhom, Exceptional).receiveMessage(msgFrom, senderUser.DisplayName ?? msgFrom, senderUser.Avatar ?? "/content/images/avatars/avatar2.png", 0, msg);//Utility.Gettoaltimestring(DateTime.Now), msg);
        }

        [HubMethodName("groupconnect")]
        public void Get_Connect(String username, String connectionid)
        {
            var defaultRoom = msgRoomBiz.GetDefaultRoomInfo();
            var id = Context.ConnectionId;
            Groups.Add(id, defaultRoom.Tenhom);

        }
        [HubMethodName("defaultGroupconnect")]
        public void Get_DefaultConnect(String username)
        {
            var defaultRoom = msgRoomBiz.GetDefaultRoomInfo();

            var id = Context.ConnectionId;
            Groups.Add(id, defaultRoom.Tenhom);
        }
        //public override System.Threading.Tasks.Task OnConnected()
        //{
        //    return base.OnConnected();
        //}

        //public override System.Threading.Tasks.Task OnReconnected()
        //{
        //    return base.OnReconnected();
        //}

        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    return base.OnDisconnected(stopCalled);
        //}
    }
}
