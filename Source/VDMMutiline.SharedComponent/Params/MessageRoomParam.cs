using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class MessageRoomParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public MessageRoom MessageRoom { get; set; }

        public List<MessageRoom> MessageRooms { get; set; }

        public MessageRoomInfo MessageRoomInfo { get; set; }

        public List<MessageRoomInfo> MessageRoomInfos { get; set; }

        public MessageRoomFilter MessageRoomFilter { get; set; }
        #endregion
    }
    public class MessageRoomFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}