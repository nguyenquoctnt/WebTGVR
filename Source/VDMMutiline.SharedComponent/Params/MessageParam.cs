using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class MessageParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public Message Message { get; set; }

        public List<Message> Messages { get; set; }

        public MessageInfo MessageInfo { get; set; }

        public List<MessageInfo> MessageInfos { get; set; }

        public MessageFilter MessageFilter { get; set; }
        #endregion
    }
    public class MessageFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}