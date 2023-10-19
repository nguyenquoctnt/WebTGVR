using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Dao
{
    public class BK_PassengerDao : BaseDao
    {
        #region Action
        public int Insert(BK_Passenger item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {

                dbContext.BK_Passengers.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.ID;
            }
        }

        public int InsertNewPassengerBackEnd(BK_Passenger item, int BookingID, double? TotalPriceDepart, double? TotalPriceReturn)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        item.IsDeleted = Constant.IsNotDeleted;
                        item.CreatedDate = DateTime.Now;
                        dbContext.BK_Passengers.InsertOnSubmit(item);
                        dbContext.SubmitChanges();
                        var idpass = item.ID;
                        var lstticket = dbContext.BK_Tickets.Where(z => z.BookingID == BookingID).ToList();
                        double toangtienve = 0;
                        foreach (var itemticket in lstticket)
                        {
                            var ckBookingDetail = new BK_BookingDetail();
                            ckBookingDetail.FromCity = itemticket.FromCity;
                            ckBookingDetail.ToCity = itemticket.ToCity;
                            ckBookingDetail.BookingID = BookingID;
                            ckBookingDetail.TicketID = itemticket.ID;
                            ckBookingDetail.PassengerID = idpass;
                            ckBookingDetail.FlightNo = itemticket.FlightNo;
                            ckBookingDetail.StartDate = itemticket.StartDate;
                            if (itemticket.TypeID == Constant.Typeve.LuotVe)
                            {
                                ckBookingDetail.Price = TotalPriceReturn;
                            }
                            else
                            {
                                ckBookingDetail.Price = TotalPriceDepart ;
                            }
                            toangtienve += (ckBookingDetail.Price ?? 0);
                            dbContext.BK_BookingDetails.InsertOnSubmit(ckBookingDetail);
                            dbContext.SubmitChanges();
                        }
                        var objbooking = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == BookingID);
                        objbooking.TotalPrice = (objbooking.TotalPrice + toangtienve + ((double)(item.BaggageDepartPrice ?? 0)) + ((double)(item.BaggageReturnPrice ?? 0)));
                        dbContext.SubmitChanges();
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                return item.ID;
            }
        }

        public void Update(BK_Passenger item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Passengers.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {

                    dbItem.TypeID = item.TypeID;
                    dbItem.Name = item.Name;
                    dbItem.Birthday = item.Birthday;
                    dbItem.Sex = item.Sex;
                    dbItem.FirstName = item.FirstName;
                    dbItem.Address = item.Address;
                    dbItem.City = item.City;
                    dbItem.TK_CountryID = item.TK_CountryID;
                    dbItem.TK_CountryName = item.TK_CountryName;
                    dbItem.Email = item.Email;
                    dbItem.Phone = item.Phone;
                    dbItem.PassportNo = item.PassportNo;
                    dbItem.PassportCountryID = item.PassportCountryID;
                    dbItem.PassportCountryName = item.PassportCountryName;
                    dbItem.PassportExp = item.PassportExp;
                    dbItem.NationnalityID = item.NationnalityID;
                    dbItem.BaggageDepartID = item.BaggageDepartID;
                    dbItem.BaggageReturnID = item.BaggageReturnID;
                    dbItem.BaggageDepartValue = item.BaggageDepartValue;
                    dbItem.BaggageReturnValue = item.BaggageReturnValue;
                    dbItem.BaggageDepartPrice = item.BaggageDepartPrice;
                    dbItem.BaggageReturnPrice = item.BaggageReturnPrice;
                    dbContext.SubmitChanges();
                }
            }
        }
        public void updateTen(int id, string ho, string ten, string BaggageDepartValue, decimal? BaggageDepartPrice,
            string BaggageReturnValue, decimal? BaggageReturnPrice, string sex, int? bookingid, DateTime? birthday)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Passengers.FirstOrDefault(sitem => sitem.ID == id);
                if (dbItem != null)
                {
                    var tongtienhanhlyold = (dbItem.BaggageDepartPrice ?? 0) + (dbItem.BaggageReturnPrice ?? 0);
                    dbItem.Name = ten;
                    dbItem.FirstName = ho;
                    dbItem.BaggageDepartValue = BaggageDepartValue;
                    dbItem.BaggageDepartPrice = BaggageDepartPrice;
                    dbItem.BaggageReturnValue = BaggageReturnValue;
                    dbItem.BaggageReturnPrice = BaggageReturnPrice;
                    dbItem.Birthday = birthday;
                    dbItem.Sex = sex;
                    dbContext.SubmitChanges();
                    var objbking = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == bookingid);
                    if (objbking != null)
                    {
                        objbking.TotalPrice = ((objbking.TotalPrice ?? 0) - (double)tongtienhanhlyold) + (double)(dbItem.BaggageDepartPrice ?? 0) + (double)(dbItem.BaggageReturnPrice ?? 0);
                        dbContext.SubmitChanges();
                    }
                }
            }
        }

        public bool Delete(BK_Passenger item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Passengers.FirstOrDefault(en => en.ID == item.ID);

                if (dbItem != null)
                {
                    dbContext.BK_Passengers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }

        public bool DeleteById(int id, int bookingid)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Passengers.FirstOrDefault(en => en.ID == id);
                if (dbItem != null)
                {
                    var hanhlydi =
                        Utility.ConvertTodouble(dbItem.BaggageDepartPrice.HasValue
                            ? dbItem.BaggageDepartPrice.Value.ToString()
                            : "0");
                    var hanhlyve =
                        Utility.ConvertTodouble(dbItem.BaggageReturnPrice.HasValue ? dbItem.BaggageReturnPrice.Value.ToString() : "0");

                    var getticketbybooking = dbContext.BK_BookingDetails.Where(z => z.PassengerID == dbItem.ID);
                    var tongtien = getticketbybooking.Sum(z => z.Price);
                    var objbking = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == bookingid);
                    if (objbking != null)
                        objbking.TotalPrice = (objbking.TotalPrice.HasValue ? objbking.TotalPrice.Value : 0)
                            - (tongtien.HasValue ? tongtien.Value : 0)
                            - (hanhlydi.HasValue ? hanhlydi.Value : 0)
                            - (hanhlyve.HasValue ? hanhlyve.Value : 0);
                    dbContext.BK_Passengers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }


        #endregion
        #region Query
        public BK_Passenger GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Passengers
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public List<BK_PassengerInfo> GetbyBookingId(int BookingId)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Passengers
                            join BookingDetails in dbContext.BK_BookingDetails on item.ID equals BookingDetails.PassengerID
                            where item.IsDeleted == Constant.IsNotDeleted
                            && BookingDetails.BookingID == BookingId
                            select new BK_PassengerInfo
                            {
                                ID = item.ID,
                                TypeID = item.TypeID,
                                Name = item.Name,
                                Birthday = item.Birthday,
                                Sex = item.Sex,
                                FirstName = item.FirstName,
                                FullName = item.FirstName + " " + item.Name,
                                Address = item.Address,
                                City = item.City,
                                TK_CountryID = item.TK_CountryID,
                                TK_CountryName = item.TK_CountryName,
                                Email = item.Email,
                                Phone = item.Phone,
                                PassportNo = item.PassportNo,
                                PassportCountryID = item.PassportCountryID,
                                PassportCountryName = item.PassportCountryName,
                                PassportExp = item.PassportExp,
                                NationnalityID = item.NationnalityID,
                                BaggageDepartID = item.BaggageDepartID,
                                BaggageReturnID = item.BaggageReturnID,
                                BaggageDepartValue = item.BaggageDepartValue,
                                BaggageReturnValue = item.BaggageReturnValue,
                                BaggageDepartPrice = item.BaggageDepartPrice,
                                BaggageReturnPrice = item.BaggageReturnPrice,
                                CreatedDate = item.CreatedDate,
                                IsDeleted = item.IsDeleted,
                                DeletedDate = item.DeletedDate,
                                Price = BookingDetails.Price.HasValue ? BookingDetails.Price.Value : 0,
                            };
                var list = new List<BK_PassengerInfo>();
                foreach (var item in query.ToList())
                {
                    var objcheck = list.FirstOrDefault(z => z.ID == item.ID);
                    if (objcheck == null)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }
        public BK_PassengerInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Passengers
                            where item.ID == id
                            select new BK_PassengerInfo()
                            {
                                TypeID = item.TypeID,
                                Name = item.Name,
                                Birthday = item.Birthday,
                                Sex = item.Sex,
                                FirstName = item.FirstName,
                                Address = item.Address,
                                City = item.City,
                                TK_CountryID = item.TK_CountryID,
                                TK_CountryName = item.TK_CountryName,
                                Email = item.Email,
                                Phone = item.Phone,
                                PassportNo = item.PassportNo,
                                PassportCountryID = item.PassportCountryID,
                                PassportCountryName = item.PassportCountryName,
                                PassportExp = item.PassportExp,
                                NationnalityID = item.NationnalityID,
                                BaggageDepartID = item.BaggageDepartID,
                                BaggageReturnID = item.BaggageReturnID,
                                BaggageDepartValue = item.BaggageDepartValue,
                                BaggageReturnValue = item.BaggageReturnValue,
                                BaggageDepartPrice = item.BaggageDepartPrice,
                                CreatedDate = item.CreatedDate,
                                IsDeleted = item.IsDeleted,
                                DeletedDate = item.DeletedDate,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(BK_PassengerParam param)
        {
            if (param.BK_PassengerFilter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var query = from item in dbContext.BK_Passengers
                                where item.IsDeleted == Constant.IsNotDeleted
                                select new BK_PassengerInfo
                                {

                                    TypeID = item.TypeID,
                                    Name = item.Name,
                                    Birthday = item.Birthday,
                                    Sex = item.Sex,
                                    FirstName = item.FirstName,
                                    Address = item.Address,
                                    City = item.City,
                                    TK_CountryID = item.TK_CountryID,
                                    TK_CountryName = item.TK_CountryName,
                                    Email = item.Email,
                                    Phone = item.Phone,
                                    PassportNo = item.PassportNo,
                                    PassportCountryID = item.PassportCountryID,
                                    PassportCountryName = item.PassportCountryName,
                                    PassportExp = item.PassportExp,
                                    NationnalityID = item.NationnalityID,
                                    BaggageDepartID = item.BaggageDepartID,
                                    BaggageReturnID = item.BaggageReturnID,
                                    BaggageDepartValue = item.BaggageDepartValue,
                                    BaggageReturnValue = item.BaggageReturnValue,
                                    BaggageDepartPrice = item.BaggageDepartPrice,
                                    CreatedDate = item.CreatedDate,
                                    IsDeleted = item.IsDeleted,
                                    DeletedDate = item.DeletedDate,
                                };
                    param.BK_PassengerInfos = query.ToList();
                }
            }
        }
        public void GetAll(BK_PassengerParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Passengers
                            where item.IsDeleted == Constant.IsNotDeleted
                            select new BK_PassengerInfo
                            {
                                TypeID = item.TypeID,
                                Name = item.Name,
                                Birthday = item.Birthday,
                                Sex = item.Sex,
                                FirstName = item.FirstName,
                                Address = item.Address,
                                City = item.City,
                                TK_CountryID = item.TK_CountryID,
                                TK_CountryName = item.TK_CountryName,
                                Email = item.Email,
                                Phone = item.Phone,
                                PassportNo = item.PassportNo,
                                PassportCountryID = item.PassportCountryID,
                                PassportCountryName = item.PassportCountryName,
                                PassportExp = item.PassportExp,
                                NationnalityID = item.NationnalityID,
                                BaggageDepartID = item.BaggageDepartID,
                                BaggageReturnID = item.BaggageReturnID,
                                BaggageDepartValue = item.BaggageDepartValue,
                                BaggageReturnValue = item.BaggageReturnValue,
                                BaggageDepartPrice = item.BaggageDepartPrice,
                                CreatedDate = item.CreatedDate,
                                IsDeleted = item.IsDeleted,
                                DeletedDate = item.DeletedDate,

                            };
                param.BK_PassengerInfos = query.ToList();
            }
        }
        public void GetbyBooking(BK_PassengerParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Passengers
                            join BookingDetails in dbContext.BK_BookingDetails on item.ID equals BookingDetails.PassengerID
                            join ckbk in dbContext.BK_Tickets on BookingDetails.TicketID equals ckbk.ID
                            where item.IsDeleted == Constant.IsNotDeleted
                            && BookingDetails.BookingID == param.BookingID
                            && (param.Veid.HasValue == false || BookingDetails.TicketID == param.Veid)
                            select new BK_PassengerInfo
                            {
                                ID = item.ID,
                                FlightNo = ckbk.FlightNo,
                                FromCity = ckbk.FromCity,
                                ToCity = ckbk.ToCity,
                                StartDate = ckbk.StartDate,
                                TypeID = item.TypeID,
                                Name = item.Name,
                                Birthday = item.Birthday,
                                Sex = item.Sex,
                                FirstName = item.FirstName,
                                Address = item.Address,
                                City = item.City,
                                TK_CountryID = item.TK_CountryID,
                                TK_CountryName = item.TK_CountryName,
                                Email = item.Email,
                                Phone = item.Phone,
                                PassportNo = item.PassportNo,
                                PassportCountryID = item.PassportCountryID,
                                PassportCountryName = item.PassportCountryName,
                                PassportExp = item.PassportExp,
                                NationnalityID = item.NationnalityID,
                                BaggageDepartID = item.BaggageDepartID,
                                BaggageReturnID = item.BaggageReturnID,
                                BaggageDepartValue = item.BaggageDepartValue,
                                BaggageReturnValue = item.BaggageReturnValue,
                                BaggageDepartPrice = item.BaggageDepartPrice,
                                BaggageReturnPrice = item.BaggageReturnPrice,
                                CreatedDate = item.CreatedDate,
                                IsDeleted = item.IsDeleted,
                                DeletedDate = item.DeletedDate,
                                Price = BookingDetails.Price.HasValue ? BookingDetails.Price.Value : 0,
                                codedi = ckbk.TypeID == Constant.Typeve.LuotDi ? ckbk.Code : "",
                                Typeve = ckbk.TypeID
                            };
                param.BK_PassengerInfos = query.ToList();
            }
        }
        public void Getbyticket(BK_PassengerParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Passengers
                            join BookingDetails in dbContext.BK_BookingDetails on item.ID equals BookingDetails.PassengerID
                            join ckbk in dbContext.BK_Tickets on BookingDetails.TicketID equals ckbk.ID
                            where item.IsDeleted == Constant.IsNotDeleted
                            && BookingDetails.TicketID == param.ticketID
                            && (param.Veid.HasValue == false || BookingDetails.TicketID == param.Veid)
                            select new BK_PassengerInfo
                            {
                                ID = item.ID,
                                FlightNo = ckbk.FlightNo,
                                FromCity = ckbk.FromCity,
                                ToCity = ckbk.ToCity,
                                StartDate = ckbk.StartDate,
                                TypeID = item.TypeID,
                                Name = item.Name,
                                Birthday = item.Birthday,
                                Sex = item.Sex,
                                FirstName = item.FirstName,
                                Address = item.Address,
                                City = item.City,
                                TK_CountryID = item.TK_CountryID,
                                TK_CountryName = item.TK_CountryName,
                                Email = item.Email,
                                Phone = item.Phone,
                                PassportNo = item.PassportNo,
                                PassportCountryID = item.PassportCountryID,
                                PassportCountryName = item.PassportCountryName,
                                PassportExp = item.PassportExp,
                                NationnalityID = item.NationnalityID,
                                BaggageDepartID = item.BaggageDepartID,
                                BaggageReturnID = item.BaggageReturnID,
                                BaggageDepartValue = item.BaggageDepartValue,
                                BaggageReturnValue = item.BaggageReturnValue,
                                BaggageDepartPrice = item.BaggageDepartPrice,
                                BaggageReturnPrice = item.BaggageReturnPrice,
                                CreatedDate = item.CreatedDate,
                                IsDeleted = item.IsDeleted,
                                DeletedDate = item.DeletedDate,
                                Price = BookingDetails.Price.HasValue ? BookingDetails.Price.Value : 0,
                                codedi = ckbk.TypeID == Constant.Typeve.LuotDi ? ckbk.Code : "",
                                Typeve = ckbk.TypeID
                            };
                param.BK_PassengerInfos = query.ToList();
            }
        }
        #endregion
    }
}
