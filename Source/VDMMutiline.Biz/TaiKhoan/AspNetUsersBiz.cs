using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.Biz
{
    public class AspNetUsersBiz
    {
        public void Insert(AspNetUsersParam param)
        {
            var dp = param.AspNetUsers;

            var dao = new AspNetUserDao();
            dao.Insert(dp);
        }
        public AspNetUserInfo GetInfoById(int? userID)
        {
            var dao = new AspNetUserDao();
            return dao.GetInfoById(userID);
        }
        
        public void Update(AspNetUsersParam param)
        {
            var endep = param.AspNetUsersInfo;
            var dao = new AspNetUserDao();
            dao.Update(endep);
        }
        public void UpdateEditdomain(AspNetUsersParam param)
        {
            var endep = param.AspNetUsersInfo;
            var dao = new AspNetUserDao();
            dao.UpdateEditdomain(endep);
        }
        
        public void UpdateSiteID(AspNetUsersParam param)
        {
            var endep = param.AspNetUsersInfo;
            var dao = new AspNetUserDao();
            dao.UpdateSiteID(endep);
        }
        public void Delete(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUsersInfos;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            param.AspNetUsersInfo = dao.GetInfo(param.Id);

        }
        public void GetInfobydomain(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            param.AspNetUsersInfo = dao.GetInfobydomain(param.UrlDomain);
        }
        public bool Checkdomain(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            if (dao.Checkdomain(param.UrlDomain) == null)
                return true;
            else return false;
        }
        
        public void SetupEditForm(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            param.AspNetUsersInfo = dao.GetInfo(param.AspNetUsersFilter.Id);
        }
        public void Search(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            dao.Search(param);
        }
        public void SearchListParam(AspNetUsersParam param)
        {
            var dao = new AspNetUserDao();
            dao.SearchListParam(param);
        }
        
      

        public void Lock(int byID,string domain)
        {
            var dao = new AspNetUserDao();
            dao.Lock(byID, domain);
        }
        public AspNetUser CheckCode(string username, string code)
        {
            var dao = new AspNetUserDao();
            return dao.CheckCode(username, code);
        }
        public AspNetUserInfo GetInfoByLoginName(string username)
        {
            var dao = new AspNetUserDao();
            return dao.GetInfoByLoginName(username);
        }
        public AspNetUserInfo GetInfoByEmail(string username)
        {
            var dao = new AspNetUserDao();
            return dao.GetInfoByEmail(username);
        }
        public bool Checkuser(string username)
        {
            var dao = new AspNetUserDao();
            return dao.Checkuser(username);
        }
        public bool CheckMail(string Email)
        {
            var dao = new AspNetUserDao();
            return dao.CheckMail(Email);
        }
        public bool CheckMail(string Email,string user)
        {
            var dao = new AspNetUserDao();
            return dao.CheckMail(Email, user);
        }
        public bool CheckPhone(string username)
        {
            var dao = new AspNetUserDao();
            return dao.CheckPhone(username);
        }
    }
}
