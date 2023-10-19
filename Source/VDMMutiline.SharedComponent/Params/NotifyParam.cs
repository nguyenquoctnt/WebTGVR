using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using System.Collections.Generic;

namespace VDMMutiline.SharedComponent.Params
{
    public class NotifyParam : BaseParam
    {
        public NotifyFilter NotifyFilter { get; set; }
        public List<NotificationInfo> Notifications { get; set; }

        public Notification Notification { get; set; }
        public NotificationInfo NotificationInfo { get; set; }
    }
    public class NotifyFilter : BaseFilter
    {
        public int? UserId { get; set; }
    }
}