using System;
using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Dao
{
    public class BK_TicketDao : BaseDao
    {
        #region Action
        public int Insert(BK_Ticket item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {

                dbContext.BK_Tickets.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.ID;
            }
        }
        public int InsertDetali(BK_TicketDetail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {

                dbContext.BK_TicketDetails.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.ID;
            }
        }
        public void Update(BK_Ticket item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Tickets.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {

                    dbItem.TypeID = item.TypeID;
                    dbItem.FirmID = item.FirmID;
                    dbItem.FlightNo = item.FlightNo;
                    dbItem.StartDate = item.StartDate;
                    dbItem.EndDate = item.EndDate;
                    dbItem.Price = item.Price;
                    dbItem.TypeID = item.TypeID;
                    dbItem.IsDeleted = item.IsDeleted;
                    dbItem.FromCity = item.FromCity;
                    dbItem.ToCity = item.ToCity;
                    dbItem.DeletedDate = item.DeletedDate;
                    dbItem.HoldToDate = item.HoldToDate;
                    dbItem.PriceBefore = item.PriceBefore;
                    dbItem.Tax = item.Tax;
                    dbItem.Class = item.Class;
                    dbItem.ClassName = item.ClassName;
                    dbItem.Code = item.Code;
                    dbItem.Transit = item.Transit;
                    dbItem.TransitInfo = item.TransitInfo;
                    dbContext.SubmitChanges();
                }
            }
        }
        public void Updatengaygio(BK_Ticket item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_TicketDetails.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {
                    var itemch = dbContext.BK_Tickets.FirstOrDefault(z => z.ID == dbItem.TicketID);
                    if (itemch != null)
                    {
                        if (itemch.StartDate == dbItem.StartDate)
                            itemch.StartDate = item.StartDate;
                        if (itemch.EndDate == dbItem.EndDate)
                            itemch.EndDate = item.EndDate;
                        if (itemch.FromCity == dbItem.FromCity)
                            itemch.FromCity = item.FromCity;
                        if (itemch.ToCity == dbItem.ToCity)
                            itemch.ToCity = item.ToCity;
                        if (itemch.Code == dbItem.airlineCode)
                            itemch.Code = item.Code;
                        if (itemch.FlightNo == dbItem.flight)
                            itemch.FlightNo = item.FlightNo;
                        if (itemch.Class == dbItem.Class)
                            itemch.Class = item.Class;
                        dbContext.SubmitChanges();
                        try
                        {
                            var objbooking = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == itemch.BookingID);
                            if (objbooking != null)
                            {
                                if (itemch.TypeID == Constant.Typeve.LuotVe)
                                {
                                    objbooking.FlightReturn = item.FlightNo;
                                }
                                else
                                {
                                    objbooking.FlightDepart = item.FlightNo;
                                }
                            }
                        }
                        catch
                        {

                        }


                    }
                    dbItem.StartDate = item.StartDate;
                    dbItem.EndDate = item.EndDate;
                    dbItem.FromCity = item.FromCity;
                    dbItem.ToCity = item.ToCity;
                    dbItem.airlineCode = item.Code;
                    dbItem.flight = item.FlightNo;
                    dbItem.Class = item.Class;
                    dbContext.SubmitChanges();


                }
            }
        }
        public void UpdateGia(int ticket, double sotien)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Tickets.FirstOrDefault(sitem => sitem.ID == ticket);
                if (dbItem != null)
                {
                    var giabefo = dbItem.Price;
                    dbItem.Price = sotien;
                    dbContext.SubmitChanges();
                    var sotiencongthem = sotien - giabefo;
                    var lstDetail = dbContext.BK_BookingDetails.Where(z => z.BookingID == dbItem.BookingID && z.TicketID == dbItem.ID);
                    var sotiencongthem1ve = Math.Round(((sotiencongthem / lstDetail.Count()) ?? 0), 0);
                    foreach (var item in lstDetail)
                    {
                        var detail = dbContext.BK_BookingDetails.FirstOrDefault(z => z.ID == item.ID);
                        if (detail != null)
                        {
                            detail.Price = (detail.Price + sotiencongthem1ve);
                            dbContext.SubmitChanges();
                        }
                    }
                    var getticketbybooking = dbContext.BK_Tickets.Where(z => z.BookingID == dbItem.BookingID);
                    double? tongtien = 0;
                    tongtien = getticketbybooking.Sum(z => z.Price);

                    var hanhkhachdao = new BK_PassengerDao();
                    var hanhkhachparam = new BK_PassengerParam() { BookingID = dbItem.BookingID };
                    hanhkhachdao.GetbyBooking(hanhkhachparam);
                    decimal tongienhanhly = 0;
                    foreach (var item in hanhkhachparam.BK_PassengerInfos.Where(z => z.Typeve == Constant.Typeve.LuotDi))
                    {
                        tongienhanhly += (item.BaggageDepartPrice ?? 0) + (item.BaggageReturnPrice ?? 0);
                    }

                    var objbking = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == dbItem.BookingID);
                    if (objbking != null)
                    {
                        if (Station.CheckQuocTe(objbking.FromCity, objbking.ToCity))
                            tongtien = sotien;
                        objbking.TotalPrice = ((double)tongienhanhly) + tongtien -
                                              (double)
                                                  (objbking.DiscountPrice.HasValue ? objbking.DiscountPrice.Value : 0);
                        objbking.OutputFee = (decimal)(tongtien.HasValue ? tongtien.Value : 0);
                    }
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(BK_Ticket item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Tickets.FirstOrDefault(en => en.ID == item.ID);

                if (dbItem != null)
                {


                    dbContext.BK_Tickets.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public BK_Ticket GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Tickets
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public BK_TicketDetail GetTicketDetailbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_TicketDetails
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public List<BK_TicketInfo> GetlistbybookingId(int bookingid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            where item.BookingID == bookingid
                            select new BK_TicketInfo()
                            {
                                ID = item.ID,
                                TypeID = item.TypeID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                Promo = item.Promo,
                            };
                return query.ToList();
            }
        }
        public BK_TicketInfo GetInfoByBooking(int bookingid, string FlightNo)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            where item.BookingID == bookingid
                            && item.FlightNo == FlightNo
                            select new BK_TicketInfo()
                            {
                                TypeID = item.TypeID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                Promo = item.Promo,
                            };
                var list = query.ToList();
                foreach (var item in list)
                {
                    if (item.Code != "VJ" && item.Code != "JQ")
                        item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                }
                return list.FirstOrDefault();
            }
        }
        public List<BK_TicketInfo> GetListByBooking(int bookingid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            where item.BookingID == bookingid
                            select new BK_TicketInfo()
                            {
                                ID = item.ID,
                                TypeID = item.TypeID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                DaGuiMaiNhac = item.DaGuiMaiNhac,
                                IP = item.IP,
                                FareAdt = item.FareAdt,
                                FareChd = item.FareChd,
                                FareInf = item.FareInf,
                                FeeAdt = item.FeeAdt,
                                FeeChd = item.FeeChd,
                                FeeInf = item.FeeInf,
                                TaxAdt = item.TaxAdt,
                                TaxChd = item.TaxChd,
                                TaxInf = item.TaxInf,
                                Duration = item.Duration,
                                Promo = item.Promo,
                            };
                var list = query.ToList();
                foreach (var item in list)
                {
                    if (item.Code != "VJ" && item.Code != "JQ")
                        item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                }
                return list;
            }
        }
        public List<BK_TicketInfo> GetListByBookingCode(string CodeBook)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            where item.CodeBook == CodeBook
                            select new BK_TicketInfo()
                            {
                                ID = item.ID,
                                TypeID = item.TypeID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                DaGuiMaiNhac = item.DaGuiMaiNhac,
                                IP = item.IP,
                                FareAdt = item.FareAdt,
                                FareChd = item.FareChd,
                                FareInf = item.FareInf,
                                FeeAdt = item.FeeAdt,
                                FeeChd = item.FeeChd,
                                FeeInf = item.FeeInf,
                                TaxAdt = item.TaxAdt,
                                TaxChd = item.TaxChd,
                                TaxInf = item.TaxInf,
                                Duration = item.Duration,
                                Promo=item.Promo,
                            };
                var list = query.ToList();
                foreach (var item in list)
                {
                    if (item.Code != "VJ" && item.Code != "JQ")
                        item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                }
                return list;
            }
        }
        public List<BK_TicketDetail> GetDetailByTicket(int ticketid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_TicketDetails
                            where item.TicketID == ticketid
                            select item;
                var list = query.ToList();
                foreach (var item in list)
                {
                    if (item.airlineCode != "VJ" && item.airlineCode != "JQ")
                        item.flight = item.flight.Contains(item.airlineCode) ? item.flight : item.airlineCode + item.flight;
                }
                return list;
            }
        }
        public List<BK_TicketDetailInfo> GetDetailBybooking(int bookingid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_TicketDetails
                            join tk in dbContext.BK_Tickets on item.TicketID equals tk.ID
                            where tk.BookingID == bookingid
                            select new BK_TicketDetailInfo
                            {
                                ID = item.ID,
                                airlineCode = item.airlineCode,
                                flight = item.flight,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                Class = item.Class,
                                TicketID = item.TicketID,
                                Plane = item.Plane,
                                Duration = item.Duration,
                                StopTime = item.StopTime,
                            };
                var list = query.ToList();
                return list;
            }
        }
        public BK_TicketInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            where item.ID == id
                            select new BK_TicketInfo()
                            {
                                TypeID = item.TypeID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                FareAdt = item.FareAdt,
                                FareChd = item.FareChd,
                                FareInf = item.FareInf,
                                FeeAdt = item.FeeAdt,
                                FeeChd = item.FeeChd,
                                FeeInf = item.FeeInf,
                                TaxAdt = item.TaxAdt,
                                TaxChd = item.TaxChd,
                                TaxInf = item.TaxInf,
                                Duration = item.Duration,

                            };
                var list = query.ToList();
                foreach (var item in list)
                {
                    if (item.Code != "VJ" && item.Code != "JQ")
                        item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                }
                return list.FirstOrDefault();
            }
        }
        public void Search(BK_TicketParamParam param)
        {
            if (param.BK_TicketFilter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var query = from item in dbContext.BK_Tickets
                                where item.IsDeleted == Constant.IsNotDeleted
                                select new BK_TicketInfo
                                {

                                    BookingID = item.BookingID,
                                    FirmID = item.FirmID,
                                    FlightNo = item.FlightNo,
                                    StartDate = item.StartDate,
                                    EndDate = item.EndDate,
                                    Price = item.Price,
                                    TypeID = item.TypeID,
                                    IsDeleted = item.IsDeleted,
                                    FromCity = item.FromCity,
                                    ToCity = item.ToCity,
                                    DeletedDate = item.DeletedDate,
                                    HoldToDate = item.HoldToDate,
                                    PriceBefore = item.PriceBefore,
                                    Tax = item.Tax,
                                    Class = item.Class,
                                    ClassName = item.ClassName,
                                    Code = item.Code,
                                    Transit = item.Transit,
                                    TransitInfo = item.TransitInfo,
                                    CodeBook = item.CodeBook,
                                    Exdate = item.Exdate,
                                    FareAdt = item.FareAdt,
                                    FareChd = item.FareChd,
                                    FareInf = item.FareInf,
                                    FeeAdt = item.FeeAdt,
                                    FeeChd = item.FeeChd,
                                    FeeInf = item.FeeInf,
                                    TaxAdt = item.TaxAdt,
                                    TaxChd = item.TaxChd,
                                    TaxInf = item.TaxInf,
                                    Duration = item.Duration,
                                };
                    var list = query.ToList();
                    foreach (var item in list)
                    {
                        if (item.Code != "VJ" && item.Code != "JQ")
                            item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                    }
                    param.BK_TicketInfos = list;
                }
            }
        }
        public void GetAll(BK_TicketParamParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            where item.IsDeleted == Constant.IsNotDeleted
                            select new BK_TicketInfo
                            {

                                BookingID = item.BookingID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                TypeID = item.TypeID,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                FareAdt = item.FareAdt,
                                FareChd = item.FareChd,
                                FareInf = item.FareInf,
                                FeeAdt = item.FeeAdt,
                                FeeChd = item.FeeChd,
                                FeeInf = item.FeeInf,
                                TaxAdt = item.TaxAdt,
                                TaxChd = item.TaxChd,
                                TaxInf = item.TaxInf,
                                Duration = item.Duration,

                            };
                var list = query.ToList();
                foreach (var item in list)
                {
                    if (item.Code != "VJ" && item.Code != "JQ")
                        item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                }
                param.BK_TicketInfos = list;
            }
        }
        public List<BK_TicketInfo> GetAlltake200(BK_TicketParamParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Tickets
                            join bk in dbContext.BK_Bookings on item.BookingID equals bk.ID
                            where item.IsDeleted == Constant.IsNotDeleted
                              && (param.Fromdate.HasValue == false || bk.CreatedDate.Value.Date >= param.Fromdate)
                                && (param.Todate.HasValue == false || bk.CreatedDate.Value.Date <= param.Todate)
                                 && (param.ListUserName.Count == 0 || param.ListUserName.Contains(bk.UserCreate))
                            select new BK_TicketInfo
                            {
                                ID = item.ID,
                                BookingID = item.BookingID,
                                FirmID = item.FirmID,
                                FlightNo = item.FlightNo,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Price = item.Price,
                                TypeID = item.TypeID,
                                IsDeleted = item.IsDeleted,
                                FromCity = item.FromCity,
                                ToCity = item.ToCity,
                                DeletedDate = item.DeletedDate,
                                HoldToDate = item.HoldToDate,
                                PriceBefore = item.PriceBefore,
                                Tax = item.Tax,
                                Class = item.Class,
                                ClassName = item.ClassName,
                                Code = item.Code,
                                Transit = item.Transit,
                                TransitInfo = item.TransitInfo,
                                CodeBook = item.CodeBook,
                                Exdate = item.Exdate,
                                Eror = item.Eror,
                                FareAdt = item.FareAdt,
                                FareChd = item.FareChd,
                                FareInf = item.FareInf,
                                FeeAdt = item.FeeAdt,
                                FeeChd = item.FeeChd,
                                FeeInf = item.FeeInf,
                                TaxAdt = item.TaxAdt,
                                TaxChd = item.TaxChd,
                                TaxInf = item.TaxInf,
                                Duration = item.Duration,
                                CreateUser = item.CreateUser

                            };

                if (param.PagingInfo != null)
                {
                    var infopage = param.PagingInfo;
                    param.PagingInfo.RecordCount = query.Count();
                    var list = query.OrderByDescending(z => z.ID).Skip(infopage.RowsSkip).Take(infopage.PageSize).ToList();
                    foreach (var item in list)
                    {
                        if (item.Code != "VJ" && item.Code != "JQ")
                            item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                    }
                    return list;
                }
                else
                {
                    var list = query.OrderByDescending(z => z.ID).ToList();
                    foreach (var item in list)
                    {
                        if (item.Code != "VJ" && item.Code != "JQ")
                            item.FlightNo = item.FlightNo.Contains(item.Code) ? item.FlightNo : item.Code + item.FlightNo;
                    }
                    return list;

                }

            }
        }
        #endregion
        public void xoa(int idbooking, int idve)
        {
            var list = GetListByBooking(idbooking);

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var objdetail = dbContext.BK_TicketDetails.FirstOrDefault(z => z.ID == idve);
                if (objdetail != null)
                {
                    var objidve = dbContext.BK_Tickets.FirstOrDefault(z => z.ID == objdetail.TicketID);
                    var listdetail = dbContext.BK_TicketDetails.Where(z => z.TicketID == objdetail.TicketID);
                    if (listdetail.Count() > 1)
                    {
                        dbContext.BK_TicketDetails.DeleteOnSubmit(objdetail);
                        dbContext.SubmitChanges();
                    }
                    else
                    {
                        if (objidve != null)
                        {
                            dbContext.BK_Tickets.DeleteOnSubmit(objidve);
                            dbContext.SubmitChanges();
                            var bkupdate = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == idbooking);
                            if (bkupdate != null)
                            {
                                bkupdate.TotalPrice = bkupdate.TotalPrice - objidve.Price;
                                string code = bkupdate.BookCode;
                                var FailCode = bkupdate.FailCode;
                                if (FailCode.Length > 0)
                                {
                                    if (FailCode.Split('-').Count() > 1)
                                    {
                                        if (string.IsNullOrEmpty(FailCode))
                                            FailCode = "";
                                        if (string.IsNullOrEmpty(objidve.CodeBook))
                                            FailCode = FailCode.Replace("NA", "").Replace("-", "");
                                        else FailCode = "";
                                        bkupdate.FailCode = FailCode;
                                    }
                                }
                                if (code.Length > 0)
                                {
                                    if (code.Split('-').Count() > 1)
                                    {
                                        if (!string.IsNullOrEmpty(objidve.CodeBook))
                                        {
                                            code = code.Replace(objidve.CodeBook, "").Replace("-", "");
                                            bkupdate.BookCode = code;
                                        }
                                    }
                                }
                                dbContext.SubmitChanges();
                            }
                        }
                    }
                }
                foreach (var bk in list)
                {
                    var obj = dbContext.BK_Tickets.FirstOrDefault(z => z.ID == bk.ID);
                    if (obj != null)
                    {
                        obj.TypeID = Constant.Typeve.LuotDi;
                        dbContext.SubmitChanges();
                    }
                }
            }
        }



    }
}
