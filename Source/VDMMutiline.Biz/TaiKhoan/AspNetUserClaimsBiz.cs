using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetUserClaimsBiz
    {
        public void Insert(AspNetUserClaimsParam param)
        {
            var dp = param.AspNetUserClaims;
           
            var dao = new AspNetUserClaimDao();
            dao.Insert(dp);
        }
        public void Update(AspNetUserClaimsParam param)
        {
            var endep = param.AspNetUserClaims;
            var dao = new AspNetUserClaimDao();
            dao.Update(endep);
        }
        public void Delete(AspNetUserClaimsParam param)
        {
            var dao = new AspNetUserClaimDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserClaimss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetUserClaimsParam param)
        {
            var dao = new AspNetUserClaimDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserClaimss;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetUserClaimsParam param)
        {
            var dao = new AspNetUserClaimDao();
            param.AspNetUserClaimsInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetUserClaimsParam param)
        {
            var dao = new AspNetUserClaimDao();
            param.AspNetUserClaims = dao.GetbyId(param.Id);
        }
        public void Search(AspNetUserClaimsParam param)
        {
            var dao = new AspNetUserClaimDao();
            dao.Search(param);
        }
        public void GetAll(AspNetUserClaimsParam param)
        {
            var dao = new AspNetUserClaimDao();
            dao.GetAll(param);
        }
    }
}
