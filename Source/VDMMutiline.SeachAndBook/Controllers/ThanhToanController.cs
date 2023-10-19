using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.Ultilities;
using VDMMutiline.Core.UI;
using VDMMutiline.Core.Common;
using Common.Logging;

namespace VDMMutiline.SeachAndBook.Controllers
{
    public class ThanhToanController : PublishController
    {
        string ThanhtoanCodeWesite = System.Configuration.ConfigurationSettings.AppSettings.Get("ThanhtoanCodeWesite");
        string DonviThanhToan = System.Configuration.ConfigurationSettings.AppSettings.Get("DonviThanhToan");
        string TaiKhoanNhanTien = System.Configuration.ConfigurationSettings.AppSettings.Get("TaiKhoanNhanTien");
        private static readonly ILog log =
           LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChonHinhThucThanhToan(string madonhang)
        {
            try
            {
                if (string.IsNullOrEmpty(madonhang))
                    return Redirect("/");
                ViewBag.Madonhang = madonhang;
                var bookingdao = new BK_BookingDao();
                var obj = bookingdao.GetInfoALL(madonhang.Trim());
                var param = new SeachParam();
                param.BkBooking = obj;
                if (obj?.Status == Constant.StatusVe.DaXuatVe)
                    return RedirectToAction("Dathanhtoan", "SeachHome");

                return View(param);
            }
            catch
            {
                return Redirect("/");
            }

        }
        [HttpPost]
        public ActionResult ChonHinhThucThanhToan(FormCollection forminput)
        {
            try
            {
                var hinhthucthanhtoan = forminput.GetValue("IdHinhthucthanhtoan")?.AttemptedValue;
                var MaDonHang = forminput.GetValue("MaDonHang")?.AttemptedValue;
                var bookingdao = new BK_BookingDao();
                var obj = bookingdao.GetInfoALL(MaDonHang);
                var param = new SeachParam();
                param.BkBooking = obj;
                var paymentinfo = new tbl_PaymentInfo();
                if (obj != null)
                {
                    var hinhthucthanhtoanvalue = hinhthucthanhtoan != null ? Utility.ConvertToInt(hinhthucthanhtoan) : 0;
                    paymentinfo.IdHinhThuc = hinhthucthanhtoanvalue;
                    paymentinfo.IdBooking = obj.ID;
                    switch (hinhthucthanhtoanvalue)
                    {
                        case Constant.Hinhthucthanhtoav2.chuyenkhoan:
                            {
                                var SoTaiKhoanChon = forminput.GetValue("SoTaiKhoanChon")?.AttemptedValue;
                                var NganHangChon = forminput.GetValue("NganHangChon")?.AttemptedValue;
                                var ChuTheChon = forminput.GetValue("ChuTheChon")?.AttemptedValue;
                                var ChiNhanhChon = forminput.GetValue("ChiNhanhChon")?.AttemptedValue;
                                string note = "";
                                note += "STK: " + SoTaiKhoanChon;
                                note += "<br>Ngân hàng: " + NganHangChon;
                                note += "<br>Chủ thẻ: " + ChuTheChon;
                                note += "<br>Chi nhánh: " + ChiNhanhChon;
                                paymentinfo.Note = paymentinfo.Note;
                                bookingdao.UpdatePayment(paymentinfo);
                                return RedirectToAction("KetQua", "ThanhToan", new { madonhang = MaDonHang });
                                break;
                            }
                        case Constant.Hinhthucthanhtoav2.Taivanphong:
                            bookingdao.UpdatePayment(paymentinfo);
                            return RedirectToAction("KetQua", "ThanhToan", new { madonhang = MaDonHang });
                            break;
                        case Constant.Hinhthucthanhtoav2.Giaovetannha:
                            paymentinfo.Note = "Địa chỉ: " + obj.Address;
                            bookingdao.UpdatePayment(paymentinfo);
                            return RedirectToAction("KetQua", "ThanhToan", new { madonhang = MaDonHang });
                            break;
                        case Constant.Hinhthucthanhtoav2.VtcPay:
                            {
                                var NganHangChon = forminput.GetValue("NganHangChon") == null ? "" : forminput.GetValue("NganHangChon").AttemptedValue;
                                bookingdao.UpdatePayment(paymentinfo);
                                return Redirect(ThanhtoanVTC(obj, NganHangChon));
                                break;

                            }
                        case Constant.Hinhthucthanhtoav2.VnPay:
                            {
                                var NganHangChon = forminput.GetValue("NganHangChon") == null ? "" : forminput.GetValue("NganHangChon").AttemptedValue;
                                bookingdao.UpdatePayment(paymentinfo);
                                return Redirect(ThanhToanVNPay(obj, NganHangChon));
                                break;
                            }
                    }

                }
                return Redirect("/");
            }
            catch
            {
                return Redirect("/");
            }
        }
        public ActionResult KetQua(string madonhang)
        {
            try
            {
                var bookingdao = new BK_BookingDao();
                var obj = bookingdao.GetInfoALL(madonhang);
                if (obj != null)
                {
                    ViewBag.Fail = string.IsNullOrEmpty(obj.BookCode);
                    ViewBag.Quocte = Station.CheckQuocTe(obj.FromCity, obj.ToCity);
                    var payment = bookingdao.GetInfoThanhToan(obj.ID);
                    if (payment != null)
                    {
                        string Note = "";
                        switch (payment.IdHinhThuc)
                        {
                            case Constant.Hinhthucthanhtoav2.VtcPay:
                                {
                                    Note = ThanhToanThanhCongVTC(obj.BookCode);
                                    break;
                                }
                            case Constant.Hinhthucthanhtoav2.VnPay:
                                {
                                    Note = ThanhToanThanhCongVNPay(obj.BookCode);
                                    break;
                                }
                        }
                        if (!string.IsNullOrEmpty(Note))
                        {
                            //if (obj.Status == Constant.StatusVe.DangGiuCho)
                            //{
                            bookingdao.UpdatePayment(obj.ID, Note);
                            // }

                        }
                    }
                }
                else
                {
                    return Redirect("/");
                }
                return View(obj);
            }
            catch
            {
                return Redirect("/");
            }
        }
        private string ThanhtoanVTC(BK_Booking bk, string nganhang)
        {
            var hinhthucthanhtoan = nganhang;
            var txtsodienthoai = bk.Phone == null ? "" : bk.Phone;
            var txttotalprice = bk.TotalPrice == null ? 0 : bk.TotalPrice;
            var BookCode = bk.BookCode == null ? "" : bk.BookCode;
            string Security_Key = ConfigurationManager.AppSettings["SecretKey"];
            string urlfull = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string path = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            string urlReturn = string.Format("{0}{1}{2}", urlfull.Replace(path, ""),
                    Url.Action("KetQua", "ThanhToan") + "?madonhang=", bk.rCode);


            string website_id = ThanhtoanCodeWesite;
            string amount = txttotalprice?.ToString();
            string currency = "VND";
            string receiver_account = TaiKhoanNhanTien;
            string reference_number = BookCode;
            string transaction_type = "sale";
            string language = "vi";
            string url_return = urlReturn;
            string payment_type = hinhthucthanhtoan;
            string bill_to_email = bk.Email == null ? "" : bk.Email;
            string bill_to_phone = txtsodienthoai;
            string bill_to_address = bk.Address == null ? "" : bk.Address;
            string bill_to_address_city = "";
            string bill_to_surname = bk.Name == null ? "" : bk.Name;
            string bill_to_forename = bk.Name == null ? "" : bk.Name;
            Dictionary<string, object> paramQueryList = new Dictionary<string, object>()
                {
                    {"website_id", website_id},
                    {"amount",amount},
                    {"currency",currency},
                    {"receiver_account",receiver_account},
                    {"reference_number",reference_number},
                    {"transaction_type",transaction_type},
                    {"language",language},
                    {"url_return",url_return},
                    {"payment_type",payment_type},
                    {"bill_to_email",bill_to_email},
                    {"bill_to_phone",bill_to_phone},
                    {"bill_to_address",bill_to_address},
                    {"bill_to_address_city",bill_to_address_city},
                    {"bill_to_surname",bill_to_surname},
                    {"bill_to_forename",bill_to_forename}
                };

            string plaintext = string.Empty;
            string listparam = string.Empty;

            String[] sortedKeys = paramQueryList.Keys.ToArray();
            Array.Sort(sortedKeys);

            foreach (String key in sortedKeys)
            {
                plaintext += string.Format("{0}{1}", plaintext.Length > 0 ? "|" : string.Empty, paramQueryList[key]);
                if (new string[] { "url_return", "bill_to_surname", "bill_to_forename", "bill_to_address", "bill_to_address_city" }.Contains(key))
                    listparam += string.Format("{0}={1}&", key, Server.UrlEncode(paramQueryList[key].ToString()));
                else
                    listparam += string.Format("{0}={1}&", key, paramQueryList[key].ToString());
            }
            string textSign = string.Format("{0}|{1}", plaintext, Security_Key);
            string signature = Security.SHA256encrypt(textSign);
            NLogLogger.LogInfo("Textsign:" + textSign
                + Environment.NewLine + "signature:" + signature);
            listparam = string.Format("{0}signature={1}", listparam, signature);
            string urlRedirect = string.Format("{0}?{1}", ConfigurationManager.AppSettings["VTCPay_Url"], listparam);
            NLogLogger.LogInfo("urlFull: " + urlRedirect);
            return urlRedirect;
        }
        private string ThanhToanVNPay(BK_Booking bk, string nganhang)
        {
            double? phicongthem = 0;
            if (nganhang == "VISA")
            {
                phicongthem = ((bk.TotalPrice * 2.75) / 100) + 2500;
            }
            else
            {
                phicongthem = ((bk.TotalPrice * 1.1) / 100) + 1650;
            }
            //Get Config Info
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            // var vnp_Params = new OrderedDictionary();
            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_CurrCode", "VND");

            vnpay.AddRequestData("vnp_TxnRef", bk.BookCode);// Boooking code
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan TGVR, dien thoai khach hang" + bk.Phone);
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_Amount", ((bk.TotalPrice + phicongthem) * 100)?.ToString());
            String strPathAndQuery = Request.Url.PathAndQuery;
            String strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "");
            vnpay.AddRequestData("vnp_ReturnUrl", strUrl + string.Format("{0}{1}", Url.Action("KetQua", "ThanhToan") + "?madonhang=", bk.rCode));
            vnpay.AddRequestData("vnp_IpAddr", VnPayLibrary.Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            vnpay.AddRequestData("vnp_BankCode", nganhang);
            //Calculate data for sign
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("SecureHash={0}", paymentUrl);
                log.DebugFormat("vnp_url={0}", vnp_Url);
            }
            log.InfoFormat("Init Success, Redirect to: {0}", paymentUrl);
            return paymentUrl;
        }
        private string ThanhToanThanhCongVTC(string BookingCode)
        {
            string KetQua = "";
            if (Request.QueryString["reference_number"] == null || Request.QueryString["amount"] == null ||
               Request.QueryString["website_id"] == null)
            {
                KetQua = "Thanh toán thất bại, TGVR sẽ liên hệ lại với bạn.";
                NLogLogger.LogInfo(BookingCode + "Thanh toán thất bại : reference_number=null;amount=null;website_id=null;");
                return KetQua;
            }
            // Lay cac tham so tra ve tren url
            double amount = Convert.ToDouble(Request.QueryString["amount"]);
            string message = Request.QueryString["message"];
            string payment_type = Request.QueryString["payment_type"];
            string reference_number = Request.QueryString["reference_number"];
            int status = Convert.ToInt32(Request.QueryString["status"]);
            string trans_ref_no = Request.QueryString["trans_ref_no"];
            int website_id = Convert.ToInt32(Request.QueryString["website_id"]);
            string signature = Server.HtmlDecode(Request.QueryString["signature"].ToString().Replace(" ", "+"));
            bool isVerify = false;
            string merchantSign = string.Empty;
            object[] arrParamReturn = new object[] { amount, message, payment_type, reference_number, status, trans_ref_no, website_id };
            string textSign = string.Join("|", arrParamReturn) + "|" + ConfigurationManager.AppSettings["SecretKey"];
            merchantSign = Security.SHA256encrypt(textSign);
            isVerify = (merchantSign == signature);
            if (isVerify)
            {
                if (status == 1)
                {
                    KetQua = "Thanh toán thành công.";
                    var dao = new BK_BookingDao();
                    dao.Updatestatusbycode(BookingCode, Constant.StatusVe.YeuCauXuat);
                }

            }
            KetQua = "";
            NLogLogger.LogInfo("HTTP GET.Sai chu ky. Text:" + textSign
                               + Environment.NewLine + "Sign:" + merchantSign);
            return KetQua;
        }
        private string ThanhToanThanhCongVNPay(string BookingCode)
        {
            string ketqua = "";
            var vnpayData = Request.QueryString;
            var vnp_Params = new Dictionary<string, string>();
            if (vnpayData.Count > 0)
            {
                foreach (string s in vnpayData)
                {
                    vnp_Params.Add(s, vnpayData[s]);
                }
            }
            if (vnp_Params.ContainsKey("vnp_SecureHashType"))
            {
                vnp_Params.Remove("vnp_SecureHashType");
            }
            string vnp_SecureHash = "";
            if (vnp_Params.ContainsKey("vnp_SecureHash"))
            {
                vnp_SecureHash = vnp_Params["vnp_SecureHash"];
                vnp_Params.Remove("vnp_SecureHash");
            }
            if (vnp_Params.ContainsKey("madonhang"))
            {
                vnp_Params.Remove("madonhang");
            }
            vnp_Params = vnp_Params.OrderBy(o => o.Key, new VnPayLibrary.VnPayCompare()).ToDictionary(k => k.Key, v => v.Value);
            string singData = string.Join("&",
                vnp_Params.Where(x => !string.IsNullOrEmpty(x.Value))
                    .Select(k => (HttpUtility.UrlDecode(k.Key) + "=" + HttpUtility.UrlDecode(k.Value))));
            //   log.DebugFormat("{0}", singData);
            var orderId = vnp_Params["vnp_TxnRef"];
            var vnpTranId = vnp_Params["vnp_TransactionNo"];
            string rspCode = vnp_Params["vnp_ResponseCode"];
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            string secureHash = VnPayLibrary.Utils.Md5(vnp_HashSecret + singData);
            if (secureHash.Equals(vnp_SecureHash))
            {
                if (rspCode == "00")
                {
                    ketqua = "Thanh toán thành công";
                    //var dao = new BK_BookingDao();
                    //dao.Updatestatusbycode(BookingCode, Constant.StatusVe.YeuCauXuat);
                }
                else
                {
                    ketqua = "Giao Dịch Thất Bại, TGVR sẽ liên hệ lại với bạn.";
                }
            }
            else
            {
                ketqua = "";
            }
            return ketqua;
        }

    }
}