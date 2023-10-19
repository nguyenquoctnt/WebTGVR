using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetOnlineUsersParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetOnlineUser AspNetOnlineUsers { get; set; }

        public List<AspNetOnlineUser> AspNetOnlineUserss { get; set; }

        public AspNetOnlineUsersInfo AspNetOnlineUsersInfo { get; set; }

        public List<AspNetOnlineUsersInfo> AspNetOnlineUsersInfos { get; set; }

        public AspNetOnlineUsersFilter AspNetOnlineUsersFilter { get; set; }
        #endregion
    }
    public class AspNetOnlineUsersFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}