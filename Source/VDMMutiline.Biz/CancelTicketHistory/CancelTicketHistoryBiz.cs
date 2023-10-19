using VDMMutiline.Dao.VoidTicket;
using VDMMutiline.SharedComponent.Params.CancelTicket;
namespace VDMMutiline.Biz.VoidTicketHistory
{
    public class CancelTicketHistoryBiz : BaseBiz
    {
        public void Insert(CancelTicketHistoryParam param)
        {
            var dp = param.CancelTicketHistory;
            var dao = new CancelTicketHistorysDao();
            dao.Insert(dp, param.isAdmin);
        }
        public void Search(CancelTicketHistoryParam param)
        {
            var dao = new CancelTicketHistorysDao();
            dao.Search(param);
        }

    }
}
