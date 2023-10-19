using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class BK_BookingDetailDao:BaseDao
    {
        #region Action
        public int Insert(BK_BookingDetail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {

                dbContext.BK_BookingDetails.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.ID;
            }
        }
        
        public void Update(BK_BookingDetail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_BookingDetails.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {

                    dbItem.BookingID = item.BookingID;
                    dbItem.TicketID = item.TicketID;
                    dbItem.PassengerID = item.PassengerID;
                    dbItem.FlightNo = item.FlightNo;
                    dbItem.FromCity = item.FromCity;
                    dbItem.ToCity = item.ToCity;
                    dbItem.Handbag = item.Handbag;
                    dbItem.Regbag = item.Regbag;
                    dbItem.Price = item.Price;
                    dbItem.StartDate = item.StartDate;
                    dbItem.IsDeleted = item.IsDeleted;
                    dbItem.DeletedDate = item.DeletedDate;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(BK_BookingDetail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_BookingDetails.FirstOrDefault(en => en.ID == item.ID);

                if (dbItem != null)
                {


                    dbContext.BK_BookingDetails.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public BK_BookingDetail GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_BookingDetails
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public List<BK_BookingDetailInfo>  Getbylistbybooking(int bookingid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_BookingDetails
                            where n.BookingID == bookingid
                            select new BK_BookingDetailInfo()
                            {
                                ID=n.ID,
                                BookingID = n.BookingID,
                                TicketID = n.TicketID,
                                PassengerID = n.PassengerID,
                                FlightNo = n.FlightNo,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                Handbag = n.Handbag,
                                Regbag = n.Regbag,
                                Price = n.Price,
                                StartDate = n.StartDate,
                                IsDeleted = n.IsDeleted,
                                DeletedDate = n.DeletedDate,
                            };
                return query.ToList();
            }
        }
        public BK_BookingDetailInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_BookingDetails
                            where n.ID == id
                            select new BK_BookingDetailInfo()
                            {
                                BookingID = n.BookingID,
                                TicketID = n.TicketID,
                                PassengerID = n.PassengerID,
                                FlightNo = n.FlightNo,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                Handbag = n.Handbag,
                                Regbag = n.Regbag,
                                Price = n.Price,
                                StartDate = n.StartDate,
                                IsDeleted = n.IsDeleted,
                                DeletedDate = n.DeletedDate,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(BK_BookingDetailParam param)
        {
            if (param.BK_BookingDetailAnhFilter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var query = from n in dbContext.BK_BookingDetails
                                where n.IsDeleted == Constant.IsNotDeleted
                                select new BK_BookingDetailInfo
                                {

                                    BookingID = n.BookingID,
                                    TicketID = n.TicketID,
                                    PassengerID = n.PassengerID,
                                    FlightNo = n.FlightNo,
                                    FromCity = n.FromCity,
                                    ToCity = n.ToCity,
                                    Handbag = n.Handbag,
                                    Regbag = n.Regbag,
                                    Price = n.Price,
                                    StartDate = n.StartDate,
                                    IsDeleted = n.IsDeleted,
                                    DeletedDate = n.DeletedDate,
                                };
                    param.BK_BookingDetailInfos = query.ToList();
                }
            }
        }
        public void GetAll(BK_BookingDetailParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_BookingDetails
                            where n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingDetailInfo
                            {


                                BookingID = n.BookingID,
                                TicketID = n.TicketID,
                                PassengerID = n.PassengerID,
                                FlightNo = n.FlightNo,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                Handbag = n.Handbag,
                                Regbag = n.Regbag,
                                Price = n.Price,
                                StartDate = n.StartDate,
                                IsDeleted = n.IsDeleted,
                                DeletedDate = n.DeletedDate,

                            };
                param.BK_BookingDetailInfos = query.ToList();
            }
        }
        public List<BK_BookingDetailInfo> GetbylistbybookingjoinPassengers(int bookingid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_BookingDetails
                            join pg in dbContext.BK_Passengers on n.PassengerID equals pg.ID
                            join tk in dbContext.BK_Tickets on n.TicketID equals tk.ID
                            where n.BookingID == bookingid
                            select new BK_BookingDetailInfo()
                            {
                                ID = n.ID,
                                BookingID = n.BookingID,
                                TicketID = n.TicketID,
                                PassengerID = n.PassengerID,
                                FlightNo = n.FlightNo,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                Handbag = n.Handbag,
                                Regbag = n.Regbag,
                                Price = n.Price,
                                StartDate = n.StartDate,
                                IsDeleted = n.IsDeleted,
                                DeletedDate = n.DeletedDate,
                                typepg=pg.TypeID,
                                Namepg=pg.Name,
                                typeve= tk.TypeID
                            };
                return query.ToList();
            }
        }
        #endregion
    }
}
