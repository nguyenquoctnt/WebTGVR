using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.WebBackEnd.Models;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.Dao;
using VDMMutiline.Ultilities;
using VDMMutiline.Core;

namespace VDMMutiline.WebBackEnd.Areas.QuanLyVe.Controllers
{
    public class CreateBookingController : BaseController
    {
        private string SSkey = "CreateBookingAdmin";
        private List<AirlineModelInfo> GetAirlineInfos()
        {
            var biz = new AirlineBiz();
            var param = new AirlineParam() { AirlineFilter = new AirlineFilter() };
            biz.Search(param);
            return param.AirlineInfos.Select(z => new AirlineModelInfo
            {
                Code = z.Code,
                Name = z.Name + " (" + z.Code + ")"
            }).ToList();
        }
        public ActionResult Index()
        {
            Session[SSkey] = null;
            CreareBookingModel model = new CreareBookingModel();
            model.Contact = new Contacts();
            model.TotalAdult = 1;
            model.lstTicket = new List<Tickket>();
            model.lstpassenger = new List<passenger>();
            var ticketinfo = new Tickket();
            ticketinfo.lstAirlineInfo = GetAirlineInfos();
            ticketinfo.types = Constant.Typeve.LuotDi;
            model.lstTicket.Add(ticketinfo);


            var ticketinfove = new Tickket();
            ticketinfove.lstAirlineInfo = GetAirlineInfos();
            ticketinfove.types = Constant.Typeve.LuotVe;
            model.lstTicket.Add(ticketinfove);
            model.Contact.FullName = CurrentUser.DisplayName;
            model.Contact.Phone = CurrentUser.PhoneNumber;
            model.Contact.Email = CurrentUser.Email;
            model.Contact.Address = CurrentUser.Location;
            ///lstTicket chiều đi
            Session[SSkey] = model;
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(CreareBookingModel model, FormCollection forminput)
        {
            try
            {
                var getWays = Utility.ConvertToInt(forminput.GetValue("Ways") != null ? forminput.GetValue("Ways").AttemptedValue : "0");
                model.Itinerary = getWays;
                int IDBooking = SaveBooking(model);
                var indexticket = 0;
                foreach (var ticket in model.lstTicket)
                {

                    if (getWays <= 1)
                    {
                        if (indexticket > 0)
                            break;
                    }
                    var ticketid = SaveTicket(IDBooking, ticket, ticket.types, model.FromCity, model.ToCity);
                    SaveTicketDetail(ticket, ticketid, model.FromCity, model.ToCity);
                    var FareAdt = Utility.ConvertTodouble((ticket.PriceNetAdult == null ? "" : ticket.PriceNetAdult).Replace(",", "").Replace(".", "")) ?? 0;
                    var FareChd = Utility.ConvertTodouble((ticket.PriceNetChild == null ? "" : ticket.PriceNetChild).Replace(",", "").Replace(".", "")) ?? 0;
                    var FareInf = Utility.ConvertTodouble((ticket.PriceNetInfant == null ? "" : ticket.PriceNetInfant).Replace(",", "").Replace(".", "")) ?? 0;
                    var FeeAdt = Utility.ConvertTodouble((ticket.PriceFeeAdult == null ? "" : ticket.PriceFeeAdult).Replace(",", "").Replace(".", "")) ?? 0;
                    var FeeChd = Utility.ConvertTodouble((ticket.PriceFeeChild == null ? "" : ticket.PriceFeeChild).Replace(",", "").Replace(".", "")) ?? 0;
                    var FeeInf = Utility.ConvertTodouble((ticket.PriceFeeInfant == null ? "" : ticket.PriceFeeInfant).Replace(",", "").Replace(".", "")) ?? 0;
                    var TaxAdt = Utility.ConvertTodouble((ticket.PriceTaxAdult == null ? "" : ticket.PriceTaxAdult).Replace(",", "").Replace(".", "")) ?? 0;
                    var TaxChd = Utility.ConvertTodouble((ticket.PriceTaxChild == null ? "" : ticket.PriceTaxChild).Replace(",", "").Replace(".", "")) ?? 0;
                    var TaxInf = Utility.ConvertTodouble((ticket.PriceTaxInfant == null ? "" : ticket.PriceTaxInfant).Replace(",", "").Replace(".", "")) ?? 0;
                    var StartDate = Utility.ConvertStringToDate(ticket.StartDate, ticket.StartTime);
                    var EndDate = Utility.ConvertStringToDate(ticket.EndDate, ticket.EndTime);
                    var index = 0;
                    foreach (var item in model.lstpassenger)
                    {
                        if (item.Type == Constant.TypePassenger.NguoiLon)
                        {
                            item.Price += FareAdt + FeeAdt + TaxAdt;
                        }
                        else if (item.Type == Constant.TypePassenger.TreEm)
                        {
                            item.Price += FareChd + FeeChd + TaxChd;
                        }
                        else if (item.Type == Constant.TypePassenger.EmBe)
                        {
                            item.Price += FareInf + FeeInf + TaxInf;
                        }
                        var ckBookingDetail = new BK_BookingDetail();
                        var idPassenger = 0;
                        if (indexticket <= 1)
                        {
                            idPassenger = SavePassenger(item, model.FromCity);
                            item.Id = idPassenger;
                            ckBookingDetail.FromCity = model.FromCity;
                            ckBookingDetail.ToCity = model.ToCity;
                        }
                        else
                        {
                            idPassenger = item.Id ?? 0;
                            ckBookingDetail.FromCity = model.ToCity;
                            ckBookingDetail.ToCity = model.FromCity;
                        }
                        ckBookingDetail.BookingID = IDBooking;
                        ckBookingDetail.TicketID = ticketid;
                        ckBookingDetail.PassengerID = idPassenger;
                        ckBookingDetail.FlightNo = ticket.FlightNo;
                        ckBookingDetail.StartDate = StartDate;
                        if (item.Type == Constant.TypePassenger.NguoiLon)
                        {
                            ckBookingDetail.Price = FareAdt + FeeAdt + TaxAdt;
                        }
                        else if (item.Type == Constant.TypePassenger.TreEm)
                        {
                            ckBookingDetail.Price = FareChd + FeeChd + TaxChd;
                        }
                        else if (item.Type == Constant.TypePassenger.EmBe)
                        {
                            ckBookingDetail.Price = FareInf + FeeInf + TaxInf;
                        }
                        saveDetail(ckBookingDetail);
                        index++;
                    }
                    indexticket++;
                }
                LogMain.LogBooking(IDBooking, CurrentUser.UserName, "", "", Constant.LogBookingType.DatVeTrenHeThong);
                return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateDanhSachHanhKhac(int? nguoilon, int? treem, int? embe)
        {
            var model = (CreareBookingModel)Session[SSkey];
            model.lstpassenger = new List<passenger>();
            for (var i = 0; i < nguoilon; i++)
            {
                var pasobj = new passenger()
                {
                    Type = Constant.TypePassenger.NguoiLon,
                    GuiID = "NL" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString(),
                };
                model.lstpassenger.Add(pasobj);
            }
            for (var i = 0; i < treem; i++)
            {
                var pasobj = new passenger()
                {
                    Type = Constant.TypePassenger.TreEm,
                    GuiID = "TE" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString(),
                };
                model.lstpassenger.Add(pasobj);
            }
            for (var i = 0; i < embe; i++)
            {
                var pasobj = new passenger()
                {
                    Type = Constant.TypePassenger.EmBe,
                    GuiID = "EB" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString(),
                };
                model.lstpassenger.Add(pasobj);
            }
            model.lstpassenger = model.lstpassenger.OrderBy(z => z.Type).ToList();
            Session[SSkey] = model;
            return View(model);
        }
        public ActionResult Getlistdichvu(string guiid)
        {
            var model = (CreareBookingModel)Session[SSkey];
            var objpad = model.lstpassenger.FirstOrDefault(z => z.GuiID == guiid);
            if (objpad.lstaddons == null)
                objpad.lstaddons = new List<addons>();
            return View(objpad);
        }
        public ActionResult Adddichvu(string guiid)
        {
            var obj = new addons();
            obj.GuiIDaddon = "addon" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString();
            obj.GuiIDPad = guiid;
            return View(obj);
        }
        [HttpPost]
        public ActionResult Adddichvu(addons obj)
        {
            if (Session[SSkey] == null)
                return Json(new { isSuccess = false, mess = "Phiên làm việc đã kết thúc, xin vui lòng thao tác lại từ đầu." }, JsonRequestBehavior.AllowGet);
            var model = (CreareBookingModel)Session[SSkey];
            try
            {
                foreach (var item in model.lstpassenger.Where(z => z.GuiID == obj.GuiIDPad))
                {

                    if (item.lstaddons == null)
                        item.lstaddons = new List<addons>();

                    if (item.lstaddons.Count(z => z.Type == obj.Type) >= 1)
                    {
                        return Json(new { isSuccess = false, mess = "Chỉ có thể thêm 1 dịch vụ ở chiều đi hoặc chiều về." }, JsonRequestBehavior.AllowGet);
                    }
                    if (item.lstaddons.Count <= 0)
                    {
                        item.lstaddons.Add(obj);
                    }
                    else
                    {
                        var objcheck = item.lstaddons.FirstOrDefault(z => z.GuiIDaddon == obj.GuiIDaddon);
                        if (objcheck == null)
                        {
                            item.lstaddons.Add(obj);
                        }
                        else
                        {
                            foreach (var items in item.lstaddons.Where(z => z.GuiIDaddon == obj.GuiIDaddon))
                            {
                                items.valueAddon = obj.valueAddon;
                                items.NameAddon = obj.NameAddon;
                                items.priceAfterAddon = obj.priceAfterAddon;
                                items.pricebeforeAddon = obj.pricebeforeAddon;
                            }
                        }
                    }

                }
                Session[SSkey] = model;
                return Json(new { isSuccess = true, GuiID = obj.GuiIDPad, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Deletedichvu(string guiid, string GuiIDPad)
        {
            if (Session[SSkey] == null)
                return Json(new { isSuccess = false, mess = "Phiên làm việc đã kết thúc, xin vui lòng thao tác lại từ đầu." }, JsonRequestBehavior.AllowGet);
            var model = (CreareBookingModel)Session[SSkey];
            var objpass = model.lstpassenger.Where(z => z.GuiID == GuiIDPad);
            if (objpass.Count() > 0)
            {
                foreach (var item in model.lstpassenger.Where(z => z.GuiID == GuiIDPad))
                {
                    var objcheck = item.lstaddons.FirstOrDefault(z => z.GuiIDaddon == guiid);
                    if (objcheck != null)
                    {
                        item.lstaddons.Remove(objcheck);
                    }
                }
                Session[SSkey] = model;
                return Json(new { isSuccess = true, GuiID = GuiIDPad, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { isSuccess = false, mess = "Không tìm thấy hành khách." }, JsonRequestBehavior.AllowGet);

        }
        #region method
        public void saveDetail(BK_BookingDetail ck)
        {
            var dao = new BK_BookingDetailDao();
            dao.Insert(ck);
        }
        public int SaveTicket(int IDBooking, Tickket input, int? TypeID, string StartPoint, string EndPoint)
        {

            var FareAdt = Utility.ConvertTodouble((input.PriceNetAdult == null ? "" : input.PriceNetAdult).Replace(",", "").Replace(".", "")) ?? 0;
            var FareChd = Utility.ConvertTodouble((input.PriceNetChild == null ? "" : input.PriceNetChild).Replace(",", "").Replace(".", "")) ?? 0;
            var FareInf = Utility.ConvertTodouble((input.PriceNetInfant == null ? "" : input.PriceNetInfant).Replace(",", "").Replace(".", "")) ?? 0;
            var FeeAdt = Utility.ConvertTodouble((input.PriceFeeAdult == null ? "" : input.PriceFeeAdult).Replace(",", "").Replace(".", "")) ?? 0;
            var FeeChd = Utility.ConvertTodouble((input.PriceFeeChild == null ? "" : input.PriceFeeChild).Replace(",", "").Replace(".", "")) ?? 0;
            var FeeInf = Utility.ConvertTodouble((input.PriceFeeInfant == null ? "" : input.PriceFeeInfant).Replace(",", "").Replace(".", "")) ?? 0;
            var TaxAdt = Utility.ConvertTodouble((input.PriceTaxAdult == null ? "" : input.PriceTaxAdult).Replace(",", "").Replace(".", "")) ?? 0;
            var TaxChd = Utility.ConvertTodouble((input.PriceTaxChild == null ? "" : input.PriceTaxChild).Replace(",", "").Replace(".", "")) ?? 0;
            var TaxInf = Utility.ConvertTodouble((input.PriceTaxInfant == null ? "" : input.PriceTaxInfant).Replace(",", "").Replace(".", "")) ?? 0;
            var ck = new BK_Ticket
            {
                BookingID = IDBooking,
                FlightNo = input.FlightNo,
                StartDate = Utility.ConvertStringToDate(input.StartDate, input.StartTime),
                EndDate = Utility.ConvertStringToDate(input.EndDate, input.EndTime),
                Price = (FareAdt + FareChd + FareInf + FeeAdt + FeeChd + FeeInf + TaxAdt + TaxChd + TaxInf),
                FromCity = StartPoint,
                ToCity = EndPoint,
                Class = input.Class,
                ClassName = input.Class,
                Code = input.AirlineCode,
                IsDeleted = false,
                PriceBefore = (FareAdt + FareChd + FareInf + FeeAdt + FeeChd + FeeInf + TaxAdt + TaxChd + TaxInf),
                TypeID = TypeID,
                FareAdt = FareAdt,
                FareChd = FareChd,
                FareInf = FareInf,
                FeeAdt = FeeAdt,
                FeeChd = FeeChd,
                FeeInf = FeeInf,
                TaxAdt = TaxAdt,
                TaxChd = TaxChd,
                TaxInf = TaxInf,
                Duration = 0,

            };
            ck.Tax = ck.Price - ck.PriceBefore;
            ck.CodeBook = input.Code;
            ck.Exdate = Utility.ConvertStringToDate(input.HoldToDate, input.HoldTime);
            ck.Eror = "";
            var dao = new BK_TicketDao();
            return dao.Insert(ck);
        }
        public int SaveTicket(BK_Ticket ck)
        {
            var dao = new BK_TicketDao();
            return dao.Insert(ck);
        }
        public int SaveTicketDetail(Tickket Flight, int? ticketid, string StartPoint, string EndPoint)
        {

            var ck = new BK_TicketDetail()
            {
                airlineCode = Flight.AirlineCode,
                flight = Flight.FlightNo,
                StartDate = Utility.ConvertStringToDate(Flight.StartDate, Flight.StartTime),
                EndDate = Utility.ConvertStringToDate(Flight.EndDate, Flight.EndTime),
                FromCity = StartPoint,
                ToCity = EndPoint,
                Class = Flight.Class,
                TicketID = ticketid,
                Plane = "",
                Duration = 0,
                StopTime = 0,
            };
            var dao = new BK_TicketDao();
            return dao.InsertDetali(ck);
        }
        public int SaveTicketDetail(BK_TicketDetail ck)
        {
            var dao = new BK_TicketDao();
            return dao.InsertDetali(ck);
        }
        public int SaveBooking(CreareBookingModel model)
        {
            double? totalprice = 0;
            var item = new BK_Booking();
            var dao = new BK_BookingDao();
            item.SessionID = Guid.NewGuid().ToString();
            string codedi = "";
            string codeve = "";
            item.Status = 1;
            item.Ways = model.lstpassenger.Count;

            item.FromCity = model.FromCity;
            item.ToCity = model.ToCity;
            item.Name = model.Contact.FullName;
            item.Email = model.Contact.Email;
            item.Phone = model.Contact.Phone;
            item.Address = model.Contact.Address;
            item.Note = model.Contact.Notes;
            var DepartureFlight = model.lstTicket.FirstOrDefault(z => z.types == Constant.Typeve.LuotDi);
            DateTime? hodldate = new DateTime();
            if (DepartureFlight != null)
            {
                codedi = DepartureFlight.Code;
                item.FlightDepart = DepartureFlight.FlightNo;
                hodldate = Utility.ConvertStringToDate(DepartureFlight.HoldToDate, DepartureFlight.HoldTime);

                var FareAdt = Utility.ConvertTodouble((DepartureFlight.PriceNetAdult == null ? "" : DepartureFlight.PriceNetAdult).Replace(",", "").Replace(".", "")) ?? 0;
                var FareChd = Utility.ConvertTodouble((DepartureFlight.PriceNetChild == null ? "" : DepartureFlight.PriceNetChild).Replace(",", "").Replace(".", "")) ?? 0;
                var FareInf = Utility.ConvertTodouble((DepartureFlight.PriceNetInfant == null ? "" : DepartureFlight.PriceNetInfant).Replace(",", "").Replace(".", "")) ?? 0;
                var FeeAdt = Utility.ConvertTodouble((DepartureFlight.PriceFeeAdult == null ? "" : DepartureFlight.PriceFeeAdult).Replace(",", "").Replace(".", "")) ?? 0;
                var FeeChd = Utility.ConvertTodouble((DepartureFlight.PriceFeeChild == null ? "" : DepartureFlight.PriceFeeChild).Replace(",", "").Replace(".", "")) ?? 0;
                var FeeInf = Utility.ConvertTodouble((DepartureFlight.PriceFeeInfant == null ? "" : DepartureFlight.PriceFeeInfant).Replace(",", "").Replace(".", "")) ?? 0;
                var TaxAdt = Utility.ConvertTodouble((DepartureFlight.PriceTaxAdult == null ? "" : DepartureFlight.PriceTaxAdult).Replace(",", "").Replace(".", "")) ?? 0;
                var TaxChd = Utility.ConvertTodouble((DepartureFlight.PriceTaxChild == null ? "" : DepartureFlight.PriceTaxChild).Replace(",", "").Replace(".", "")) ?? 0;
                var TaxInf = Utility.ConvertTodouble((DepartureFlight.PriceTaxInfant == null ? "" : DepartureFlight.PriceTaxInfant).Replace(",", "").Replace(".", "")) ?? 0;
                totalprice += (((FareAdt + FeeAdt + TaxAdt) * model.TotalAdult) + ((FareChd + FeeChd + TaxChd) * model.TotalChild) + ((FareInf + FeeInf + TaxInf) * model.TotalInfant));
            }
            item.CreatedDate = DateTime.Now;
            item.IP = Utility.GetUserIPAddress();
            item.DiscountCode = "";
            item.DiscountPrice = 0;

            item.UserCreate = CurrentUser.UserName;
            item.SourceSite = CurrentUser.UrlDomain1;
            if (model.Itinerary > 1)
            {
                var FlightReturn = model.lstTicket.FirstOrDefault(z => z.types == Constant.Typeve.LuotVe);
                if (FlightReturn != null)
                {
                    item.FlightReturn = FlightReturn.FlightNo;
                    codeve = FlightReturn.Code;
                    var date = Utility.ConvertStringToDate(FlightReturn.HoldToDate, FlightReturn.HoldTime);
                    if (date < hodldate)
                        hodldate = date;
                    var FareAdt = Utility.ConvertTodouble((FlightReturn.PriceNetAdult == null ? "" : FlightReturn.PriceNetAdult).Replace(",", "").Replace(".", "")) ?? 0;
                    var FareChd = Utility.ConvertTodouble((FlightReturn.PriceNetChild == null ? "" : FlightReturn.PriceNetChild).Replace(",", "").Replace(".", "")) ?? 0;
                    var FareInf = Utility.ConvertTodouble((FlightReturn.PriceNetInfant == null ? "" : FlightReturn.PriceNetInfant).Replace(",", "").Replace(".", "")) ?? 0;
                    var FeeAdt = Utility.ConvertTodouble((FlightReturn.PriceFeeAdult == null ? "" : FlightReturn.PriceFeeAdult).Replace(",", "").Replace(".", "")) ?? 0;
                    var FeeChd = Utility.ConvertTodouble((FlightReturn.PriceFeeChild == null ? "" : FlightReturn.PriceFeeChild).Replace(",", "").Replace(".", "")) ?? 0;
                    var FeeInf = Utility.ConvertTodouble((FlightReturn.PriceFeeInfant == null ? "" : FlightReturn.PriceFeeInfant).Replace(",", "").Replace(".", "")) ?? 0;
                    var TaxAdt = Utility.ConvertTodouble((FlightReturn.PriceTaxAdult == null ? "" : FlightReturn.PriceTaxAdult).Replace(",", "").Replace(".", "")) ?? 0;
                    var TaxChd = Utility.ConvertTodouble((FlightReturn.PriceTaxChild == null ? "" : FlightReturn.PriceTaxChild).Replace(",", "").Replace(".", "")) ?? 0;
                    var TaxInf = Utility.ConvertTodouble((FlightReturn.PriceTaxInfant == null ? "" : FlightReturn.PriceTaxInfant).Replace(",", "").Replace(".", "")) ?? 0;
                    totalprice += (((FareAdt + FeeAdt + TaxAdt) * model.TotalAdult) + ((FareChd + FeeChd + TaxChd) * model.TotalChild) + ((FareInf + FeeInf + TaxInf) * model.TotalInfant));
                }
            }
            string codebook = codedi;
            if (!string.IsNullOrEmpty(codeve))
            {

                if (codedi != codeve)
                    codebook += "-" + codeve;
            }
            item.Expireddate = hodldate;
            item.BookCode = codebook;
            item.UserId = CurrentUser.UserName;

            var modelpass = (CreareBookingModel)Session[SSkey];
            foreach (var itempass in modelpass.lstpassenger)
            {
                if (itempass.lstaddons != null)
                {
                    foreach (var itemaddon in itempass.lstaddons)
                    {
                        totalprice += (Utility.ConvertTodouble(itemaddon.priceAfterAddon != null ? itemaddon.priceAfterAddon.Replace(",", "").Replace(".", "") : "") ?? 0);
                    }
                }
            }
            item.TotalPrice = totalprice;
            item.OutputFee = (decimal?)totalprice;
            var idbk = dao.Insert(item);
            var paymentinfo = new tbl_PaymentInfo();
            paymentinfo.IdHinhThuc = Constant.Hinhthucthanhtoav2.chuyenkhoan;
            paymentinfo.IdBooking = idbk;
            dao.UpdatePayment(paymentinfo);

            return idbk;
        }
        public int SavePassenger(passenger item, string StartPoint)
        {
            var fullname = item.FUllName.Split(' ');
            string LastName = "";
            for (int i = 1; i < fullname.Count(); i++)
            {
                LastName += fullname[i] + " ";
            }
            var dbItem = new BK_Passenger
            {
                TypeID = item.Type,
                FirstName = fullname.Count() >= 1 ? Utility.ConvertToUnsign(fullname[0]) : "",

                Sex = item.Sex,
                Name = Utility.ConvertToUnsign(LastName),
                Address = "",
                Email = "",
                Phone = "",
                City = StartPoint,
                IsDeleted = false
            };
            if (item.Type != Constant.TypePassenger.NguoiLon)
                dbItem.Birthday = Utility.ConvertToDate(item.Birthday);
            var model = (CreareBookingModel)Session[SSkey];
            var objpad = model.lstpassenger.FirstOrDefault(z => z.GuiID == item.GuiID);
            item.lstaddons = objpad.lstaddons;
            if (item.lstaddons != null)
            {
                foreach (var itempas in item.lstaddons)
                {
                    if (itempas.Type == "Đi")
                    {
                        dbItem.BaggageDepartID = itempas.NameAddon;
                        dbItem.BaggageDepartValue = itempas.valueAddon;
                        dbItem.BaggageDepartPrice = Utility.ConvertToDecimal(itempas.priceAfterAddon.Replace(",", "").Replace(".", ""));
                    }
                    else if (itempas.Type == "Về")
                    {
                        dbItem.BaggageReturnID = itempas.NameAddon;
                        dbItem.BaggageReturnValue = itempas.valueAddon;
                        dbItem.BaggageReturnPrice = Utility.ConvertToDecimal(itempas.priceAfterAddon.Replace(",", "").Replace(".", ""));
                    }
                }
            }
            var dao = new BK_PassengerDao();
            return dao.Insert(dbItem);
        }
        #endregion
    }
}