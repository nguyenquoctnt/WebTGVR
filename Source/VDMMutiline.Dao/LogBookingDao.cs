using System;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
namespace VDMMutiline.Dao
{
    public class LogBookingDao : BaseDao
    {
        #region Action
        public void Insert(tbl_LogBooking item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_LogBookings.InsertOnSubmit(item);
                dbContext.SubmitChanges();

            }
        }
        #endregion
        #region Query
        public tbl_LogBooking GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_LogBookings
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public LogBookingInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_LogBookings
                            where n.Id == id
                            select new LogBookingInfo()
                            {
                                Id = n.Id,
                                Idbooking = n.Idbooking,
                                TypeAction = n.TypeAction,
                                Note = n.Note,
                                Bookingcode = n.Bookingcode,
                                Madonhang = n.Madonhang,
                                Ngaytao = n.Ngaytao,
                                Nguoitao = n.Nguoitao,
                                Noidungcu = n.Noidungcu,
                                Noidungmoi = n.Noidungmoi,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(LogBookingParam param)
        {
            var filter = param.LogBookingFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_LogBookings
                            where (filter.Fromdate.HasValue == false || n.Ngaytao >= filter.Fromdate)
                            && (filter.Fromdate.HasValue == false || n.Ngaytao <= filter.Todate)
                            && (string.IsNullOrEmpty(filter.User) || n.Nguoitao == filter.User)
                            && (filter.BookingID.HasValue == false || n.Idbooking == filter.BookingID)
                            && (string.IsNullOrEmpty(filter.Bookcode) || (n.Bookingcode == filter.Bookcode || n.Madonhang == filter.Bookcode))
                            select new LogBookingInfo
                            {
                                Id = n.Id,
                                Idbooking = n.Idbooking,
                                TypeAction = n.TypeAction,
                                Note = n.Note,
                                Bookingcode = n.Bookingcode,
                                Madonhang = n.Madonhang,
                                Ngaytao = n.Ngaytao,
                                Nguoitao = n.Nguoitao,
                                Noidungcu = n.Noidungcu,
                                Noidungmoi = n.Noidungmoi,

                            };
                param.LogBookingInfos = query.OrderByDescending(z => z.Id).ToList();
            }
        }
        #endregion
    }
}

