using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class HtmlBlockBiz :BaseBiz
    {
        public void Insert(HtmlBlockParam param)
        {
            var dp = param.HtmlBlock;
            var dao = new HMTLBlockDao();
            dao.Insert(dp);
        }
        public void Update(HtmlBlockParam param)
        {
            var endep = param.HtmlBlock;
            var dao = new HMTLBlockDao();
            dao.Update(endep);
        }
        public void Delete(HtmlBlockParam param)
        {
            var dao = new HMTLBlockDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.HtmlBlocks;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(HtmlBlockParam param)
        {
            var dao = new HMTLBlockDao();
            param.HtmlBlock = dao.GetbyId(param.Id );

        }
        public void SetupEditForm(HtmlBlockParam param)
        {
            var dao = new HMTLBlockDao();
            param.HtmlBlock = dao.GetbyId(param.Id );
        }
        public void GetbyKey(HtmlBlockParam param)
        {
            var dao = new HMTLBlockDao();
            param.HtmlBlock = dao.GetbyKey(param.key);
        }
        public void Search(HtmlBlockParam param)
        {
            var dao = new HMTLBlockDao();
            dao.Search(param);
        }
    }
}
