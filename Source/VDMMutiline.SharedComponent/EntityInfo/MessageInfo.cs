using VDMMutiline.SharedComponent.Entities;
namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class MessageInfo: Message
    {
        public AspNetUserInfo Sender { get; set; }

        public double Ticks { get; set; }
    }
}