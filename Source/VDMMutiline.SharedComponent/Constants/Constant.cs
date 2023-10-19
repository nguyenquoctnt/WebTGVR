using System;
using System.Collections.Generic;

namespace VDMMutiline.SharedComponent.Constants
{
    public class Constant
    {

        public const string DeleteID = "DeleteID";
        public const string ParamID = "id";
        public const string ParamType = "type";
        public const string KL = "KL";
        public const string ParamUrl = "url";
        public const bool IsNotDeleted = false;
        public const bool IsDeleted = true;
        public const int FirstVersion = 1;
        public const int AdminPageSize = 20;
        public const string keyEncrypt = "DUONG@!@#$%^&*";
        public const string NotItem = "Bản ghi không tồn tại, hoặc đã bị xóa trước đó.";
        public const string Loaddingdata = "Đang xử lý.";
        public const string zeroRecords = "Không có dữ liệu.";
        public const string Addnewsuccess = "Thêm mới thành công.";
        public const string Updatesuccess = "Sửa thành công.";
        public const string Deletesuccess = "Xóa thành công.";
        public const string Completesuccess = "Kết thúc thành công.";
        public const string Aprovesuccess = "Duyệt thành công.";
        public const string Locksuccess = "Khóa thành công.";
        public const string Unlocksuccess = "Kích hoạt thành công.";
        public const string AcactFail = "Thất bại.";
        public const string SessionSeach = "SessionSeachMuti";
        public const string SessionSeachBackup = "SessionSeachMutiBackup";
        public class Gender
        {
            public const string Male = "Nam";
            public const string Female = "Nữ";

