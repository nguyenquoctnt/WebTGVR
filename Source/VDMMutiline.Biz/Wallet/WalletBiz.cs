using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.Dao.Wallet;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Params.Wallet;

namespace VDMMutiline.Biz.Wallet
{
    public class WalletBiz : BaseBiz
    {
        WalletDao dao = new WalletDao();
        public void Update(WalletParam param)
        {
            dao.Update(param.WalletHistory, param.UserID, param.UserIDSource,param.IsAdmin);
        }
        public void GetbyId(WalletParam param)
        {
            param.Wallet = dao.GetbyId(param.IdWallet);
        }
        public void GetInfo(WalletParam param)
        {
            param.WalletInfo = dao.GetInfo(param.UserID);
        }
        public void SearchWallet(WalletParam param)
        {
            dao.SearchWallet(param);
        }
        public void SearchHistorys(WalletParam param)
        {
            dao.SearchHistorys(param);
        }
        public bool CheckKyQuy(int? IdUser, decimal? Amount)
        {
            return dao.CheckKyQuy(IdUser, Amount);
        }
    }
}
