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
    public class BK_BookingDao : BaseDao
    {
        #region Action
        public int Insert(BK_Booking item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                item.CreatedDate = DateTime.Now;
                item.IsDeleted = false;
                dbContext.BK_Bookings.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                var obj = dbContext.BK_Bookings.FirstOrDefault(z => z.ID == item.ID);
                if (obj != null)
                {
                    obj.rCode = TaoMaDonhang.CreateMaThongTin(item.ID);
                    dbContext.SubmitChanges();
                }
                return item.ID;
            }
        }
        public void Update(BK_Booking item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {

                    dbItem.SessionID = item.SessionID;
                    dbItem.Status = item.Status;
                    dbItem.TotalPrice = item.TotalPrice;
                    dbItem.FromCity = item.FromCity;
                    dbItem.ToCity = item.ToCity;
                    dbItem.FlightDepart = item.FlightDepart;
                    dbItem.FlightReturn = item.FlightReturn;
                    dbItem.Ways = item.Ways;
                    dbItem.Amount = item.Amount;
                    dbItem.Name = item.Name;
                    dbItem.Phone = item.Phone;
                    dbItem.Address = item.Address;
                    dbItem.Email = item.Email;
                    dbItem.Note = item.Note;
                    dbItem.IP = item.IP;
                    dbItem.CreatedDate = item.CreatedDate;
                    dbItem.UpdatedUser = item.UpdatedUser;
                    dbItem.UpdatedDate = item.UpdatedDate;
                    dbItem.DeletedUser = item.DeletedUser;
                    dbItem.DeletedDate = item.DeletedDate;
                    dbItem.rTotal = item.rTotal;
                    dbItem.rCode = item.rCode;
                    dbItem.UserId = item.UserId;
                    dbItem.OutputFee = item.OutputFee;
                    dbItem.PaymentTypeID = item.PaymentTypeID;
                    dbItem.PaymentTypeName = item.PaymentTypeName;
                    dbItem.HoldToDate = item.HoldToDate;
                    dbItem.ConfirmUserId = item.ConfirmUserId;
                    dbItem.ConfirmDate = item.ConfirmDate;
                    dbItem.FlightInfo = item.FlightInfo;
                    dbItem.DiscountCode = item.DiscountCode;
                    dbItem.DiscountPrice = item.DiscountPrice;
                    dbItem.FromCityName = item.FromCityName;
                    dbItem.ToCityName = item.ToCityName;
                    dbItem.IsPaid = item.IsPaid;
                    dbItem.PaidDate = item.PaidDate;


                    dbContext.SubmitChanges();
                }
            }
        }
        public void Updatestatusbycode(string code, int status)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.BookCode == code);
                if (dbItem != null)
                {
                    dbItem.Status = status;
                    dbContext.SubmitChanges();
                }
            }
        }
        public void UpdateCode(BK_Booking item, decimal TongTienHanhLy)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {
                    dbItem.BookCode = item.BookCode;
                    dbItem.TotalPrice += Utility.ConvertTodouble(TongTienHanhLy.ToString());
                    dbContext.SubmitChanges();
                }
            }
        }
        public BK_Booking UpdateTrangThai(int? id, int? Status, string code, string taikhoan, DateTime? Expireddate, DateTime? datetimeold)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.ID == id);
                if (dbItem != null)
                {
                    if (code.Contains("-NA") && code.Length <= 11)
                        code = "";
                    dbItem.BookCode = code;
                    dbItem.Status = Status;
                    dbItem.UserId = taikhoan;
                    dbItem.Expireddate = Expireddate;
                    if (datetimeold.HasValue == false || datetimeold < DateTime.Now.AddMinutes(-3))
                        dbItem.Sendmailgannhat = DateTime.Now;
                    //dbItem.Tiensendmailganhat = dbItem.TotalPrice;
                    if (Status == Constant.StatusVe.DaXuatVe)
                        dbItem.HoldToDate = DateTime.Now;
                    dbContext.SubmitChanges();
                    if (string.IsNullOrEmpty(code))
                        code = "";
                    var codes = code.Split('-');
                    var codedi = codes.Count() > 0 ? codes[0] : "";
                    var codeve = codes.Count() > 1 ? codes[1] : "";
                    if (codes.Count() == 1)
                    {
                        codeve = codedi;
                    }
                    var listve = dbContext.BK_Tickets.Where(z => z.BookingID == id);
                    foreach (var item in listve)
                    {
                        var objve = dbContext.BK_Tickets.FirstOrDefault(z => z.ID == item.ID);
                        if (objve != null)
                        {
                            if (objve.TypeID == Constant.Typeve.LuotDi)
                                objve.CodeBook = codedi;
                            else objve.CodeBook = codeve;
                            dbContext.SubmitChanges();
                        }
                    }
                    return dbItem;
                }
                return null;
            }
        }
        public BK_Booking UpdateXuatHoaDon(int? id, string taikhoang, string Hotenguoixuly, bool daxuathoadon, DateTime? Ngayxuat)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.ID == id);
                if (dbItem != null)
                {
                    dbItem.Daxuathoadon = daxuathoadon;
                    dbItem.Nguoixuly = taikhoang;
                    dbItem.Hotenguoixuly = Hotenguoixuly;
                    dbItem.Ngayxuly = Ngayxuat;
                    dbContext.SubmitChanges();
                    return dbItem;
                }
                return null;
            }
        }

        public void Updateghichu(int? id, string note)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.ID == id);
                if (dbItem != null)
                {
                    dbItem.Note = note;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(BK_Booking item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(en => en.ID == item.ID);

                if (dbItem != null)
                {
                    dbItem.IsDeleted = true;
                    dbItem.DeletedUser = item.DeletedUser;
                    dbItem.DeletedDate = item.DeletedDate;
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public void UpdateSdt(int id, string sdt)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Bookings.FirstOrDefault(sitem => sitem.ID == id);
                if (dbItem != null)
                {
                    dbItem.Phone = sdt;
                    dbContext.SubmitChanges();
                }
            }
        }
        #endregion
        #region Query
        public BK_Booking GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where n.ID == id && n.IsDeleted == Constant.IsNotDeleted
                            select n;
                return query.FirstOrDefault();
            }
        }
        public BK_BookingInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where n.ID == id && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo()
                            {
                                ID = n.ID,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                BookCode = n.BookCode,
                                Expireddate = n.Expireddate,


                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public BK_BookingInfo GetInfo(string Bookingcode)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where n.BookCode.Trim().ToUpper() == Bookingcode.Trim().ToUpper()
                            || n.rCode.Trim().ToUpper() == Bookingcode.Trim().ToUpper()
                            && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo()
                            {
                                ID = n.ID,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                BookCode = n.BookCode,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public BK_BookingInfo GetInfoByCodeFormTicket(string Bookingcode)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            join ticket in dbContext.BK_Tickets on n.ID equals ticket.BookingID
                            where ticket.CodeBook.Trim().ToUpper() == Bookingcode.Trim().ToUpper()
                            && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo()
                            {
                                ID = n.ID,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                BookCode = n.BookCode,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public BK_BookingInfo GetInfoALL(string Bookingcode)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where (n.BookCode.Trim().ToUpper() == Bookingcode.Trim().ToUpper() || n.rCode == Bookingcode.Trim().ToUpper())
                            && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo()
                            {
                                ID = n.ID,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                BookCode = n.BookCode,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public BK_BookingInfo GetDetail(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where n.ID == id && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo
                            {
                                ID = n.ID,
                                BookCode = n.BookCode,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                IsDeleted = n.IsDeleted,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public BK_BookingInfo GetDetail(string code)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where (n.BookCode == code || n.rCode == code || n.FailCode == code) && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo
                            {
                                ID = n.ID,
                                BookCode = n.BookCode,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                IsDeleted = n.IsDeleted,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(BK_BookingParam param)
        {
            var fiter = param.BookingFilter;
            if (fiter != null)
            {

                using (var dbContext = new ContextDataContext(ConnectionString))
                {

                    var lstTaiKhoan = new List<string>();
                    if (!string.IsNullOrEmpty(fiter.TaiKhoan))
                    {
                        lstTaiKhoan.Add(fiter.TaiKhoan);
                        var objuser = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == fiter.TaiKhoan);
                        if (objuser != null)
                        {
                            var querytk = dbContext.AspNetUsers.Where(z => z.ParentId == objuser.Id);
                            lstTaiKhoan.AddRange(querytk.Select(z => z.UserName).ToList());
                        }

                    }
                    var query = from n in dbContext.BK_Bookings
                                where n.IsDeleted == Constant.IsNotDeleted
                                && (
                                string.IsNullOrEmpty(fiter.Search)
                                || (
                                   n.Phone.Contains(fiter.Search)
                                || n.Name.Contains(fiter.Search)
                                || n.Email.Contains(fiter.Search)
                                || ((n.BookCode.Contains(fiter.Search) || n.FailCode.Contains(fiter.Search)))
                                || n.rCode.Contains(fiter.Search)))
                                && (fiter.Fromdate.HasValue == false || n.CreatedDate.Value.Date >= fiter.Fromdate)
                                && (fiter.Todate.HasValue == false || n.CreatedDate.Value.Date <= fiter.Todate)
                                && (fiter.StatusVe.HasValue == false || n.Status == fiter.StatusVe)

                                 && (lstTaiKhoan.Count() == 0 || lstTaiKhoan.Contains(n.UserId))
                                  && (fiter.Vehethangiucho.HasValue == false || n.Expireddate > DateTime.Now)
                                     && (fiter.DatVeThatBai.HasValue == false || n.BookCode.Trim() == "")
                                      && (fiter.ListUserName.Count == 0 || fiter.ListUserName.Contains(n.UserCreate))
                                select new BK_BookingInfo
                                {
                                    ID = n.ID,
                                    BookCode = n.BookCode,
                                    SessionID = n.SessionID,
                                    Status = n.Status,
                                    TotalPrice = n.TotalPrice,
                                    FromCity = n.FromCity,
                                    ToCity = n.ToCity,
                                    FlightDepart = n.FlightDepart,
                                    FlightReturn = n.FlightReturn,
                                    Ways = n.Ways,
                                    Amount = n.Amount,
                                    Name = n.Name,
                                    Phone = n.Phone,
                                    Address = n.Address,
                                    Email = n.Email,
                                    Note = n.Note,
                                    IP = n.IP,
                                    CreatedDate = n.CreatedDate,
                                    UpdatedUser = n.UpdatedUser,
                                    UpdatedDate = n.UpdatedDate,
                                    DeletedUser = n.DeletedUser,
                                    DeletedDate = n.DeletedDate,
                                    IsDeleted = n.IsDeleted,
                                    rTotal = n.rTotal,
                                    rCode = n.rCode,
                                    UserId = n.UserId,
                                    OutputFee = n.OutputFee,
                                    PaymentTypeID = n.PaymentTypeID,
                                    PaymentTypeName = n.PaymentTypeName,
                                    HoldToDate = n.HoldToDate,
                                    ConfirmUserId = n.ConfirmUserId,
                                    ConfirmDate = n.ConfirmDate,
                                    FlightInfo = n.FlightInfo,
                                    DiscountCode = n.DiscountCode,
                                    DiscountPrice = n.DiscountPrice,
                                    FromCityName = n.FromCityName,
                                    ToCityName = n.ToCityName,
                                    IsPaid = n.IsPaid,
                                    PaidDate = n.PaidDate,
                                    Expireddate = n.Expireddate,
                                    xhdtencongty = n.xhdtencongty,
                                    xhddiachi = n.xhddiachi,
                                    xhdthanhpho = n.xhdthanhpho,
                                    xhdnguoinhanhd = n.xhdnguoinhanhd,
                                    xhdvat = n.xhdvat,
                                    xhdktencongty = n.xhdktencongty,
                                    xhdkdiachi = n.xhdkdiachi,
                                    xhdktp = n.xhdktp,
                                    xhdknguoinhanhd = n.xhdknguoinhanhd,
                                    Sendmailgannhat = n.Sendmailgannhat,
                                    Tiensendmailganhat = n.Tiensendmailganhat,
                                    NguoiDaxem = n.NguoiDaxem,
                                    PaymentInfo = dbContext.tbl_PaymentInfos.FirstOrDefault(z => z.IdBooking == n.ID),
                                    Daxuathoadon = n.Daxuathoadon,
                                    Nguoixuly = n.Nguoixuly,
                                    Hotenguoixuly = n.Hotenguoixuly,
                                    UserCreate = n.UserCreate,
                                    SourceSite = n.SourceSite,
                                    FailCode = n.FailCode,
                                };
                    if (param.PagingInfo != null)
                    {
                        var infopage = param.PagingInfo;
                        param.PagingInfo.RecordCount = query.Count();
                        if (fiter.Vehethangiucho == true)
                        {
                            param.BookingInfos =
                                query.OrderBy(z => z.Expireddate).Skip(infopage.RowsSkip).Take(infopage.PageSize).ToList().ToList();
                        }
                        else
                        {
                            param.BookingInfos =
                   query.OrderByDescending(z => z.CreatedDate).Skip(infopage.RowsSkip).Take(infopage.PageSize).ToList().ToList();

                        }
                    }
                    else
                    {
                        param.BookingInfos = query.OrderByDescending(z => z.CreatedDate).ToList();

                    }
                }
            }
        }

        public void SearchCount(BK_BookingParam param)
        {
            var fiter = param.BookingFilter;
            if (fiter != null)
            {

                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var lstTaiKhoan = new List<string>();
                    if (!string.IsNullOrEmpty(fiter.TaiKhoan))
                    {
                        lstTaiKhoan.Add(fiter.TaiKhoan);
                        var objuser = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == fiter.TaiKhoan);
                        if (objuser != null)
                        {
                            var querytk = dbContext.AspNetUsers.Where(z => z.ParentId == objuser.Id);
                            lstTaiKhoan.AddRange(querytk.Select(z => z.UserName).ToList());
                        }
                    }
                    var query = from n in dbContext.BK_Bookings
                                join detail in dbContext.BK_BookingDetails on n.ID equals detail.BookingID
                                join ticket in dbContext.BK_Tickets on detail.TicketID equals ticket.ID
                                join pass in dbContext.BK_Passengers on detail.PassengerID equals pass.ID
                                where n.IsDeleted == Constant.IsNotDeleted
                                && pass.TypeID != Constant.TypePassenger.EmBe
                                && (
                                string.IsNullOrEmpty(fiter.Search)
                                || (
                                   n.Phone.Contains(fiter.Search)
                                || n.Name.Contains(fiter.Search)
                                || n.Email.Contains(fiter.Search)
                                || ((n.BookCode.Contains(fiter.Search) || n.FailCode.Contains(fiter.Search)))
                                || n.rCode.Contains(fiter.Search)))
                                && (fiter.Fromdate.HasValue == false || n.CreatedDate.Value.Date >= fiter.Fromdate)
                                && (fiter.Todate.HasValue == false || n.CreatedDate.Value.Date <= fiter.Todate)
                                && (fiter.StatusVe.HasValue == false || n.Status == fiter.StatusVe)
                                 && (lstTaiKhoan.Count() == 0 || lstTaiKhoan.Contains(n.UserId))
                                  && (fiter.Vehethangiucho.HasValue == false || n.Expireddate > DateTime.Now)
                                     && (fiter.DatVeThatBai.HasValue == false || n.BookCode.Trim() == "")
                                      && (fiter.ListUserName.Count == 0 || fiter.ListUserName.Contains(n.UserCreate))
                                select new 
                                {
                                   detail.ID,
                                   ticket.Code
                                };

                    var list = new List<SharedComponent.EntityInfo.Ticket.TicketCountInfo>();
                    list.Add(new SharedComponent.EntityInfo.Ticket.TicketCountInfo { AirlineCode = "VN", Count = query.Count(z => z.Code == "VN") });
                    list.Add(new SharedComponent.EntityInfo.Ticket.TicketCountInfo { AirlineCode = "VJ", Count = query.Count(z => z.Code == "VJ") });
                    list.Add(new SharedComponent.EntityInfo.Ticket.TicketCountInfo { AirlineCode = "JQ", Count = query.Count(z => z.Code == "JQ") });
                    list.Add(new SharedComponent.EntityInfo.Ticket.TicketCountInfo { AirlineCode = "QH", Count = query.Count(z => z.Code == "QH") });
                    list.Add(new SharedComponent.EntityInfo.Ticket.TicketCountInfo { AirlineCode = "HK", Count = query.Count(z => z.Code != "VN" &&
                    z.Code != "QH" && z.Code != "VJ" && z.Code != "JQ") });
                    param.TicketCountInfos = list;
                }
            }
        }


        public void GetAll(BK_BookingParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo
                            {

                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,

                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,

                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                BookCode = n.BookCode,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,

                            };
                param.BookingInfos = query.ToList();
            }
        }
        public BK_BookingInfo GetInfoPrint(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            join detail in dbContext.BK_BookingDetails on n.ID equals detail.BookingID
                            join Passenger in dbContext.BK_Passengers on detail.PassengerID equals Passenger.ID
                            where n.ID == id && n.IsDeleted == Constant.IsNotDeleted
                            select new BK_BookingInfo()
                            {

                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                BookCode = n.BookCode,
                                Expireddate = n.Expireddate,
                                xhdtencongty = n.xhdtencongty,
                                xhddiachi = n.xhddiachi,
                                xhdthanhpho = n.xhdthanhpho,
                                xhdnguoinhanhd = n.xhdnguoinhanhd,
                                xhdvat = n.xhdvat,
                                xhdktencongty = n.xhdktencongty,
                                xhdkdiachi = n.xhdkdiachi,
                                xhdktp = n.xhdktp,
                                xhdknguoinhanhd = n.xhdknguoinhanhd,
                                Sendmailgannhat = n.Sendmailgannhat,
                                Tiensendmailganhat = n.Tiensendmailganhat,
                                NguoiDaxem = n.NguoiDaxem,
                                Daxuathoadon = n.Daxuathoadon,
                                Nguoixuly = n.Nguoixuly,
                                Hotenguoixuly = n.Hotenguoixuly,
                                Ngayxuly = n.Ngayxuly,
                                UserCreate = n.UserCreate,
                                SourceSite = n.SourceSite,
                                FailCode = n.FailCode,
                            };
                return query.FirstOrDefault();
            }
        }
        public List<BK_BookingInfo> VeHetHanGiuChoGuiMail(DateTime? datestarttool)
        {

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            where n.IsDeleted == Constant.IsNotDeleted
                            && n.Status == Constant.StatusVe.DangGiuCho
                              && (n.Expireddate <= DateTime.Now)
                                && n.CreatedDate >= datestarttool
                              && (n.HetHan == false || n.HetHan.HasValue == false)
                            select new BK_BookingInfo
                            {
                                ID = n.ID,
                                BookCode = n.BookCode,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                IsDeleted = n.IsDeleted,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                Expireddate = n.Expireddate,
                                FailCode = n.FailCode,
                            };
                return query.OrderBy(z => z.Expireddate).ToList();

            }
        }
        public List<BK_BookingInfo> NhacLichbay(DateTime? datestarttool)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            join tk in dbContext.BK_Tickets on n.ID equals tk.BookingID
                            where n.IsDeleted == Constant.IsNotDeleted
                            && n.Status == Constant.StatusVe.DaXuatVe
                            && n.CreatedDate >= datestarttool
                            && tk.StartDate.Value.Date == DateTime.Now.AddDays(1).Date
                            && (tk.DaGuiMaiNhac == false || tk.DaGuiMaiNhac.HasValue == false)
                            select new BK_BookingInfo
                            {
                                ID = n.ID,
                                BookCode = n.BookCode,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                IsDeleted = n.IsDeleted,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                Expireddate = n.Expireddate,
                                ticketid = tk.ID,
                                Startdate = tk.StartDate,
                                daguimailnhac = tk.DaGuiMaiNhac,
                                FailCode = n.FailCode,
                            };
                return query.OrderBy(z => z.Expireddate).Take(300).ToList();
            }
        }
        public List<BK_BookingInfo> ChuyenBayKetthuc(DateTime? datestarttool)
        {

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Bookings
                            join tk in dbContext.BK_Tickets on n.ID equals tk.BookingID
                            where n.IsDeleted == Constant.IsNotDeleted
                            && n.Status == Constant.StatusVe.DaXuatVe
                            && tk.EndDate.Value.AddHours(12) < DateTime.Now
                            && n.CreatedDate >= datestarttool
                            && (tk.Daguimailcamon == false || tk.Daguimailcamon.HasValue == false)
                            select new BK_BookingInfo
                            {
                                ID = n.ID,
                                BookCode = n.BookCode,
                                SessionID = n.SessionID,
                                Status = n.Status,
                                TotalPrice = n.TotalPrice,
                                FromCity = n.FromCity,
                                ToCity = n.ToCity,
                                FlightDepart = n.FlightDepart,
                                FlightReturn = n.FlightReturn,
                                Ways = n.Ways,
                                Amount = n.Amount,
                                Name = n.Name,
                                Phone = n.Phone,
                                Address = n.Address,
                                Email = n.Email,
                                Note = n.Note,
                                IP = n.IP,
                                CreatedDate = n.CreatedDate,
                                UpdatedUser = n.UpdatedUser,
                                UpdatedDate = n.UpdatedDate,
                                DeletedUser = n.DeletedUser,
                                DeletedDate = n.DeletedDate,
                                IsDeleted = n.IsDeleted,
                                rTotal = n.rTotal,
                                rCode = n.rCode,
                                UserId = n.UserId,
                                OutputFee = n.OutputFee,
                                PaymentTypeID = n.PaymentTypeID,
                                PaymentTypeName = n.PaymentTypeName,
                                HoldToDate = n.HoldToDate,
                                ConfirmUserId = n.ConfirmUserId,
                                ConfirmDate = n.ConfirmDate,
                                FlightInfo = n.FlightInfo,
                                DiscountCode = n.DiscountCode,
                                DiscountPrice = n.DiscountPrice,
                                FromCityName = n.FromCityName,
                                ToCityName = n.ToCityName,
                                IsPaid = n.IsPaid,
                                PaidDate = n.PaidDate,
                                Expireddate = n.Expireddate,
                                ticketid = tk.ID,
                                Startdate = tk.StartDate,
                                daguimailnhac = tk.DaGuiMaiNhac,
                                Daguimailcamon = tk.Daguimailcamon,
                                FailCode = n.FailCode,
                            };
                return query.OrderBy(z => z.Expireddate).Take(300).ToList();
            }
        }
        public void ChuyenBaySapToiView(BK_BookingParam param)
        {
            var fiter = param.BookingFilter;
            if (fiter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var lstTaiKhoan = new List<string>();
                    if (!string.IsNullOrEmpty(fiter.TaiKhoan))
                    {
                        lstTaiKhoan.Add(fiter.TaiKhoan);
                        var objuser = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == fiter.TaiKhoan);
                        if (objuser != null)
                        {
                            var querytk = dbContext.AspNetUsers.Where(z => z.ParentId == objuser.Id);
                            lstTaiKhoan.AddRange(querytk.Select(z => z.UserName).ToList());
                        }
                    }
                    var query = from n in dbContext.BK_Bookings
                                join tk in dbContext.BK_Tickets on n.ID equals tk.BookingID
                                where n.IsDeleted == Constant.IsNotDeleted
                                 && (lstTaiKhoan.Count() == 0 || lstTaiKhoan.Contains(n.UserId))
                                 && n.Status == Constant.StatusVe.DaXuatVe
                                 && tk.StartDate.Value.Date == DateTime.Now.AddDays(1).Date
                                  && (fiter.ListUserName.Count == 0 || fiter.ListUserName.Contains(n.UserCreate))
                                select new BK_BookingInfo
                                {
                                    ID = n.ID,
                                    BookCode = tk.CodeBook,
                                    Status = n.Status,
                                    TotalPrice = n.TotalPrice,
                                    FromCity = tk.FromCity,
                                    ToCity = tk.ToCity,
                                    FlightDepart = n.FlightDepart,
                                    FlightReturn = n.FlightReturn,
                                    Ways = n.Ways,
                                    Amount = n.Amount,
                                    Name = n.Name,
                                    Phone = n.Phone,
                                    Address = n.Address,
                                    Email = n.Email,
                                    Note = n.Note,
                                    IP = n.IP,
                                    CreatedDate = n.CreatedDate,
                                    Startdate = tk.StartDate,
                                    rTotal = n.rTotal,
                                    rCode = n.rCode,
                                    UserId = n.UserId,
                                    HoldToDate = n.HoldToDate,
                                    Expireddate = n.Expireddate,
                                    UserCreate = n.UserCreate,
                                    SourceSite = n.SourceSite,
                                    FailCode = n.FailCode,

                                };
                    param.BookingInfos = query.OrderByDescending(z => z.CreatedDate).ToList();
                }
            }
        }
        #endregion

        #region  Payment Info
        public tbl_PaymentInfo GetInfoThanhToan(int? BookingID)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                return dbContext.tbl_PaymentInfos.FirstOrDefault(z => z.IdBooking == BookingID);
            }
        }
        public void UpdatePayment(tbl_PaymentInfo ck)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var checkpayment = dbContext.tbl_PaymentInfos.FirstOrDefault(z => z.IdBooking == ck.IdBooking);
                if (checkpayment == null)
                    dbContext.tbl_PaymentInfos.InsertOnSubmit(ck);
                else
                {
                    checkpayment.IdHinhThuc = ck.IdHinhThuc;
                    checkpayment.Note = ck.Note;
                }
                dbContext.SubmitChanges();
            }
        }
        public void UpdatePayment(int IdBooking, string note)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var checkpayment = dbContext.tbl_PaymentInfos.FirstOrDefault(z => z.IdBooking == IdBooking);
                if (checkpayment != null)
                {
                    checkpayment.Note = note;
                }
                dbContext.SubmitChanges();
            }
        }
        #endregion
        public void Doanhso(BK_BookingParam param)
        {
            var fiter = param.BookingFilter;
            if (fiter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var query = from n in dbContext.BK_Bookings
                                where n.IsDeleted == Constant.IsNotDeleted
                                && (
                                string.IsNullOrEmpty(param.BookingFilter.Search)
                                || (
                                   n.Phone.Contains(param.BookingFilter.Search)
                                || n.Name.Contains(param.BookingFilter.Search)
                                || n.Email.Contains(param.BookingFilter.Search)
                                || n.BookCode.Contains(param.BookingFilter.Search)))
                                && (param.BookingFilter.Fromdate.HasValue == false || n.CreatedDate.Value.Date >= param.BookingFilter.Fromdate)
                                && (param.BookingFilter.Todate.HasValue == false || n.CreatedDate.Value.Date <= param.BookingFilter.Todate)
                                && (n.Status == Constant.StatusVe.DaXuatVe)
                                && (fiter.ListUserName.Count == 0 || fiter.ListUserName.Contains(n.UserCreate))
                                && n.BookCode.Trim() != ""
                                select new BK_BookingInfo
                                {
                                    ID = n.ID,
                                    BookCode = n.BookCode,
                                    SessionID = n.SessionID,
                                    Status = n.Status,
                                    TotalPrice = n.TotalPrice,
                                    FromCity = n.FromCity,
                                    ToCity = n.ToCity,
                                    FlightDepart = n.FlightDepart,
                                    FlightReturn = n.FlightReturn,
                                    Ways = n.Ways,
                                    Amount = n.Amount,
                                    Name = n.Name,
                                    Phone = n.Phone,
                                    Address = n.Address,
                                    Email = n.Email,
                                    Note = n.Note,
                                    IP = n.IP,
                                    CreatedDate = n.CreatedDate,
                                    UpdatedUser = n.UpdatedUser,
                                    UpdatedDate = n.UpdatedDate,
                                    DeletedUser = n.DeletedUser,
                                    DeletedDate = n.DeletedDate,
                                    IsDeleted = n.IsDeleted,
                                    rTotal = n.rTotal,
                                    rCode = n.rCode,
                                    UserId = n.UserId,
                                    OutputFee = n.OutputFee,
                                    PaymentTypeID = n.PaymentTypeID,
                                    PaymentTypeName = n.PaymentTypeName,
                                    HoldToDate = n.HoldToDate,
                                    ConfirmUserId = n.ConfirmUserId,
                                    ConfirmDate = n.ConfirmDate,
                                    FlightInfo = n.FlightInfo,
                                    DiscountCode = n.DiscountCode,
                                    DiscountPrice = n.DiscountPrice,
                                    FromCityName = n.FromCityName,
                                    ToCityName = n.ToCityName,
                                    IsPaid = n.IsPaid,
                                    PaidDate = n.PaidDate,
                                    Expireddate = n.Expireddate,
                                    xhdtencongty = n.xhdtencongty,
                                    xhddiachi = n.xhddiachi,
                                    xhdthanhpho = n.xhdthanhpho,
                                    xhdnguoinhanhd = n.xhdnguoinhanhd,
                                    xhdvat = n.xhdvat,
                                    xhdktencongty = n.xhdktencongty,
                                    xhdkdiachi = n.xhdkdiachi,
                                    xhdktp = n.xhdktp,
                                    xhdknguoinhanhd = n.xhdknguoinhanhd,
                                    Sendmailgannhat = n.Sendmailgannhat,
                                    Tiensendmailganhat = n.Tiensendmailganhat,
                                    NguoiDaxem = n.NguoiDaxem,
                                    PaymentInfo = dbContext.tbl_PaymentInfos.FirstOrDefault(z => z.IdBooking == n.ID),
                                    Daxuathoadon = n.Daxuathoadon,
                                    Nguoixuly = n.Nguoixuly,
                                    Hotenguoixuly = n.Hotenguoixuly,
                                    Ngayxuly = n.Ngayxuly,
                                };
                    param.BookingInfos = query.OrderByDescending(z => z.CreatedDate).ToList();

                }
            }
        }
    }
}