            public static readonly Dictionary<string, string> dctName = new Dictionary<string, string>()
            {
                {Male, "Nam"},
                {Female, "Nữ"}
            };
        }
        public class IssueHistoryStatus
        {
            public const int Success = 1;
            public const int Fail = 2;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Success, "Thành công"},
                {Fail, "Thất bại"}
            };
        }
        public class VoidHistoryStatus
        {
            public const int Success = 1;
            public const int Fail = 2;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Success, "Thành công"},
                {Fail, "Thất bại"}
            };
        }
        public class CancelHistoryStatus
        {
            public const int Success = 1;
            public const int Fail = 2;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Success, "Thành công"},
                {Fail, "Thất bại"}
            };
        }
        public class TemPleateHTMLID
        {
            public const int VeXacNhanHanhTrinh = 1;
            public const int KichHoatVe = 2;
            public const int VeSapBay = 3;
            public const int HoanTatHanhTrinh = 4;
            public const int HuyVe = 7;
            public const int PDFVJ = 6;
            public const int PDFALL = 8;
        }
        public class LogBookingType
        {
            public const int Bookve = 0;
            public const int sendsms = 1;
            public const int sendmail = 2;
            public const int capnhaptrangthai = 3;
            public const int capnhapghichu = 4;
            public const int capnhapgia = 5;
            public const int thaydoingaygiobay = 6;
            public const int doitenhanhkhach = 7;
            public const int Hoanve = 8;
            public const int Xoahanhkhach = 9;
            public const int Xoabooking = 10;
            public const int xoave = 11;
            public const int DatVeTrenHeThong = 12;
            public const int ThemHanhKhach = 13;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Bookve, "Đặt vé"},
                {sendsms, "Gửi sms, thay đổi số điện thoại"},
                {sendmail, "Gửi mail"},
                {capnhaptrangthai, "Thay đổi Code, trạng thái, Booker, thời gian giữ chỗ"},
                 {capnhapghichu, "Thay đổi ghi chú"},
                   {capnhapgia, "Thay đổi giá"},
                      {doitenhanhkhach, "Đổi tên hành khách"},
                       {Hoanve, "Hoàn vé"},
                        {Xoahanhkhach, "Xóa hành khách"},
                        {Xoabooking, "Xóa booking"},
                         {xoave, "Xóa vé"},
                           {DatVeTrenHeThong, "Đặt vé trên hệ thống"},
                            {ThemHanhKhach, "Thêm hành khách"},

            };
        }
        #region All Type

        public static class LoaiQuangcao
        {
            public const int Anh = 1;
            public const int Link = 2;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
           {
               {Anh, "Hình ảnh"},
               {Link, "Liên kết"},

           };
        }
        public static class LoaiThuocTinh
        {
            public const int Number = 1;
            public const int String = 2;
            public const int ChuoiNhieuDong = 3;
            public const int Bit = 4;
            public const int BangMau = 5;
            public const int File = 6;
            public const int Editor = 7;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
           {
               {Number, "Số"},
               {String, "Chuỗi"},
               {ChuoiNhieuDong, "Chuỗi nhiều dòng"},
               {Bit, "Có/Không"},
                 {BangMau, "Bảng màu"},
                   {File, "File"},
                   {Editor, "Editor"},

           };
        }
        public static class CateType
        {
            public const int KhachSan = 1;
            public const int DuLich = 2;
            public const int KhuyenMai = 3;
            public const int DoiTac = 5;
            public const int TinTuc = 6;
            public const int NoiDung = 7;
            public const int NhanVienHoTro = 8;
            public const int Vinpearl = 9;
            public const int visa = 10;
            public const int Tour = 11;
            public const int ThueXe = 12;
            public const int Thongtinkhac = 13;
            public const int BanCoThacMac = 14;
            public const int MuaHangVaThanhToan = 15;
            public const int LienKet = 16;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {KhachSan, "Khách sạn"},
                {DuLich, "Du lịch"},
                {KhuyenMai, "Khuyến mãi"},
                 {DoiTac, "Đối tác"},
                {TinTuc, "Tin tức"},
                 {NoiDung, "Nội dung"},
                   {NhanVienHoTro, "Nhân viên hỗ trợ"},
                    {Vinpearl, "Vinpearl"},
                     {visa, "Visa"},
                      {Tour, "Tour"},
                       {ThueXe, "Thuê xe"},
                       {Thongtinkhac, "Thông tin khác"},
                        {BanCoThacMac, "Bạn có thắc mắc"},
                         {MuaHangVaThanhToan, "Mua hàng và thanh toán"},
                         {LienKet, "Liên kết"},
            };
        }
        public static class DocumentType
        {
            public const int ImageNews = 1;
            public const int ImageProduct = 2;
            public const int HosoDauthau = 3;
            public const int AdImage = 4;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {ImageNews, "Ảnh tin"},
                {ImageProduct, "Ảnh sản phẩm"},
                {HosoDauthau, "Hồ sơ thầu"},
                {AdImage, "Ảnh quảng cáo"},
            };
        }
        public static class FeatureType
        {
            public const int Membertype = 1;
            public const int Storetype = 2;
            public const int CategryGrouptype = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Membertype, "Loại thành viên"},
                {Storetype, "Loại gian hàng"},
                {CategryGrouptype, "Nhóm ngành hàng"},
            };
        }
        public static class TindangType
        {
            public const int Tinmua = 1;
            public const int Tinban = 2;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Tinmua, "Tin mua"},
                {Tinban, "Tin bán"},
            };
        }
        public static class TindangCate
        {
            public const int Tinthuong = 1;
            public const int TinVip = 2;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Tinthuong, "Tin thường"},
                {TinVip, "Tin VIP"},
            };
        }
        public static class Trangthaive
        {
            public const int Choban = 1;
            public const int Dangiaodich = 2;
            public const int Dahoanthanh = 3;
            public const int Yeucauhuy = 4;
            public const int Huygiaodich = 5;
            public const int Hoantien = 6;
            public const int Daban = 7;
            public const int Choduyet = 8;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Choban, "Chờ bán"},
                {Dangiaodich, "Đang giao dịch"},
                {Dahoanthanh, "Đã hoàn thành"},
                {Yeucauhuy, "Yêu cầu hủy"},
                {Huygiaodich, "Hủy giao dịch"},
                {Hoantien, "Hoàn tiền"},
                {Daban, "Đã bán"},
                {Choduyet, "Chờ duyệt"},
            };
        }
        public static class Trangthaimuave
        {
            public const int choduyet = 1;
            public const int Dangiaodich = 2;
            public const int Dahoanthanh = 3;
            public const int Yeucauhuy = 4;
            public const int Huygiaodich = 5;
            public const int Hoantien = 6;
            public const int Damua = 7;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {choduyet, "Chờ duyệt"},
                {Dangiaodich, "Đang giao dịch"},
                {Dahoanthanh, "Đã hoàn thành"},
                {Yeucauhuy, "Yêu cầu hủy"},
                {Huygiaodich, "Hủy giao dịch"},
                {Hoantien, "Hoàn tiền"},
                {Damua, "Đã mua"},
            };
        }
        public static class Hinhthucthahtoan
        {
            public const int ChuyenKhoan = 1;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {ChuyenKhoan, "Chuyển khoản"},
            };
        }
        public enum MessageState
        {
            Warning = 0,
            Success = 1,
            Error = 2,
            Info = 3
        }
        public enum LogType
        {
            Warning = 0,
            Success = 1,
            Error = 2,
            Info = 3
        }
        public enum LogLevel
        {
            Login = 1,
            Action = 2,
            System = 3
        }
        public enum MessageType
        {
            Alert = 0,
            Confirm = 1,
            Notification = 2
        }
        public enum RoomType
        {
            Publish = 0,
            Private = 1
        }
        public static class LoaiThanhvien
        {
            public const int Vanglai = 0;
            public const int Thuong = 1;
            public const int Vip = 2;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
           {
                {Vanglai, "Thành viên vãng lai"},
               {Thuong, "Thường"},
               {Vip, "Đặc biệt"}
           };
        }
        public static class LoaiGoiThau
        {
            public const int Thongthuong = 1;
            public const int Baodam = 2;
            public const int Baolanh = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Thongthuong, "Gói Thông thường"},
                {Baodam, "Gói Bảo đảm"},
                {Baolanh, "Gói Bảo lãnh"},
            };
        }

        #endregion
        #region All Status
        public static class TrangthaiKichhoat
        {
            public const int Actived = 1;
            public const int Pending = 2;
            public const int NotActived = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
           {
               {Actived, "Đã kích hoạt"},
               {Pending, "Chờ duyệt"},
               {NotActived, "Chưa kích hoạt"}
           };
        }
        public static class TrangthaiBanghi
        {
            public const int Draft = 1;
            public const int Pending = 2;
            public const int Rejected = 3;
            public const int Approved = 4;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
           {
                {Draft, "Nháp"},
                {Pending, "Chờ duyệt"},
                {Rejected, "Hủy duyệt"},
                {Approved, "Đã duyệt"}
           };
        }
        public static class TrangthaiXuatban
        {
            public const int Draft = 1;
            public const int Pending = 2;
            public const int Rejected = 3;
            public const int Published = 4;
            public const int UnPublish = 5;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
           {
                {Draft, "Nháp"},
                {Pending, "Chờ xuất bản"},
                {Rejected, "Không xuất bản"},
                {Published, "Đã xuất bản"},
                {UnPublish, "Hạ xuất bản"}
           };
        }
        public static class TrangthaiGoiThau
        {
            public const int Moitao = 1;
            public const int Pending = 2;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Moitao, "Mới tạo"},
                {Pending, "Đã phê duyệt"},

            };
        }
        public static class TrangthaiGianHangDangKyDuThau
        {
            public const int Moitao = 1;
            public const int Dapheduyet = 2;
            public const int Dahuyduyet = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Moitao, "Mới tạo"},
                {Dapheduyet, "Đã phê duyệt"},
                {Dahuyduyet, "Đã hủy duyệt"},

            };
        }
        public static class TinhtrangThanhtoan
        {
            public const int Moitao = 1;
            public const int Dangthanhtoan = 2;
            public const int Dathanhtoan = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Moitao, "Chưa thanh toán"},
                {Dangthanhtoan, "Đã thanh toán một phần"},
                {Dathanhtoan, "Đã thanh toán toàn bộ"},
            };
        }
        public static class PhuongThucThanhtoan
        {
            public const int Cash = 1;
            public const int Tranfer = 2;
            public const int PayWithSystem = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Cash, "Tiền mặt"},
                {Tranfer, "Chuyển khoản ngân hàng"},
                {PayWithSystem, "Thanh toán qua hệ thống"},
            };
        }
        public static class TinhtrangGiaohang
        {
            public const int Moitao = 1;
            public const int Danggiao = 2;
            public const int Dagiao = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Moitao, "Chưa giao hàng"},
                {Danggiao, "Đã giao một phần"},
                {Dagiao, "Đã giao toàn bộ"},
            };
        }

        public static class TrangthaiDonhang
        {
            public const int Pending = 1;
            public const int Processing = 2;
            public const int Completed = 3;
            public const int Cancaled = 4;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Pending, "Chờ duyệt"},
                {Processing, "Đang xử lý"},
                {Completed, "Đã hoàn thành"},
                {Cancaled, "Đã hủy"},

            };
        }
        public static class Trangthainaprut
        {
            public const int Pending = 1;
            public const int Completed = 2;
            public const int Cancaled = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Pending, "Chờ duyệt"},
                {Completed, "Đã duyệt"},
                {Cancaled, "Đã hủy"},

            };
        }
        #endregion

        #region Ticket
        public class ThangNam
        {
            public const int thg1 = 1;
            public const int thg2 = 2;
            public const int thg3 = 3;
            public const int thg4 = 4;
            public const int thg5 = 5;
            public const int thg6 = 6;
            public const int thg7 = 7;
            public const int thg8 = 8;
            public const int thg9 = 9;
            public const int thg10 = 10;
            public const int thg11 = 11;
            public const int thg12 = 12;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {thg1, "Tháng 1/" +DateTime.Now.Year},
                {thg2, "Tháng 2/" +DateTime.Now.Year},
                 {thg3, "Tháng 3/" +DateTime.Now.Year},
                   {thg4, "Tháng 4/" +DateTime.Now.Year},
                     {thg5, "Tháng 5/" +DateTime.Now.Year},
                      {thg6, "Tháng 6/" +DateTime.Now.Year},
                {thg7,"Tháng 7/" +DateTime.Now.Year},
                 {thg8, "Tháng 8/" +DateTime.Now.Year},
                   {thg9,"Tháng 9/" +DateTime.Now.Year},
                     {thg10,"Tháng 10/" +DateTime.Now.Year},
                     {thg11,"Tháng 11/" +DateTime.Now.Year},
                     {thg12,"Tháng 12/" +DateTime.Now.Year}
            };
        }
        public class Olderve
        {
            public const string Gia = "Gia";
            public const string Thoigianbay = "Thoigianbay";
            public const string GioCatCanh = "GioCatCanh";
            public const string GioHaCanh = "GioHaCanh";
            public const string HangHangkhong = "HangHangkhong";

            public static readonly Dictionary<string, string> dctName = new Dictionary<string, string>()
            {
                {Gia, "Gia"},
                {Thoigianbay, "Thoigianbay"},
                 {GioCatCanh, "GioCatCanh"},
                   {GioHaCanh, "GioHaCanh"},
                     {HangHangkhong, "HangHangkhong"}
            };
        }
        public class StatusVoucher
        {
            public const int Chuasudung = 0;
            public const int Dasudung = 1;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Chuasudung, "Chưa sử dụng"},
                {Dasudung, "Đã sử dụng"},
            };
        }
        public class Typeolder
        {
            public const string Descending = "Descending";
            public const string Ascending = "Ascending";

            public static readonly Dictionary<string, string> dctName = new Dictionary<string, string>()
            {
                {Descending, "Descending"},
                {Ascending, "Ascending"},

            };
        }
        public class Typeve
        {
            public const int LuotDi = 0;
            public const int LuotVe = 1;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {LuotDi, "Lượt đi"},
                {LuotVe, "Lượt về"},
            };
        }
        public class TypePassenger
        {
            public const int NguoiLon = 1;
            public const int TreEm = 2;
            public const int EmBe = 3;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {NguoiLon, "Người lơn"},
                {TreEm, "Trẻ em"},
                {EmBe, "Em bé"},
            };
        }

        public class StatusVe
        {

            public const int TatCa = 0;
            public const int DangGiuCho = 1;
            public const int DaXuatVe = 2;
            public const int YeuCauXuat = 3;
            public const int YeuCauHuy = 4;
            public const int ChoHuyVe = 5;
            public const int DaHuyVe = 6;
            public const int DaHoanTien = 7;
            public const int ChuyenBayHoanTat = 8;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {TatCa, "Tất cả"},
                {DangGiuCho, "Đang giữ chỗ"},
                {DaXuatVe, "Đã xuất vé"},
                {YeuCauXuat, "Yêu cầu xuất vé"},
                {YeuCauHuy, "Yêu cầu hủy vé"},
                {ChoHuyVe, "Chờ hủy vé"},
                {DaHuyVe, "Đã hủy vé"},
                {DaHoanTien, "Đã hoàn tiền"},

                {ChuyenBayHoanTat, "Chuyến bay hoàn tất"},
            };
        }
        public class Hinhthucthanhtoav2
        {
            public const int Taivanphong = 0;
            public const int chuyenkhoan = 1;
            public const int VtcPay = 2;
            public const int VnPay = 3;
            public const int Giaovetannha = 4;
            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                {Taivanphong, "TẠI VĂN PHÒNG"},
                {chuyenkhoan, "CHUYỂN KHOẢN"},
                {Giaovetannha, "GIAO VÉ TẬN NHÀ"},
                {VtcPay, "Cổng thanh toán VTCPay"},
                {VnPay, "Cổng thanh toán VNPay"},
            };
        }
        #endregion
        public class AirlineDomestic
        {
            public const string VJ = "VJ";
            public const string JQ = "JQ";
            public const string QH = "QH";
            public const string VN = "VN";

            public static readonly Dictionary<string, string> dctName = new Dictionary<string, string>()
            {
                {VJ, "Vietjet Air"},
                {JQ, "Jetstar"},
                {QH, "Bamboo"},
                {VN, "Vietnam Airlines"}

            };
        }
    }
}