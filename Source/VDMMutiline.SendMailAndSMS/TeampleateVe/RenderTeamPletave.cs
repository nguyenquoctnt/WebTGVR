using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Ultilities;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;

namespace VDMMutiline.SendMailAndSMS.TeampleateVe
{
    public class RenderTeamPletave
    {
        public static string TeampleatevePDF(BK_BookingInfo BookingInfo, List<SettingUserInfo> SettingValue, int? Idtempleate, int? UserID)
        {
            //string str2 = System.IO.File.ReadAllText(Urlsource);

            var biz = new Biz.TempleteSetting.TempleateHTMLUserBiz();
            var obj = biz.SetupDisplayForm(Idtempleate, UserID);
            if (obj != null)
            {
                var bizUser = new Biz.AspNetUsersBiz();
                string PhoneNumber = "";
                var objUser = bizUser.GetInfoById(UserID);
                if (objUser != null)
                {
                    PhoneNumber = objUser.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(objUser.PhoneNumber2))
                        PhoneNumber += " - " + objUser.PhoneNumber2;
                }
                string str2 = obj.Value;
                NoiDungVePDF(BookingInfo, ref str2, SettingValue, PhoneNumber);
                return str2;
            }
            else
            {
                throw new Exception("Không tìm thấy dữ liệu vé của " + UserID.ToString());
            }
        }
        public static string Teampleateve(BK_BookingInfo BookingInfo, List<SettingUserInfo> SettingValue, int? Idtempleate, int? UserID)
        {
            //string str2 = System.IO.File.ReadAllText(Urlsource);

            var biz = new Biz.TempleteSetting.TempleateHTMLUserBiz();
            var obj = biz.SetupDisplayForm(Idtempleate, UserID);
            if (obj != null)
            {
                string str2 = obj.Value;
                NoiDungVe(BookingInfo, ref str2, SettingValue);
                return str2;
            }
            else
            {
                throw new Exception("Không tìm thấy dữ liệu vé của " + UserID.ToString());
            }
        }
        public static string Teampleateve(BK_BookingInfo BookingInfo, List<SettingUserInfo> SettingValue, int? Idtempleate, int? UserID, string ngaybay)
        {
            //string str2 = System.IO.File.ReadAllText(Urlsource);

            var biz = new Biz.TempleteSetting.TempleateHTMLUserBiz();
            var obj = biz.SetupDisplayForm(Idtempleate, UserID);
            if (obj != null)
            {
                string str2 = obj.Value;
                NoiDungVe(BookingInfo, ref str2, SettingValue, ngaybay);
                return str2;
            }
            else
            {
                throw new Exception("Không tìm thấy dữ liệu vé của " + UserID.ToString());
            }
        }
        public static void NoiDungVe(BK_BookingInfo BookingInfo, ref string template, List<SettingUserInfo> SettingValue)
        {
            if (BookingInfo != null)
            {
                var daoticket = new BK_TicketDao();
                var daoPassenger = new BK_PassengerDao();
                string urlfull = HttpContext.Current.Request.Url.AbsoluteUri;
                string path = HttpContext.Current.Request.Url.AbsolutePath;
                string url = urlfull.Replace(path, "");
                var bannerve = url + "/Content/bannerve.PNG";
                template = template.Replace("{linkbanner}", bannerve);
                template = template.Replace("{Sodienthoai}", BookingInfo.Phone);
                string trangthai = "";
                var CurentUser = CommonUI.GetInforByUserOrUserParent(BookingInfo.UserCreate);
                var setting = CommonUI.GetSettingByUser(CurentUser.UserName, true);
                template = template.Replace("{tencongty}", UIUty.GetsettingByKey(setting, "PRT_COMPANY_NAME"));
                string phone = "Tổng đài: ";
                phone += CurentUser.PhoneNumber;
                if (!string.IsNullOrEmpty(CurentUser.PhoneNumber2))
                    phone += "- Hotline:" + CurentUser.PhoneNumber2;
                template = template.Replace("{tongdai}", phone);
                template = template.Replace("{ngaybay}", phone);
                template = template.Replace("{SourceURL}", BookingInfo.SourceSite);
                if (!string.IsNullOrEmpty(BookingInfo.BookCode))
                {
                    trangthai = string.Format(@"{0}",
                        Utility.GetDictionaryValue(Constant.StatusVe.dctName, BookingInfo.Status));
                    if (BookingInfo.Status == Constant.StatusVe.DangGiuCho)
                    {
                        template = template.Replace("{TITELTHOIGIANGIUCHO}", "Dự kiến giữ đến: ");
                        template = template.Replace("{THOIGIANGIUCHO}", (BookingInfo.Expireddate.HasValue ? BookingInfo.Expireddate.Value.ToString("dd/MM/yyyy HH:mm") : ""));

                        string linkthanhtoan = "http://" + BookingInfo.SourceSite + "/ThanhToan/ChonHinhThucThanhToan?madonhang=" + BookingInfo.rCode;
                        string thanhtoanngay =
                            "<a href='" + linkthanhtoan + "'  style='text-align:center;font-size:18px;height: 12px;width:94px;font-size:11px;font-family:arial,helvetica,sans-serif;padding: 0px 8px 11px 9px;text-decoration:none;display:inline-block;font-weight:bold;color:#ffffff;margin-top:10px;margin-bottom:10px;background-color: #709a04;'>Thanh toán ngay</a>";
                        if (BookingInfo.Status == Constant.StatusVe.DangGiuCho)
                            template = template.Replace("{thanhtoanngay}", thanhtoanngay);
                        else
                        {
                            template = template.Replace("{thanhtoanngay}", "");
                            template = template.Replace("{TITELTHOIGIANGIUCHO}", "");
                        }
                    }
                    else
                    {
                        template = template.Replace("{TITELTHOIGIANGIUCHO}", "");
                        template = template.Replace("{THOIGIANGIUCHO}", "");
                        template = template.Replace("{thanhtoanngay}", "");
                    }
                }
                else
                {
                    template = template.Replace("{TITELTHOIGIANGIUCHO}", "");
                    template = template.Replace("{THOIGIANGIUCHO}", "");
                    template = template.Replace("{thanhtoanngay}", "");
                    trangthai = "Đặt vé thất bại.";
                }
                template = template.Replace("{TrangThai}", trangthai);
                template = template.Replace("{Madonhang}", BookingInfo.rCode);
                template = template.Replace("{TenKhachHang}", Utility.ConvertToUnsign(BookingInfo.Name).Trim());
                //template = template.Replace("{Hanthanhtoan}", BookingInfo.Expireddate.HasValue ? BookingInfo.Expireddate.Value.ToString("dd/MM/yyyy") : "");
                template = template.Replace("{MaDatCho}", BookingInfo.BookCode);
                var list = daoticket.GetListByBooking(BookingInfo.ID);
                string Thongtinhanhkhach = @"<tr style='font-weight:bold '>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Stt
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Giới tính
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Họ tên
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Xách tay
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Ký gửi
                    </td>
                </tr>";
                var listTicket = daoticket.GetListByBooking(BookingInfo.ID).OrderBy(z => z.ID);
                var listAirline = listTicket.Select(z => z.Code).ToList();

                var ishow12Kg = true;
                var Shchjq = "";
                if (listAirline.Count(z => z != "VN") == 0)
                {
                    ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
                    var listCheckBaggageVnJq = new List<ApiClient.Models.BaggageVnJq>();
                    var listTicketDetail = new List<BK_TicketDetail>();
                    foreach (var item in listTicket)
                    {
                        listCheckBaggageVnJq.AddRange(apiClient.GetBaggageVnJq(item.FromCity, item.ToCity));
                        var lidetial = daoticket.GetDetailByTicket(item.ID);
                        listTicketDetail.AddRange(lidetial);
                    }

                    foreach (var item in listTicketDetail)
                    {
                        var lst = listCheckBaggageVnJq.Where(z => z.StartPoind == item.FromCity && z.EndPoind == item.ToCity);
                        if (WebProject.Ultilities.BaggageVnUti.CheckBaggageVnJq(lst.ToList(), item.flight))
                        {
                            Shchjq += (item.flight + " - ");
                        }
                    }
                    foreach (var item in listTicketDetail)
                    {
                        var lst = listCheckBaggageVnJq.Where(z => z.StartPoind == item.FromCity && z.EndPoind == item.ToCity);
                        if (WebProject.Ultilities.BaggageVnUti.CheckBaggageVnJq(lst.ToList(), item.flight))
                        {
                            ishow12Kg = false;
                            break;
                        }
                    }

                }
                if (!ishow12Kg)
                {
                    template = template.Replace("{VnBayMayBayJQ}",
                         "Chuyến bay " + Shchjq + " là chuyến bay khai thác liên doanh giữa Vietnam Airlines và Jetstar, Hl xách tay 7kg, làm thủ tục tại quầy Jetstar");
                }
                else
                {
                    template = template.Replace("{VnBayMayBayJQ}", "");
                }
                foreach (var item in list)
                {
                    string hanhlyxachtay = "";
                    if (item.Code == "VN")
                    {
                        if (ishow12Kg)
                            hanhlyxachtay = "12kg";
                        else hanhlyxachtay = "7kg";
                    }   
                    else
                        hanhlyxachtay = "7kg";

                    var param = new BK_PassengerParam { BookingID = BookingInfo.ID, Veid = item.ID };
                    daoPassenger.GetbyBooking(param);
                    var stt = 1;
                    foreach (var itemhanhkhach in param.BK_PassengerInfos?.OrderBy(z => z.TypeID))
                    {

                        if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                        {
                            hanhlyxachtay = "";
                        }
                        string gioitinh = "";
                        if (itemhanhkhach.TypeID == Constant.TypePassenger.TreEm)
                            gioitinh = "Trẻ em";
                        else if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                        {
                            gioitinh = "Em bé";
                        }
                        else
                        {
                            if (itemhanhkhach.Sex == "Nam")
                            {

                                gioitinh = "ÔNG";
                            }
                            else
                            {
                                gioitinh = "BÀ";
                            }
                        }
                        string hanhlykygui = ((!string.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue)
                            ? itemhanhkhach.BaggageDepartValue
                            : "0") + "kg");
                        if (listTicket.Count(z => z.TypeID == Constant.Typeve.LuotVe) > 0)
                        {
                            if (!string.IsNullOrEmpty(itemhanhkhach.BaggageReturnValue))
                                hanhlykygui += (" - " + itemhanhkhach.BaggageReturnValue + "kg");
                            else hanhlykygui += (" - 0kg");
                        }
                        //if (item.Code == "VN")
                        //{
                        //    if (String.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue))
                        //        hanhlykygui = "20kg";
                        //}
                        Thongtinhanhkhach += string.Format(@"<tr>
                    <td style='padding:5px'>
                        {0}
                    </td>
                    <td style='padding:5px'>
                      {1}
                    </td>
                    <td style='padding:5px'>
                        {2}
                    </td>
                    <td style='padding:5px'>
                       {3}
                    </td>
 <td style='padding:5px'>
                       {4}
                    </td>
                </tr>", stt,
                      gioitinh,
                      (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper(),
                      hanhlyxachtay,
                     hanhlykygui);
                        stt++;
                    }
                    break;
                }
                template = template.Replace("{Danhsachkhachhang}", Thongtinhanhkhach);
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
                    var paramPassenger = new BK_PassengerParam { BookingID = BookingInfo.ID, Veid = item.ID };
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
                string chitietve = @"
                <tr style='font-weight:bold'>
                    <td style='border: 1px solid gray;padding:5px'>
                        Số hiệu
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Ngày bay
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Hạng vé
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Điểm đi
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Điểm đến
                    </td>
                </tr>";
                foreach (var item in listTicket)
                {
                    string Luot = "";
                    if (item.TypeID == Constant.Typeve.LuotDi)
                    {

                        Luot = "<img  style='width: 50px; height: 30px;' src='http://" + BookingInfo.SourceSite + UIUty.GetImagebyHang(item.Code) + "'>";
                    }
                    else if (item.TypeID == Constant.Typeve.LuotVe)
                    {
                        Luot = "<img  style='width: 50px; height: 30px;' src='http://" + BookingInfo.SourceSite + UIUty.GetImagebyHang(item.Code) + "'>";
                    }
                    chitietve += string.Format(@"<tr style='font-weight:bold'>
                    <td colspan='2' style='padding:5px'>
                        {0}
                    </td>
                    <td colspan='3' style='padding:5px;text-align:left'>
                       Airline:
                        <span>{1}</span>
                    </td>
                </tr>
                <tr>
                    <td style='padding:5px'>
                        <span>{2}</span>
                    </td>
                    <td style='padding:5px'>
                        <span>{3}</span>
                    </td>
                    <td style='padding:5px'>
                        <span>{4}</span>
                    </td>
                    <td style='padding:5px'>
                        <span>{5}</span>
                        <span>{6}</span>
                         {7} ({8})
                    </td>
                    <td style='padding:5px'>
                        <span>{9}</span>
                        <span>{10}</span>
                        {11} ({12})
                    </td>
                </tr>", Luot, UIUty.GetNamebyHang(item.Code), item.FlightNo,
                      item.StartDate.HasValue ? item.StartDate.Value.ToString("dd/MM/yyyy") : "",
                      item.Class,
                      item.StartDate.HasValue ? item.StartDate.Value.ToString("HH:mm") : "",
                      "",
                       getsanbaycode(item.FromCity)
                       , item.FromCity,
                       item.EndDate.HasValue ? item.EndDate.Value.ToString("HH:mm") : "",
                       "",
                       getsanbaycode(item.ToCity),
                       item.ToCity);

                }
                template = template.Replace("{ChiTietVe}", chitietve);
                template = template.Replace("{TienHanhLy}", tongtienhanhly.ToString("n0") + " VND");

                var bkdetail = new BK_BookingDetailDao();
                var listdetailbk = bkdetail.GetbylistbybookingjoinPassengers(BookingInfo.ID);
                var objPricenguoilon = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var objPricetreem = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var objPriceembe = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);


                var objPricenguoilonve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotVe);
                var objPricetreemve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotVe);
                var objPriceembeve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotVe);

                var checkquoc = Station.CheckQuocTe(BookingInfo.FromCity, BookingInfo.ToCity);



                var Pricenguoilon = objPricenguoilon != null ? objPricenguoilon.Price : 0;
                var Pricetreem = objPricetreem != null ? objPricetreem.Price : 0;
                var Priceembe = objPriceembe != null ? objPriceembe.Price : 0;

                var Pricenguoilonve = objPricenguoilonve != null ? objPricenguoilonve.Price : 0;
                var Pricetreemve = objPricetreemve != null ? objPricetreemve.Price : 0;
                var Priceembeve = objPriceembeve != null ? objPriceembeve.Price : 0;
                if (checkquoc)
                {
                    Pricenguoilonve = 0;
                    Pricetreemve = 0;
                    Priceembeve = 0;
                }
                template = template.Replace("{Gianguoilon}", ((Pricenguoilon ?? 0) + (Pricenguoilonve ?? 0)).ToString("n0"));
                template = template.Replace("{Giatreem}", ((Pricetreem ?? 0) + (Pricetreemve ?? 0)).ToString("n0"));
                template = template.Replace("{Giaembe}", ((Priceembe ?? 0) + (Priceembeve ?? 0)).ToString("n0"));

                var coutnguoilon = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var counttreem = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var countembe = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);

                template = template.Replace("{Soluongnguoilon}", coutnguoilon.ToString("n0"));
                template = template.Replace("{Soluongtreem}", counttreem.ToString("n0"));
                template = template.Replace("{Soluongembe}", countembe.ToString("n0"));
                var tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon).Sum(z => z.Price);
                var tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm).Sum(z => z.Price);
                var tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe).Sum(z => z.Price);
                if (checkquoc)
                {
                    tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                }
                template = template.Replace("{TongTienNguoiLon}", (tongtiennguoilon ?? 0).ToString("n0"));
                template = template.Replace("{TongTientreem}", (tongtientreem ?? 0).ToString("n0"));
                template = template.Replace("{TongTienembe}", (tongtientembe ?? 0).ToString("n0"));

                var tongtien = BookingInfo.TotalPrice.HasValue ? BookingInfo.TotalPrice.Value : 0;
                template = template.Replace("{TongCongTien}", tongtien.ToString("n0") + " VND");
                template = template.Replace("{TongTienVe}", (tongtien - (double)tongtienhanhly).ToString("n0") + " VND");
                template = template.Replace("{NGAYDAT}", BookingInfo.CreatedDate.HasValue ? BookingInfo.CreatedDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }
        public static void NoiDungVe(BK_BookingInfo BookingInfo, ref string template, List<SettingUserInfo> SettingValue, string ngaybay)
        {
            if (BookingInfo != null)
            {
                var daoticket = new BK_TicketDao();
                var daoPassenger = new BK_PassengerDao();
                string urlfull = HttpContext.Current.Request.Url.AbsoluteUri;
                string path = HttpContext.Current.Request.Url.AbsolutePath;
                string url = urlfull.Replace(path, "");
                var bannerve = url + "/Content/bannerve.PNG";
                template = template.Replace("{linkbanner}", bannerve);
                template = template.Replace("{Sodienthoai}", BookingInfo.Phone);
                string trangthai = "";
                var CurentUser = CommonUI.GetInforByUserOrUserParent(BookingInfo.UserCreate);
                var setting = CommonUI.GetSettingByUser(CurentUser.UserName, true);
                template = template.Replace("{tencongty}", UIUty.GetsettingByKey(setting, "PRT_COMPANY_NAME"));
                string phone = "Tổng đài: ";
                phone += CurentUser.PhoneNumber;
                if (!string.IsNullOrEmpty(CurentUser.PhoneNumber2))
                    phone += "- Hotline:" + CurentUser.PhoneNumber2;
                template = template.Replace("{tongdai}", phone);
                template = template.Replace("{ngaybay}", ngaybay);
                template = template.Replace("{SourceURL}", BookingInfo.SourceSite);

                if (!string.IsNullOrEmpty(BookingInfo.BookCode))
                {
                    trangthai = string.Format(@"{0}",
                        Utility.GetDictionaryValue(Constant.StatusVe.dctName, BookingInfo.Status));
                    if (BookingInfo.Status == Constant.StatusVe.DangGiuCho)
                    {
                        template = template.Replace("{TITELTHOIGIANGIUCHO}", "Dự kiến giữ đến: ");
                        template = template.Replace("{THOIGIANGIUCHO}", (BookingInfo.Expireddate.HasValue ? BookingInfo.Expireddate.Value.ToString("dd/MM/yyyy HH:mm") : ""));

                        string linkthanhtoan = "http://" + BookingInfo.SourceSite + "/ThanhToan/ChonHinhThucThanhToan?madonhang=" + BookingInfo.rCode;
                        string thanhtoanngay =
                            "<a href='" + linkthanhtoan + "'  style='text-align:center;font-size:18px;height: 12px;width:94px;font-size:11px;font-family:arial,helvetica,sans-serif;padding: 0px 8px 11px 9px;text-decoration:none;display:inline-block;font-weight:bold;color:#ffffff;margin-top:10px;margin-bottom:10px;background-color: #709a04;'>Thanh toán ngay</a>";
                        if (BookingInfo.Status == Constant.StatusVe.DangGiuCho)
                            template = template.Replace("{thanhtoanngay}", thanhtoanngay);
                        else
                        {
                            template = template.Replace("{thanhtoanngay}", "");
                            template = template.Replace("{TITELTHOIGIANGIUCHO}", "");
                        }
                    }
                    else
                    {
                        template = template.Replace("{TITELTHOIGIANGIUCHO}", "");
                        template = template.Replace("{THOIGIANGIUCHO}", "");
                        template = template.Replace("{thanhtoanngay}", "");
                    }
                }
                else
                {
                    template = template.Replace("{TITELTHOIGIANGIUCHO}", "");
                    template = template.Replace("{THOIGIANGIUCHO}", "");
                    template = template.Replace("{thanhtoanngay}", "");
                    trangthai = "Đặt vé thất bại.";
                }
                template = template.Replace("{TrangThai}", trangthai);
                template = template.Replace("{Madonhang}", BookingInfo.rCode);
                template = template.Replace("{TenKhachHang}", Utility.ConvertToUnsign(BookingInfo.Name).Trim());
                //template = template.Replace("{Hanthanhtoan}", BookingInfo.Expireddate.HasValue ? BookingInfo.Expireddate.Value.ToString("dd/MM/yyyy") : "");
                template = template.Replace("{MaDatCho}", BookingInfo.BookCode);
                var list = daoticket.GetListByBooking(BookingInfo.ID);
                string Thongtinhanhkhach = @"<tr style='font-weight:bold '>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Stt
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Giới tính
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Họ tên
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Xách tay
                    </td>
                    <td style='border: 1px solid gray;padding:5px;'>
                        Ký gửi
                    </td>
                </tr>";


                var listTicket = daoticket.GetListByBooking(BookingInfo.ID).OrderBy(z => z.ID);

                var listAirline = listTicket.Select(z => z.Code).ToList();

                var ishow12Kg = true;
                var Shchjq = "";
                if (listAirline.Count(z => z != "VN") == 0)
                {
                    ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
                    var listCheckBaggageVnJq = new List<ApiClient.Models.BaggageVnJq>();
                    var listTicketDetail = new List<BK_TicketDetail>();
                    foreach (var item in listTicket)
                    {
                        listCheckBaggageVnJq.AddRange(apiClient.GetBaggageVnJq(item.FromCity, item.ToCity));
                        var lidetial = daoticket.GetDetailByTicket(item.ID);
                        listTicketDetail.AddRange(lidetial);
                    }

                    foreach (var item in listTicketDetail)
                    {
                        var lst = listCheckBaggageVnJq.Where(z => z.StartPoind == item.FromCity && z.EndPoind == item.ToCity);
                        if (WebProject.Ultilities.BaggageVnUti.CheckBaggageVnJq(lst.ToList(), item.flight))
                        {
                            Shchjq += (item.flight + " - ");
                        }
                    }
                    foreach (var item in listTicketDetail)
                    {
                        var lst = listCheckBaggageVnJq.Where(z => z.StartPoind == item.FromCity && z.EndPoind == item.ToCity);
                        if (WebProject.Ultilities.BaggageVnUti.CheckBaggageVnJq(lst.ToList(), item.flight))
                        {
                            ishow12Kg = false;
                            break;
                        }
                    }

                }
                if (!ishow12Kg)
                {
                    template = template.Replace("{VnBayMayBayJQ}",
                         "Chuyến bay " + Shchjq + " là chuyến bay khai thác liên doanh giữa Vietnam Airlines và Jetstar, Hl xách tay 7kg, làm thủ tục tại quầy Jetstar");
                }
                else
                {
                    template = template.Replace("{VnBayMayBayJQ}", "");
                }
                foreach (var item in list)
                {
                    string hanhlyxachtay = "";
                    if (item.Code == "VN")
                    {
                        if (ishow12Kg)
                            hanhlyxachtay = "12kg";
                        else hanhlyxachtay = "7kg";
                    }   
                    else
                        hanhlyxachtay = "7kg";
                    var param = new BK_PassengerParam { BookingID = BookingInfo.ID, Veid = item.ID };
                    daoPassenger.GetbyBooking(param);
                    var stt = 1;
                    foreach (var itemhanhkhach in param.BK_PassengerInfos?.OrderBy(z=> z.TypeID))
                    {

                        if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                        {
                            hanhlyxachtay = "";
                        }
                        string gioitinh = "";
                        if (itemhanhkhach.TypeID == Constant.TypePassenger.TreEm)
                            gioitinh = "Trẻ em";
                        else if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                        {
                            gioitinh = "Em bé";
                        }
                        else
                        {
                            if (itemhanhkhach.Sex == "Nam")
                            {

                                gioitinh = "ÔNG";
                            }
                            else
                            {
                                gioitinh = "BÀ";
                            }
                        }
                        string hanhlykygui = ((!string.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue)
                            ? itemhanhkhach.BaggageDepartValue
                            : "0") + "kg");
                        if (listTicket.Count(z => z.TypeID == Constant.Typeve.LuotVe) > 0)
                        {
                            if (!string.IsNullOrEmpty(itemhanhkhach.BaggageReturnValue))
                                hanhlykygui += (" - " + itemhanhkhach.BaggageReturnValue + "kg");
                            else hanhlykygui += (" - 0kg");
                        }

                        Thongtinhanhkhach += string.Format(@"<tr>
                    <td style='padding:5px'>
                        {0}
                    </td>
                    <td style='padding:5px'>
                      {1}
                    </td>
                    <td style='padding:5px'>
                        {2}
                    </td>
                    <td style='padding:5px'>
                       {3}
                    </td>
 <td style='padding:5px'>
                       {4}
                    </td>
                </tr>", stt,
                      gioitinh,
                      (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper(),
                      hanhlyxachtay,
                     hanhlykygui);
                        stt++;
                    }
                    break;
                }
                template = template.Replace("{Danhsachkhachhang}", Thongtinhanhkhach);

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
                    var paramPassenger = new BK_PassengerParam { BookingID = BookingInfo.ID, Veid = item.ID };
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
                string chitietve = @"
                <tr style='font-weight:bold'>
                    <td style='border: 1px solid gray;padding:5px'>
                        Số hiệu
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Ngày bay
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Hạng vé
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Điểm đi
                    </td>
                    <td style='border: 1px solid gray;padding:5px'>
                        Điểm đến
                    </td>
                </tr>";
                foreach (var item in listTicket)
                {
                    string Luot = "";
                    if (item.TypeID == Constant.Typeve.LuotDi)
                    {
                        Luot = "<img  style='width: 50px; height: 30px;' src='http://" + BookingInfo.SourceSite + UIUty.GetImagebyHang(item.Code) + "'>";
                    }
                    else if (item.TypeID == Constant.Typeve.LuotVe)
                    {
                        Luot = "<img  style='width: 50px; height: 30px;' src='http://" + BookingInfo.SourceSite + UIUty.GetImagebyHang(item.Code) + "'>";
                    }
                    chitietve += string.Format(@"<tr style='font-weight:bold'>
                    <td colspan='2' style='padding:5px'>
                        {0}
                    </td>
                    <td colspan='3' style='padding:5px;text-align:left'>
                       Airline:
                        <span>{1}</span>
                    </td>
                </tr>
                <tr>
                    <td style='padding:5px'>
                        <span>{2}</span>
                    </td>
                    <td style='padding:5px'>
                        <span>{3}</span>
                    </td>
                    <td style='padding:5px'>
                        <span>{4}</span>
                    </td>
                    <td style='padding:5px'>
                        <span>{5}</span>
                        <span>{6}</span>
                         {7} ({8})
                    </td>
                    <td style='padding:5px'>
                        <span>{9}</span>
                        <span>{10}</span>
                        {11} ({12})
                    </td>
                </tr>", Luot, UIUty.GetNamebyHang(item.Code), item.FlightNo,
                      item.StartDate.HasValue ? item.StartDate.Value.ToString("dd/MM/yyyy") : "",
                      item.Class,
                      item.StartDate.HasValue ? item.StartDate.Value.ToString("HH:mm") : "",
                      "",
                       getsanbaycode(item.FromCity)
                       , item.FromCity,
                       item.EndDate.HasValue ? item.EndDate.Value.ToString("HH:mm") : "",
                       "",
                       getsanbaycode(item.ToCity),
                       item.ToCity);

                }
                template = template.Replace("{ChiTietVe}", chitietve);
                template = template.Replace("{TienHanhLy}", tongtienhanhly.ToString("n0") + " VND");

                var bkdetail = new BK_BookingDetailDao();
                var listdetailbk = bkdetail.GetbylistbybookingjoinPassengers(BookingInfo.ID);
                var objPricenguoilon = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var objPricetreem = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var objPriceembe = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);


                var objPricenguoilonve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotVe);
                var objPricetreemve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotVe);
                var objPriceembeve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotVe);

                var checkquoc = Station.CheckQuocTe(BookingInfo.FromCity, BookingInfo.ToCity);



                var Pricenguoilon = objPricenguoilon != null ? objPricenguoilon.Price : 0;
                var Pricetreem = objPricetreem != null ? objPricetreem.Price : 0;
                var Priceembe = objPriceembe != null ? objPriceembe.Price : 0;

                var Pricenguoilonve = objPricenguoilonve != null ? objPricenguoilonve.Price : 0;
                var Pricetreemve = objPricetreemve != null ? objPricetreemve.Price : 0;
                var Priceembeve = objPriceembeve != null ? objPriceembeve.Price : 0;
                if (checkquoc)
                {
                    Pricenguoilonve = 0;
                    Pricetreemve = 0;
                    Priceembeve = 0;
                }
                template = template.Replace("{Gianguoilon}", ((Pricenguoilon ?? 0) + (Pricenguoilonve ?? 0)).ToString("n0"));
                template = template.Replace("{Giatreem}", ((Pricetreem ?? 0) + (Pricetreemve ?? 0)).ToString("n0"));
                template = template.Replace("{Giaembe}", ((Priceembe ?? 0) + (Priceembeve ?? 0)).ToString("n0"));

                var coutnguoilon = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var counttreem = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var countembe = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);

                template = template.Replace("{Soluongnguoilon}", coutnguoilon.ToString("n0"));
                template = template.Replace("{Soluongtreem}", counttreem.ToString("n0"));
                template = template.Replace("{Soluongembe}", countembe.ToString("n0"));
                var tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon).Sum(z => z.Price);
                var tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm).Sum(z => z.Price);
                var tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe).Sum(z => z.Price);
                if (checkquoc)
                {
                    tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                }
                template = template.Replace("{TongTienNguoiLon}", (tongtiennguoilon ?? 0).ToString("n0"));
                template = template.Replace("{TongTientreem}", (tongtientreem ?? 0).ToString("n0"));
                template = template.Replace("{TongTienembe}", (tongtientembe ?? 0).ToString("n0"));
                var tongtien = BookingInfo.TotalPrice.HasValue ? BookingInfo.TotalPrice.Value : 0;
                template = template.Replace("{TongCongTien}", tongtien.ToString("n0") + " VND");
                template = template.Replace("{TongTienVe}", (tongtien - (double)tongtienhanhly).ToString("n0") + " VND");
                template = template.Replace("{NGAYDAT}", BookingInfo.CreatedDate.HasValue ? BookingInfo.CreatedDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }
        private static string getsanbaycode(string code)
        {
            var obj = Station.Getbycode(code);
            if (obj != null)
                return obj.AirportName;
            return "";
        }
        public static void Getlisthanhkhachbybooking(int? type, int bookingid, int veid, ref string Khachhang, ref string HanhLy, ref string gioitinh)
        {
            var dao = new BK_PassengerDao();
            var param = new BK_PassengerParam { BookingID = bookingid, Veid = veid };
            dao.GetbyBooking(param);
            foreach (var item in param.BK_PassengerInfos?.OrderBy(z => z.TypeID))
            {
                Khachhang += (item.FirstName + " " + item.Name).ToUpper() + "\n";
                if (type == Constant.Typeve.LuotDi)
                {
                    if (!string.IsNullOrEmpty(item.BaggageDepartValue))
                        HanhLy += (item.BaggageDepartValue + " kg") + "\n";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.BaggageReturnValue))
                        HanhLy += (item.BaggageReturnValue + " kg") + "\n";
                }
                if (item.Sex == "Nam")
                {
                    gioitinh = "ÔNG";
                }
                else
                {
                    gioitinh = "BÀ";
                }
            }
        }
        private static string GetDanhSachKhachHangPDF2(string gioitinh, int? type, string Name)
        {
            return string.Format(@"<tr align='left'>
                    <th style='font-size:18px;font-weight: 100;text-transform: uppercase; '>{0} {1}</th>
                </tr>", GetGioiTinhKhachHang(gioitinh, type), Name);
        }
        private static string GetChangBayPdf2(BK_TicketDao ticketDao, BK_BookingInfo BookingInfo, BK_TicketInfo Info, string FromName, string ToName)
        {
            var item = "";
            if (Info != null)
            {
                var listDetail = ticketDao.GetDetailByTicket(Info.ID);
                foreach (var detail in listDetail)
                {
                    string NameHang = UIUty.GetNamebyHang(detail.airlineCode);
                    string LogoHang = "http://" + BookingInfo.SourceSite + UIUty.GetImagebyHang(detail.airlineCode);
                    string NgayBay = (detail.StartDate.Value.Day + Utility.MonthName(detail.StartDate.Value.Month) + detail.StartDate.Value.Year).ToUpper();
                    DateTime ngay = detail.StartDate.Value;
                    DateTime endtime = detail.EndDate.Value;
                    TimeSpan time = endtime - ngay;
                    string Duration = (time.Hours + "hr(s)" + time.Minutes + "min(s)");
                    string GioDi = detail.StartDate.HasValue ? detail.StartDate.Value.ToString("HH:mm") : "";
                    string GioDen = detail.EndDate.HasValue ? detail.EndDate.Value.ToString("HH:mm") : "";

                    string NgayDi = detail.StartDate.HasValue ? detail.StartDate.Value.ToString("dd/MM/yyyy") : "";
                    string NgayDen = detail.EndDate.HasValue ? detail.EndDate.Value.ToString("dd/MM/yyyy") : "";
                    item += string.Format(@"<table style='width: 100%;    border-spacing: 0px 0px;margin-top:10px'>
            <tbody>
                <tr>
                    <td style='border-bottom: 1px solid;padding-bottom: 5px;font-size: 20px;font-weight: bold;text-transform: uppercase;' colspan='4'>
                        <div style='float:left'>
                            <img style='width: 30px;' src='http://id.tgvr.net/Content/ICONPDF/iconFlightPDF.png'>
                        </div>
                        <div style='float:left;margin-top: 5px;margin-left: 5px;'>
                            FLIGHT - {0} {1} {2}
                        </div>

                    </td>
                </tr>
                <tr style='height: 20.8px;'>
                    <td style='    border-left: 1px solid;text-align:center;width: 220px;'>
                        <img alt='Airline logo' style='width: 125px;' src='{3}'>
                        <br>
                        <span>
                            {4}
                        </span>
                    </td>
                    <td style='border: 1px solid;border-top: 0px;padding: 5px;border-right: 0px;' colspan='2'>
                        <table style='width: 100%;'>
                            <tbody><tr>
                                <td style='text-align:center;width:150px'>
                                    <span> Depart</span><br>
                                    <b style='font-size: 35px;'> {5}</b>
                                </td>
                                <td style='text-align:center;font-size: 17px;line-height: 25px;'>
                                    <img style='width: 128px;height: 25px;' src='http://id.tgvr.net/Content/ICONPDF/ArrowNext.png'><br>
                                    <span>{6}</span>
                                </td>
                                <td style='text-align:center;width:150px'>
                                    <span> Arrive at</span><br>
                                    <b style='font-size: 35px;'> {7}</b>
                                </td>
                            </tr>
                        </tbody></table>
                    </td>
                    <td style='border-left: 1px solid;font-size: 18px;border-right: 1px solid;width: 220px;border-bottom: 1px solid;background-color: #DCDCDC;line-height: 25px;padding: 5px;' rowspan='2'>
                        <span>Class Of Service</span> <br>
                        <span><b>{8}</b></span><br>
                        <span>	Plane</span> <br>
                        <span><b>{9}</b></span><br>
                        <span>	Seat</span> <br>
                        <span><b>Check - in required</b></span>
                    </td>
                </tr>
                <tr>
                    <td style='font-size: 18px;border-bottom: 1px solid;border-left: 1px solid;line-height: 25px;padding: 5px;'>
                        <span> Flight number:<b>{10}</b></span><br>
                        <span> Airline Code:<b>{11}</b></span><br>
                        <span> Status:<b>CONFIRM</b></span><br>
                        <span> Duration:<b>{12}</b></span>
                    </td>
                    <td style='font-size: 18px;border: 1px solid;border-right: 0px;border-top: 0px;    padding: 5px;padding-top: 0px;    padding-bottom: 0px;' colspan='2'>
					<div style='width: 49%;float: left;border-right: 1px solid;min-height: 97px;padding-top: 15px;'>
								 <span>{13}</span> <br>
                        		 <span><b style='    font-size: 30px;'>{14}</b></span><br>
                        		 <span>{15}</span>
							</div>
							<div style='width: 49%;float: right;min-height: 97px;padding-top: 15px;'>
								 <span>{16}</span> <br>
                        		 <span><b style='    font-size: 30px;'>{17}</b></span><br>
                        		 <span>{18}</span>
							</div>
                    </td>
                </tr>
            </tbody>
        </table>", NameHang, detail.flight, NgayBay, LogoHang, NameHang, detail.FromCity, Duration, detail.ToCity,
            detail.Class, detail.Plane, detail.flight, detail.airlineCode, Duration, FromName, GioDi, NgayDi, ToName, GioDen, NgayDen);
                }
            }
            return item;
        }
        private static string GetGioiTinhKhachHang(string gioitinh, int? type)
        {
            if (type == Constant.TypePassenger.EmBe)
            {
                return "INF";
            }
            else if (type == Constant.TypePassenger.TreEm)
            {
                if (gioitinh == Constant.Gender.Female)
                    return "MISS";
                if (gioitinh == Constant.Gender.Male)
                    return "MSTR";
            }
            else
            {
                if (gioitinh == Constant.Gender.Female)
                    return "MS";
                if (gioitinh == Constant.Gender.Male)
                    return "MR";
            }
            return "";

        }
        public static void NoiDungVePDF(BK_BookingInfo BookingInfo, ref string template, List<SettingUserInfo> SettingValue, string PhoneNumber)
        {
            if (BookingInfo != null)
            {
                var CurentUser = CommonUI.GetInforByUserOrUserParent(BookingInfo.UserCreate);
                var setting = CommonUI.GetSettingByUser(CurentUser.UserName, true);
                var common = CommonUI.GetlistTempleate(5, CurentUser.Id);
                template = @"<style>  .headertop {background: " + CommonUI.Getsettingtempleatebykey(common, "{Menu_BgColor}") + ";padding: 10px;}.colormain {color: " + CommonUI.Getsettingtempleatebykey(common, "{Menu_TextColor}") + "} .headertitel {background: " + CommonUI.Getsettingtempleatebykey(common, "{Menu_BgColor}") + ";color: " + CommonUI.Getsettingtempleatebykey(common, "{Menu_TextColor}") + ";padding: 5px;margin-bottom: 5px; margin-top: 5px;}  </style>   " + template;
                var daoticket = new BK_TicketDao();
                var daoPassenger = new BK_PassengerDao();
                template = template.Replace("{UrlLogo}", "http://" + BookingInfo.SourceSite + "/" + CurentUser.LogoUrl);
                template = template.Replace("{tongdai}", CurentUser.PhoneNumber);
                if (!string.IsNullOrEmpty(BookingInfo.BookCode))
                    template = template.Replace("{MaDatCho}", BookingInfo.BookCode);
                else template = template.Replace("{MaDatCho}", BookingInfo.rCode);
                template = template.Replace("{SourceURL}", "http://" + BookingInfo.SourceSite);
                string trangthai = "";
                if (!string.IsNullOrEmpty(BookingInfo.BookCode))
                {
                    trangthai = string.Format(@"{0}",
                        Utility.GetDictionaryValue(Constant.StatusVe.dctName, BookingInfo.Status));
                }
                else
                {
                    trangthai = "Đặt vé thất bại.";
                }
                var StartPoindOBJ = VDMMutiline.Ultilities.Station.Getbycode(BookingInfo.FromCity);
                if (StartPoindOBJ != null)
                {
                    var StartPoind = Utility.ConvertToUnsign(StartPoindOBJ.AirportName + "," + StartPoindOBJ.CountryName);
                    template = template.Replace("{DiemDiPDF2}", StartPoind);
                }
                var EndPoindOBJ = VDMMutiline.Ultilities.Station.Getbycode(BookingInfo.ToCity);
                if (EndPoindOBJ != null)
                {
                    var EndPoind = Utility.ConvertToUnsign(EndPoindOBJ.AirportName + "," + EndPoindOBJ.CountryName);
                    template = template.Replace("{DiemDenPDF2}", EndPoind);
                }
                template = template.Replace("{THOIGIANGIUCHO}", (BookingInfo.Expireddate.HasValue ? BookingInfo.Expireddate.Value.ToString("dd/MM/yyyy HH:mm") : ""));
                template = template.Replace("{TrangThai}", trangthai);
                template = template.Replace("{NGAYDAT}", BookingInfo.CreatedDate.HasValue ? BookingInfo.CreatedDate.Value.ToString("dd/MM/yyyy") : "");
                template = template.Replace("{Sodienthoai}", BookingInfo.Phone);
                template = template.Replace("{TenKhachHang}", Utility.ConvertToUnsign(BookingInfo.Name).Trim());
                template = template.Replace("{EMAIL}", BookingInfo.Email);

                var list = daoticket.GetListByBooking(BookingInfo.ID);
                string Thongtinhanhkhach = @"";
                string ThongtinhanhkhachFULL = @"";
                string DanhSachKhachHangPDF2 = "";
                string AddOnPdf2 = "";
                var listTicket = daoticket.GetListByBooking(BookingInfo.ID).OrderBy(z => z.ID);
                var listAirline = listTicket.Select(z => z.Code).ToList();

                var ishow12Kg = true;
                var Shchjq = "";
                if (listAirline.Count(z => z != "VN") == 0)
                {
                    ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
                    var listCheckBaggageVnJq = new List<ApiClient.Models.BaggageVnJq>();
                    var listTicketDetail = new List<BK_TicketDetail>();
                    foreach (var item in listTicket)
                    {
                        listCheckBaggageVnJq.AddRange(apiClient.GetBaggageVnJq(item.FromCity, item.ToCity));
                        var lidetial = daoticket.GetDetailByTicket(item.ID);
                        listTicketDetail.AddRange(lidetial);
                    }
                    
                    foreach (var item in listTicketDetail)
                    {
                        var lst = listCheckBaggageVnJq.Where(z => z.StartPoind == item.FromCity && z.EndPoind == item.ToCity);
                        if (WebProject.Ultilities.BaggageVnUti.CheckBaggageVnJq(lst.ToList(), item.flight))
                        {
                            Shchjq += (item.flight + " - ");
                        }
                    }
                    foreach (var item in listTicketDetail)
                    {
                        var lst = listCheckBaggageVnJq.Where(z => z.StartPoind == item.FromCity && z.EndPoind == item.ToCity);
                        if (WebProject.Ultilities.BaggageVnUti.CheckBaggageVnJq(lst.ToList(), item.flight))
                        {
                            ishow12Kg = false;
                            break;
                        }
                    }
                   
                }
                if (!ishow12Kg)
                {
                    template = template.Replace("{VnBayMayBayJQ}",
                         "Chuyến bay " + Shchjq + " là chuyến bay khai thác liên doanh giữa Vietnam Airlines và Jetstar, Hl xách tay 7kg, làm thủ tục tại quầy Jetstar");
                }
                else
                {
                    template = template.Replace("{VnBayMayBayJQ}", "");
                }                    
                foreach (var item in list)
                {
                    string hanhlyxachtay = "";
                    if (item.Code == "VN")
                    {
                        if (ishow12Kg)
                            hanhlyxachtay = "12kg";
                        else hanhlyxachtay = "7kg";
                    }
                    else
                        hanhlyxachtay = "7kg";

                    var param = new BK_PassengerParam { BookingID = BookingInfo.ID, Veid = item.ID };
                    daoPassenger.GetbyBooking(param);
                    var stt = 1;
                    foreach (var itemhanhkhach in param.BK_PassengerInfos?.OrderBy(z => z.TypeID))
                    {
                        if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                        {
                            hanhlyxachtay = "";
                        }
                        string gioitinh = "";
                        if (itemhanhkhach.TypeID == Constant.TypePassenger.TreEm)
                            gioitinh = "Trẻ em";
                        else if (itemhanhkhach.TypeID == Constant.TypePassenger.EmBe)
                        {
                            gioitinh = "Em bé";
                        }
                        else
                        {
                            if (itemhanhkhach.Sex == "Nam")
                            {

                                gioitinh = "ÔNG";
                            }
                            else
                            {
                                gioitinh = "BÀ";
                            }
                        }
                        string hanhlykyguichieudi = ((!string.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue)
                            ? itemhanhkhach.BaggageDepartValue
                            : "0") + "kg");
                        string hanhlykyguichieuve = "0kg";
                        if (listTicket.Count(z => z.TypeID == Constant.Typeve.LuotVe) > 0)
                        {
                            if (!string.IsNullOrEmpty(itemhanhkhach.BaggageReturnValue))
                                hanhlykyguichieuve = (itemhanhkhach.BaggageReturnValue + "kg");
                        }

                        Thongtinhanhkhach += string.Format(@" <tr>
                    <td style='font-size: 20px;'>{0}</td>
                    <td style='text-align:center;'>{3}</td>
                    <td style='text-align:center;'>{1}</td>
                    <td style='text-align:center;'>{2}</td>
                </tr>", (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper(), hanhlykyguichieudi, hanhlykyguichieuve, hanhlyxachtay);


                        string hanhlykygui = ((!string.IsNullOrEmpty(itemhanhkhach.BaggageDepartValue)
                          ? itemhanhkhach.BaggageDepartValue
                          : "0") + "kg");
                        if (listTicket.Count(z => z.TypeID == Constant.Typeve.LuotVe) > 0)
                        {
                            if (!string.IsNullOrEmpty(itemhanhkhach.BaggageReturnValue))
                                hanhlykygui += (" - " + itemhanhkhach.BaggageReturnValue + "kg");
                            else hanhlykygui += (" - 0kg");
                        }
                        ThongtinhanhkhachFULL += string.Format(@"<tr>
                    <td style='padding:5px'>
                        {0}
                    </td>
                    <td style='padding:5px'>
                      {1}
                    </td>
                    <td style='padding:5px'>
                        {2}
                    </td>
                    <td style='padding:5px'>
                       {3}
                    </td>
 <td style='padding:5px'>
                       {4}
                    </td>
                </tr>", stt,
                      gioitinh,
                      (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper(), hanhlyxachtay, hanhlykygui);
                        DanhSachKhachHangPDF2 += GetDanhSachKhachHangPDF2(itemhanhkhach.Sex, itemhanhkhach.TypeID, (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper());
                        AddOnPdf2 += GetDanhSachKhachHangPDF2(itemhanhkhach.Sex, itemhanhkhach.TypeID, (itemhanhkhach.FirstName + " " + itemhanhkhach.Name).ToUpper() + " (" + hanhlykygui + ")");
                        stt++;
                    }
                    break;
                }
                template = template.Replace("{Danhsachkhachhang}", Thongtinhanhkhach);
                template = template.Replace("{DanhsachkhachhangFULL}", ThongtinhanhkhachFULL);
                template = template.Replace("{DanhSachKhachHangPDF2}", DanhSachKhachHangPDF2);
                template = template.Replace("{AddOnPdf2}", AddOnPdf2);
                string chitietve = "";
                string ChangBayPdf2 = "";
                foreach (var item in listTicket)
                {
                    var FromName = getsanbaycode(item.FromCity);
                    var ToName = getsanbaycode(item.ToCity);
                    chitietve += string.Format(@"    <table style='width:100%;border-bottom:5px solid #8b8b8b;border-spacing: 0px 2px;'>
            <thead>
                <tr>
                    <th style='background:#58585a;color:#ffffff'>Hãng</th>
                    <th style='background:#58585a;color:#ffffff'>Chuyến bay</th>
                    <th style='background:#58585a;color:#ffffff'>Ngày</th>
                    <th style='background:#58585a;color:#ffffff'>Loại vé</th>
                    <th style='background:#58585a;color:#ffffff'>Khởi hành</th>
                    <th style='background:#58585a;color:#ffffff'>Đến</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style='text-align: center;'><img src='http://{0}' style='width:70px;height:40px' /></td>
                    <td>{1}</td>
                    <td><b>{2}</b></td>
                    <td>{3}</td>
                    <td><b>{4}</b> - {5} ({6})</td>
                    <td><b>{7}</b> - {8} ({9})</td>
                </tr>
            </tbody>
        </table>", BookingInfo.SourceSite + UIUty.GetImagebyHang(item.Code), item.FlightNo,
                      item.StartDate.HasValue ? item.StartDate.Value.ToString("dd/MM/yyyy") : "",
                      item.Class,
                      item.StartDate.HasValue ? item.StartDate.Value.ToString("HH:mm") : "",
                     FromName, item.FromCity, item.EndDate.HasValue ? item.EndDate.Value.ToString("HH:mm") : ""
                       , ToName, item.ToCity
                      );
                    ChangBayPdf2 += GetChangBayPdf2(daoticket, BookingInfo, item, FromName, ToName);
                }
                template = template.Replace("{ChiTietVe}", chitietve);
                template = template.Replace("{ChangBayPdf2}", ChangBayPdf2);
                decimal tongtienhanhly = 0;
                var indexve = 0;
                foreach (var item in listTicket)
                {
                    var paramPassenger = new BK_PassengerParam { BookingID = BookingInfo.ID, Veid = item.ID };
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
                template = template.Replace("{TienHanhLy}", tongtienhanhly.ToString("n0") + " VND");
                var bkdetail = new BK_BookingDetailDao();
                var listdetailbk = bkdetail.GetbylistbybookingjoinPassengers(BookingInfo.ID);
                var objPricenguoilon = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var objPricetreem = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var objPriceembe = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);
                var objPricenguoilonve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotVe);
                var objPricetreemve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotVe);
                var objPriceembeve = listdetailbk.FirstOrDefault(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotVe);
                var checkquoc = Station.CheckQuocTe(BookingInfo.FromCity, BookingInfo.ToCity);
                var Pricenguoilon = objPricenguoilon != null ? objPricenguoilon.Price : 0;
                var Pricetreem = objPricetreem != null ? objPricetreem.Price : 0;
                var Priceembe = objPriceembe != null ? objPriceembe.Price : 0;
                var Pricenguoilonve = objPricenguoilonve != null ? objPricenguoilonve.Price : 0;
                var Pricetreemve = objPricetreemve != null ? objPricetreemve.Price : 0;
                var Priceembeve = objPriceembeve != null ? objPriceembeve.Price : 0;
                if (checkquoc)
                {
                    Pricenguoilonve = 0;
                    Pricetreemve = 0;
                    Priceembeve = 0;
                }
                template = template.Replace("{Gianguoilon}", ((Pricenguoilon ?? 0) + (Pricenguoilonve ?? 0)).ToString("n0"));
                template = template.Replace("{Giatreem}", ((Pricetreem ?? 0) + (Pricetreemve ?? 0)).ToString("n0"));
                template = template.Replace("{Giaembe}", ((Priceembe ?? 0) + (Priceembeve ?? 0)).ToString("n0"));
                var coutnguoilon = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi);
                var counttreem = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi);
                var countembe = listdetailbk.Count(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi);
                template = template.Replace("{Soluongnguoilon}", coutnguoilon.ToString("n0"));
                template = template.Replace("{Soluongtreem}", counttreem.ToString("n0"));
                template = template.Replace("{Soluongembe}", countembe.ToString("n0"));
                var tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon).Sum(z => z.Price);
                var tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm).Sum(z => z.Price);
                var tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe).Sum(z => z.Price);
                if (checkquoc)
                {
                    tongtiennguoilon = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.NguoiLon && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientreem = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.TreEm && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                    tongtientembe = listdetailbk.Where(z => z.typepg == Constant.TypePassenger.EmBe && z.typeve == Constant.Typeve.LuotDi).Sum(z => z.Price);
                }
                template = template.Replace("{TongTienNguoiLon}", (tongtiennguoilon ?? 0).ToString("n0"));
                template = template.Replace("{TongTientreem}", (tongtientreem ?? 0).ToString("n0"));
                template = template.Replace("{TongTienembe}", (tongtientembe ?? 0).ToString("n0"));
                var tongtien = BookingInfo.TotalPrice.HasValue ? BookingInfo.TotalPrice.Value : 0;
                template = template.Replace("{TongCongTien}", tongtien.ToString("n0") + " VND");
                template = template.Replace("{TongTienVe}", (tongtien - (double)tongtienhanhly).ToString("n0") + " VND");
                template = template.Replace("{PRT_COMPANY_NAME_PDF2}", UIUty.GetsettingByKey(SettingValue, "PRT_COMPANY_NAME"));
                template = template.Replace("{PhoneNumberPdf2}", PhoneNumber);
                template = template.Replace("{PRT_Mail}", UIUty.GetsettingByKey(SettingValue, "PRT_EMAIL"));
                template = template.Replace("{Email_NhanMailHang}", UIUty.GetsettingByKey(SettingValue, "Email_NhanMailHang"));
                var CreateDate = "";
                if (BookingInfo.CreatedDate.HasValue)
                {
                    CreateDate = (BookingInfo.CreatedDate.Value.Day + Utility.MonthName(BookingInfo.CreatedDate.Value.Month) + BookingInfo.CreatedDate.Value.Year);
                }
                template = template.Replace("{CreateDatePDF2}", CreateDate);
            }
        }

    }
}