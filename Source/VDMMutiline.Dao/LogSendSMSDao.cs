
using System;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;


namespace VDMMutiline.Dao
{
    public class LogSendSMSDao : BaseDao
    {
        #region Action
        public void Insert(tbl_LogSendSM item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_LogSendSMs.InsertOnSubmit(item);
                dbContext.SubmitChanges();

            }
        }
        #endregion
        #region Query
        public tbl_LogSendSM GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_LogSendSMs
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public LogSendSMSInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_LogSendSMs
                            where n.ID == id
                            select new LogSendSMSInfo()
                            {
                                ID = n.ID,
                                ID_Booking = n.ID_Booking,
                                Bookingcode = n.Bookingcode,
                                UserSend = n.UserSend,
                                NoiDung = n.NoiDung,
                                CreateDate = n.CreateDate,
                                Phonesend = n.Phonesend,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(LogSendSMSParam param)
        {
            var filter = param.LogSendSMSFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_LogSendSMs
                            where (filter.Fromdate.HasValue == false || n.CreateDate.Value.Date >= filter.Fromdate)
                            && (filter.Todate.HasValue == false || n.CreateDate.Value.Date <= filter.Todate)

                            select new LogSendSMSInfo
                            {
                                ID = n.ID,
                                ID_Booking = n.ID_Booking,
                                Bookingcode = n.Bookingcode,
                                UserSend = n.UserSend,
                                NoiDung = n.NoiDung,
                                CreateDate = n.CreateDate,
                                Phonesend = n.Phonesend,
                            };
                param.LogSendSMSInfos = query.OrderByDescending(z => z.ID).ToList();
            }
        }
        #endregion
    }
}

