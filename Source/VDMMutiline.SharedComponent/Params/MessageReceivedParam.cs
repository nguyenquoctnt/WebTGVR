using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class MessageReceivedParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public MessageReceived MessageReceived { get; set; }

        public List<MessageReceived> MessageReceiveds { get; set; }

        public MessageReceivedInfo MessageReceivedInfo { get; set; }

        public List<MessageReceivedInfo> MessageReceivedInfos { get; set; }

        public MessageReceivedFilter MessageReceivedFilter { get; set; }
        #endregion
    }
    public class MessageReceivedFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}