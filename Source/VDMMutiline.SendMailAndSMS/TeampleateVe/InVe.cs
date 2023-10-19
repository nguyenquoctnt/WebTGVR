using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using System.Web;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using VDMMutiline.Core.Common;
using VDMMutiline.Ultilities;

namespace VDMMutiline.SendMailAndSMS.TeampleateVe
{
    public partial class InVe : DevExpress.XtraReports.UI.XtraReport
    {
        private string getsanbaycode(string code)
        {
            var dao = new StationDao();
            var obj = dao.Getstringbycode(code);
            if (obj != null)
                return obj.label;
            return "";
        }
        public InVe(int bookingid, List<SettingUserInfo> SettingValue)
        {
            InitializeComponent();


            var dao = new BK_BookingDao();
            var daoPassenger = new BK_PassengerDao();
            var daoticket = new BK_TicketDao();
            var inforbk = dao.GetInfo(bookingid);
            if (inforbk != null)
            {
                var CurentUser = CommonUI.GetInforByUser(inforbk.UserCreate);
                var setting = CommonUI.GetSettingByUser(CurentUser.UserName, true);
                string phone = "Tổng đài: ";
                phone += CurentUser.PhoneNumber;
                if (!string.IsNullOrEmpty(CurentUser.PhoneNumber2))
                    phone += "- Hotline:" + CurentUser.PhoneNumber2;
                TXTTONGDAI.Text = phone;
                txttensdt.Text = inforbk.Name + " - " + inforbk.Phone;
                TXTCAMONQUYKHACH.Text = "Cảm ơn quý khách đã sử dụng dịch vụ tại " + UIUty.GetsettingByKey(setting, "PRT_COMPANY_NAME");
                TXTMADONHANG.Text = "Mã đơn hàng của quý khách là: " + inforbk.rCode;
                txttrangthai.Text = "Trạng thái: " + Utility.GetDictionaryValue(Constant.StatusVe.dctName, inforbk.Status);
                txtthoigiangiucho.Text = "Thời gian giữ chỗ: "+ (inforbk.Expireddate.HasValue ? inforbk.Expireddate.Value.ToString("dd/MM/yyyy HH:mm") : "");
                txtmacode.Text = "AIRLINES CODE:"+ inforbk.BookCode;

               

                
               
                var listTicket = daoticket.GetListByBooking(inforbk.ID).OrderBy(z => z.ID);
                string hanghangkhong = "";
                decimal tongtienhanhly = 0;
                var indexve = 0;
                string codedi = "";
                foreach (var item in listTicket)
                {
                    if (indexve == 0)
                    {
                        hanghangkhong += UIUty.GetNamebyHang(item.Code);
                        codedi = item.Code;
                    }
                    else
                    {
                        if (codedi != item.Code)
                            hanghangkhong += " - " + UIUty.GetNamebyHang(item.Code);
                    }
                    var paramPassenger = new BK_PassengerParam { BookingID = inforbk.ID, Veid = item.ID };
                    daoPassenger.GetbyBooking(paramPassenger);
                    var listPassenger = paramPassenger.BK_PassengerInfos;
                    decimal tongtienhanhlysum = 0;
                    if (item.TypeID == Constant.Typeve.LuotDi)
                    {
                        var sum = listPassenger.Sum(z => z.BaggageDepartPrice);
                        tongtienhanhlysum = sum.HasValue ? sum.Value : 0;
                    }
                    else
                    {
                        var sum = listPassenger.Sum(z => z.BaggageReturnPrice);
                        tongtienhanhlysum = sum.HasValue ? sum.Value : 0;
                    }
                    tongtienhanhly += tongtienhanhlysum;

                    indexve++;
                }

                var bkdetail = new BK_BookingDetailDao();
                var listdetailbk = bkdetail.GetbylistbybookingjoinPassengers(inforbk.ID);

                var objPricenguoilon = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var objPricetreem = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var objPriceembe = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);

                var objPricenguoilonve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotVe);
                var objPricetreemve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotVe);
                var objPriceembeve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotVe);

                var Pricenguoilon = objPricenguoilon != null ? objPricenguoilon.Price : 0;
                var Pricetreem = objPricetreem != null ? objPricetreem.Price : 0;
                var Priceembe = objPriceembe != null ? objPriceembe.Price : 0;
                var Pricenguoilonve = objPricenguoilonve != null ? objPricenguoilonve.Price : 0;
                var Pricetreemve = objPricetreemve != null ? objPricetreemve.Price : 0;
                var Priceembeve = objPriceembeve != null ? objPriceembeve.Price : 0;
                var checkquoc = CheckQuocTe(inforbk.FromCity, inforbk.ToCity);
                if (checkquoc)
                {
                    Pricenguoilonve = 0;
                    Pricetreemve = 0;
                    Priceembeve = 0;
                }
                txttiengnuoilon.Text = ((Pricenguoilon ?? 0) + (Pricenguoilonve ?? 0)).ToString("n0");
                txttientreem.Text = ((Pricetreem ?? 0) + (Pricetreemve ?? 0)).ToString("n0");
                txttienembe.Text = ((Priceembe ?? 0) + (Priceembeve ?? 0)).ToString("n0");
                var coutnguoilon = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var counttreem = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var countembe = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);
                txtslnguoilon.Text = coutnguoilon.ToString("n0");
                txtsltreem.Text = counttreem.ToString("n0");
                txtsoluongembe.Text = countembe.ToString("n0");

                var tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon).Sum(z => z.Price);
                var tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm).Sum(z => z.Price);
                var tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe).Sum(z => z.Price);
                if (checkquoc)
                {
                    tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                }
                txttongtiennguoilon.Text = (tongtiennguoilon ?? 0).ToString("n0");
                txttongtientreem.Text = (tongtientreem ?? 0).ToString("n0");
                txttongtienembe.Text = (tongtientembe ?? 0).ToString("n0");
                txttonghanhly.Text = tongtienhanhly.ToString("n0") + " VND";
                var tongtien = inforbk.TotalPrice.HasValue ? inforbk.TotalPrice.Value : 0;
                txttongcongtien.Text = tongtien.ToString("n0") + " VND";
            }
            var rptchitietve = new RptVe(bookingid);
            subdanhsachve.ReportSource = rptchitietve;
            var RptChiTietGia = new RptChiTietGia(bookingid);
            subchitietve.ReportSource = RptChiTietGia;
        }
        public InVe(string bookingCode, List<SettingUserInfo> SettingValue)
        {
            InitializeComponent();
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(bookingCode);
            var daoPassenger = new BK_PassengerDao();
            var daoticket = new BK_TicketDao();
            if (inforbk != null)
            {
                var CurentUser = CommonUI.GetInforByUser(inforbk.UserCreate);
                var setting = CommonUI.GetSettingByUser(CurentUser.UserName, true);
                string phone = "Tổng đài: ";
                phone += CurentUser.PhoneNumber;
                if (!string.IsNullOrEmpty(CurentUser.PhoneNumber2))
                    phone += "- Hotline:" + CurentUser.PhoneNumber2;
                TXTTONGDAI.Text = phone;
                txttensdt.Text = inforbk.Name + " - " + inforbk.Phone;
                TXTCAMONQUYKHACH.Text = "Cảm ơn quý khách đã sử dụng dịch vụ tại " + UIUty.GetsettingByKey(setting, "PRT_COMPANY_NAME");
                TXTMADONHANG.Text = "Mã đơn hàng của quý khách là: " + inforbk.rCode;
                txttrangthai.Text = "Trạng thái: " + Utility.GetDictionaryValue(Constant.StatusVe.dctName, inforbk.Status);
                txtthoigiangiucho.Text = "Thời gian giữ chỗ: " + (inforbk.Expireddate.HasValue ? inforbk.Expireddate.Value.ToString("dd/MM/yyyy HH:mm") : "");
                txtmacode.Text = "AIRLINES CODE:" + inforbk.BookCode;





                var listTicket = daoticket.GetListByBooking(inforbk.ID).OrderBy(z => z.ID);
                string hanghangkhong = "";
                decimal tongtienhanhly = 0;
                var indexve = 0;
                string codedi = "";
                foreach (var item in listTicket)
                {
                    if (indexve == 0)
                    {
                        hanghangkhong += UIUty.GetNamebyHang(item.Code);
                        codedi = item.Code;
                    }
                    else
                    {
                        if (codedi != item.Code)
                            hanghangkhong += " - " + UIUty.GetNamebyHang(item.Code);
                    }
                    var paramPassenger = new BK_PassengerParam { BookingID = inforbk.ID, Veid = item.ID };
                    daoPassenger.GetbyBooking(paramPassenger);
                    var listPassenger = paramPassenger.BK_PassengerInfos;
                    decimal tongtienhanhlysum = 0;
                    if (item.TypeID == Constant.Typeve.LuotDi)
                    {
                        var sum = listPassenger.Sum(z => z.BaggageDepartPrice);
                        tongtienhanhlysum = sum.HasValue ? sum.Value : 0;
                    }
                    else
                    {
                        var sum = listPassenger.Sum(z => z.BaggageReturnPrice);
                        tongtienhanhlysum = sum.HasValue ? sum.Value : 0;
                    }
                    tongtienhanhly += tongtienhanhlysum;

                    indexve++;
                }

                var bkdetail = new BK_BookingDetailDao();
                var listdetailbk = bkdetail.GetbylistbybookingjoinPassengers(inforbk.ID);

                var objPricenguoilon = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var objPricetreem = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var objPriceembe = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);

                var objPricenguoilonve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotVe);
                var objPricetreemve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotVe);
                var objPriceembeve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotVe);

                var Pricenguoilon = objPricenguoilon != null ? objPricenguoilon.Price : 0;
                var Pricetreem = objPricetreem != null ? objPricetreem.Price : 0;
                var Priceembe = objPriceembe != null ? objPriceembe.Price : 0;
                

                var Pricenguoilonve = objPricenguoilonve != null ? objPricenguoilonve.Price : 0;
                var Pricetreemve = objPricetreemve != null ? objPricetreemve.Price : 0;
                var Priceembeve = objPriceembeve != null ? objPriceembeve.Price : 0;
                var checkquoc = CheckQuocTe(inforbk.FromCity, inforbk.ToCity);
                if (checkquoc)
                {
                    Pricenguoilonve = 0;
                    Pricetreemve = 0;
                    Priceembeve = 0;
                }

                txttiengnuoilon.Text = ((Pricenguoilon ?? 0) + (Pricenguoilonve ?? 0)).ToString("n0");
                txttientreem.Text = ((Pricetreem ?? 0) + (Pricetreemve ?? 0)).ToString("n0");
                txttienembe.Text = ((Priceembe ?? 0) + (Priceembeve ?? 0)).ToString("n0");


                var coutnguoilon = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var counttreem = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var countembe = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);
                txtslnguoilon.Text = coutnguoilon.ToString("n0");
                txtsltreem.Text = counttreem.ToString("n0");
                txtsoluongembe.Text = countembe.ToString("n0");

                var tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon).Sum(z => z.Price);
                var tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm).Sum(z => z.Price);
                var tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe).Sum(z => z.Price);
                if (checkquoc)
                {
                    tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                }

                txttongtiennguoilon.Text = (tongtiennguoilon ?? 0).ToString("n0");
                txttongtientreem.Text = (tongtientreem ?? 0).ToString("n0");
                txttongtienembe.Text = (tongtientembe ?? 0).ToString("n0");
                txttonghanhly.Text = tongtienhanhly.ToString("n0") + " VND";
                var tongtien = inforbk.TotalPrice.HasValue ? inforbk.TotalPrice.Value : 0;
                txttongcongtien.Text = tongtien.ToString("n0") + " VND";
                var rptchitietve = new RptVe(inforbk.ID);
                subdanhsachve.ReportSource = rptchitietve;
                var RptChiTietGia = new RptChiTietGia(inforbk.ID);
                subchitietve.ReportSource = RptChiTietGia;
            }
            
        }
        public static bool CheckQuocTe(string codedi, string codeve)
        {
            bool quocte = false;
            if (!string.IsNullOrEmpty(codedi))
            {
                var dao = new StationDao();
                var objdi = dao.Getbycode(codedi);
                if (objdi != null)
                {
                    if (objdi.CountryID != 1)
                    {
                        quocte = true;
                        return quocte;
                    }
                }
            }
            if (!string.IsNullOrEmpty(codeve))
            {
                var dao = new StationDao();
                var objdi = dao.Getbycode(codeve);
                if (objdi != null)
                {
                    if (objdi.CountryID != 1)
                    {
                        quocte = true;
                        return quocte;
                    }
                }
            }
            return quocte;
        }
    }
}
