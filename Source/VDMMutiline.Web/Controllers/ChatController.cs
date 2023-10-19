using VDMMutiline.Biz;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Params;
using System.Linq;
using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class ChatController : BaseController
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DefaultRoom()
        {
            MessageRoomBiz msgRoomBiz = new MessageRoomBiz();
            var defaultroom = msgRoomBiz.GetDefaultRoomInfo();
            var msgBiz = new MessageBiz();
            var msgPara = new MessageParam() { MessageFilter = new MessageFilter() { Id = defaultroom.Id }, PagingInfo = new VDMMutiline.SharedComponent.PagingInfo() { PageSize = 20 } };
            msgBiz.Search(msgPara);
            msgPara.MessageInfos = msgPara.MessageInfos.OrderBy(p => p.NgayGui).ToList();
            return View(msgPara);
        }
        public ActionResult PrivateRoom()
        {

            return View();
        }
    }
}