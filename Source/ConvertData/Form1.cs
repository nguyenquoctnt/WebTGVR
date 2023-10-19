using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace ConvertData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["Vdm_MutilineConnectionString"].ConnectionString ?? "";
       
        public List<SettingUserInfo> GetAllSettingUser(int? userID)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingKeys
                            join groupSetting in dbContext.SettingUsers.Where(z => z.UserID == userID) on n.Id equals groupSetting.IDSetting into p
                            from groupSetting in p.DefaultIfEmpty()

                            select new SettingUserInfo()
                            {
                                IdKey = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                UserID = groupSetting.UserID,
                                IDSetting = groupSetting.IDSetting,
                                Value = groupSetting.Value,
                                UpdateDate = groupSetting.UpdateDate,
                                GroupID = n.GroupID
                            };
                return query.OrderBy(z => z.IdKey).ToList();
            }
        }
        public string GetsettingByKey(List<SettingUserInfo> ListSetting, string key)
        {
            var obj = ListSetting.FirstOrDefault(z => z.Name == key);
            if (obj != null)
                return obj.Value;
            return "";
        }
        public string CatChuoi(string choi, int lenth)
        {
            string value = "";
            if (!string.IsNullOrEmpty(choi))
            {
                var lenthchuoi = choi.Length;
                if (lenthchuoi > lenth)
                {
                    value = choi.Substring(0, lenth - 3);
                    value = value.Substring(0, value.LastIndexOf(" ")) + "...";
                }
                else
                {
                    value = choi;
                }
            }
            return value;
        }
        public string UrlHeper(string Name)
        {
            var strValue = string.Empty;
            if (!string.IsNullOrEmpty(Name))
            {
                Name = CatChuoi(Name, 75);
                var str = HttpUtility.HtmlDecode(Name);
                strValue = str.ToLower().Trim();
                strValue = strValue.Replace("à", "a").Replace("á", "a").Replace("ạ", "a").Replace("ã", "a").Replace("ả", "a");
                strValue = strValue.Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẵ", "a").Replace("ẳ", "a").Replace("ă", "a");
                strValue = strValue.Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẫ", "a").Replace("ẩ", "a").Replace("â", "a");
                strValue = strValue.Replace("đ", "d");
                strValue = strValue.Replace("è", "e").Replace("é", "e").Replace("ẹ", "e").Replace("ẽ", "e").Replace("ẻ", "e");
                strValue = strValue.Replace("ề", "e").Replace("ế", "e").Replace("ệ", "e").Replace("ễ", "e").Replace("ể", "e").Replace("ê", "e");
                strValue = strValue.Replace("ò", "o").Replace("ó", "o").Replace("ọ", "o").Replace("õ", "o").Replace("ỏ", "o");
                strValue = strValue.Replace("ồ", "o").Replace("ố", "o").Replace("ộ", "o").Replace("ỗ", "o").Replace("ổ", "o").Replace("ô", "o");
                strValue = strValue.Replace("ờ", "o").Replace("ớ", "o").Replace("ợ", "o").Replace("ỡ", "o").Replace("ở", "o").Replace("ơ", "o");
                strValue = strValue.Replace("ù", "u").Replace("ú", "u").Replace("ụ", "u").Replace("ũ", "u").Replace("ủ", "u");
                strValue = strValue.Replace("ừ", "u").Replace("ứ", "u").Replace("ự", "u").Replace("ữ", "u").Replace("ử", "u").Replace("ư", "u");
                strValue = strValue.Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ĩ", "i").Replace("ỉ", "i");
                strValue = strValue.Replace("ỳ", "y").Replace("ý", "y").Replace("ỵ", "y").Replace("ỹ", "y").Replace("ỷ", "i");
                strValue = System.Text.RegularExpressions.Regex.Replace(strValue, "[^0-9a-zA-Z]+?", "-").Replace("--", "-");
            }
            return strValue.Replace(" ", "-");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var queryAspNetUser = from n in dbContext.AspNetUsers
                                      where n.Isdelete == Constant.IsNotDeleted && n.ParentId == null
                                      && n.Id != 1
                                      select n;
                foreach (var item in queryAspNetUser)
                {
                    Cate1(item.UserName, item.UrlDomain1, dbContext);
                    Cate2(item.UserName, item.UrlDomain1, dbContext);
                    Cate3(item.UserName, item.UrlDomain1, dbContext);
                    Cate4(item.UserName, item.UrlDomain1, dbContext);
                    Cate5(item.Id,item.PhoneNumber,item.PhoneNumber2, item.UserName, item.UrlDomain1, dbContext);
                    Cate6(item.UserName, item.UrlDomain1, dbContext);
                    Cate7(item.UserName, item.UrlDomain1, dbContext);
                    Cate8(item.UserName, item.UrlDomain1, dbContext);
                    getinfooter(item.Id, item.PhoneNumber, item.PhoneNumber2, item.UserName, dbContext);
                }

            }
        }
        private void Cate1(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "Tin tức",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 1,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "/Content/FontEnd/VDM/images/iconmenu03.png",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = true,
                IsContend = false,
                Isfooter = false
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }
            var listtintuc = dbContext.TblArticles.Where(z => z.LoaiBaiviet == Constant.CateType.TinTuc && z.Nguoitao == user);
            foreach (var item in listtintuc)
            {
                item.LoaiBaiviet = Constant.CateType.NoiDung;
                item.ChuyenMuc = objcate.Id;
                dbContext.SubmitChanges();
            }

        }
        private void Cate2(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "Thanh toán",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 2,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "/Content/payment.png",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = true,
                IsContend = true,
                Isfooter = false
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }

            string conten = "";
            foreach (var itemnganhang in dbContext.DmTaikhoanNganHangs.Where(z => z.Nguoitao == user))
            {
                string templeate = string.Format(@"<article class='box' style='margin-bottom: 1px;border-bottom: 1px dotted #867b7b;padding-bottom: 7px;padding-top: 10px;'>
                    <figure>
                        <a title='{0}'>
                            <img data-src='{1}' src='{2}' alt='{3}' class='famous-place lazy' width='270' height='192'>
                        </a>
                    </figure>
                    <div class='details ' style='    padding: 0px 0 0 145px;'>
                        <h4>
                            TK:{4}
                        </h4>
                        <h4>
                            STK:{5}
                        </h4>
                        <h4>
                            CN: {6}
                        </h4>
                    </div>
                </article>", itemnganhang.Tentaikhoan, itemnganhang.Logo, itemnganhang.Logo, itemnganhang.Tentaikhoan,
                itemnganhang.Tentaikhoan, itemnganhang.Sotaikhoan, itemnganhang.ChiNhanh);
                conten += templeate;
            }
            var objartical = new TblArticle
            {
                Tenbai = "Thông tin thanh toán",
                Mota = "Thông tin tài khoản ngân hàng",
                Phienban = 0,
                Thutu = 1,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                LoaiBaiviet = Constant.CateType.NoiDung,
                ChuyenMuc = objcate.Id,
                Noidung = conten,
            };
            dbContext.TblArticles.InsertOnSubmit(objartical);
            dbContext.SubmitChanges();
            var dbItemartical = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == objartical.Id);
            if (dbItemartical != null)
            {
                dbItemartical.SEOUrl = (UrlHeper(dbItemartical.Tenbai) + "-" + dbItemartical.Id);
                dbContext.SubmitChanges();
            }
            var dbItemCate = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItemCate != null)
            {
                dbItemCate.IdContend = (int?)objartical.Id;
                dbContext.SubmitChanges();
            }
        }
        private void Cate3(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "Khách sạn",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 3,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "/Content/FontEnd/VDM/images/iconmenu04.png",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = true,
                IsContend = false,
                Isfooter = false
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }
            var listtintuc = dbContext.TblArticles.Where(z => z.LoaiBaiviet == Constant.CateType.KhachSan && z.Nguoitao == user);
            foreach (var item in listtintuc)
            {
                item.LoaiBaiviet = Constant.CateType.NoiDung;
                item.ChuyenMuc = objcate.Id;
                dbContext.SubmitChanges();
            }

        }
        private void Cate4(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "VISA DU LỊCH",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 3,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "/Content/FontEnd/VDM/images/iconmenu01.png",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = true,
                IsContend = false,
                Isfooter = false
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }
            var listtintuc = dbContext.TblArticles.Where(z => z.LoaiBaiviet == Constant.CateType.visa && z.Nguoitao == user);
            foreach (var item in listtintuc)
            {
                item.LoaiBaiviet = Constant.CateType.NoiDung;
                item.ChuyenMuc = objcate.Id;
                dbContext.SubmitChanges();
            }

        }
        private void Cate5(int? userid, string phone1, string phone2, string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "Liên hệ",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 5,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "/Content/FontEnd/VDM/images/iconmenu06.png",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = true,
                IsContend = true,
                Isfooter = false
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }

            string conten = "";

            var setting = GetAllSettingUser(userid);
            if (setting.Count > 0)
            {
                conten = string.Format(@" <div class='page-content p-DL'>
        <ul>
            <li class='active'>
                <i class='soap-icon-star yellow-color'></i>{0}
                <ul style='width: 100%;'>
                    <li>Địa chỉ :{1} </li>
                    <li>Hotline :  {2}</li>
                    <li>Email  :{3}</li>
                </ul>
            </li>
        </ul>
    </div>", GetsettingByKey(setting, "PRT_COMPANY_NAME"),
    GetsettingByKey(setting, "PRT_ADDRESS"),
   (phone1 + " - " + phone2), GetsettingByKey(setting, "PRT_EMAIL")
    );
            }
            var objartical = new TblArticle
            {
                Tenbai = "Thông tin liên hệ",
                Mota = "Thông tin liên hệ",
                Phienban = 0,
                Thutu = 1,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                LoaiBaiviet = Constant.CateType.NoiDung,
                ChuyenMuc = objcate.Id,
                Noidung = conten,
            };
            dbContext.TblArticles.InsertOnSubmit(objartical);
            dbContext.SubmitChanges();
            var dbItemartical = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == objartical.Id);
            if (dbItemartical != null)
            {
                dbItemartical.SEOUrl = (UrlHeper(dbItemartical.Tenbai) + "-" + dbItemartical.Id);
                dbContext.SubmitChanges();
            }
            var dbItemCate = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItemCate != null)
            {
                dbItemCate.IdContend = (int?)objartical.Id;
                dbContext.SubmitChanges();
            }
        }
        private void Cate6(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "KHUYẾN MÃI VÉ MÁY BAY",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 6,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "/Content/FontEnd/VDM/images/iconmenu04.png",
                SourceSite = domain1,
                IsHome = true,
                IsMenu = false,
                IsContend = false,
                Isfooter = false
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }
            var listtintuc = dbContext.TblArticles.Where(z => z.LoaiBaiviet == Constant.CateType.KhuyenMai && z.Nguoitao==user);
            foreach (var item in listtintuc)
            {
                item.LoaiBaiviet = Constant.CateType.NoiDung;
                item.ChuyenMuc = objcate.Id;
                dbContext.SubmitChanges();
            }

        }
        private void Cate7(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "CỘNG TÁC VIÊN",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 7,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = false,
                IsContend = false,
                Isfooter = true
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }
            InserCatefooter(dbItem.Id, user, domain1, dbContext, "5 BƯỚC BAN ĐẦU ĐỂ TRỞ THÀNH MỘT CHUYÊN GIA BÁN VÉ", "5 BƯỚC BAN ĐẦU ĐỂ TRỞ THÀNH MỘT CHUYÊN GIA BÁN VÉ");
            InserCatefooter(dbItem.Id, user, domain1, dbContext, "ĐIỀU KHOẢN CTV", "ĐIỀU KHOẢN CTV");
        }
        private void Cate8(string user, string domain1, ContextDataContext dbContext)
        {
            var objcate = new SysCategory()
            {
                TenHienthi = "ĐÀO TẠO NGHIỆP VỤ",
                LoaiCate = Constant.CateType.NoiDung,
                Thutu = 8,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                Phienban = 1,
                Image = "",
                SourceSite = domain1,
                IsHome = false,
                IsMenu = false,
                IsContend = false,
                Isfooter = true
            };
            dbContext.SysCategories.InsertOnSubmit(objcate);
            dbContext.SubmitChanges();
            var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == objcate.Id);
            if (dbItem != null)
            {
                dbItem.SEOUrl = (UrlHeper(dbItem.TenHienthi) + "-" + dbItem.Id);
                dbContext.SubmitChanges();
            }
            InserCatefooter(dbItem.Id, user, domain1, dbContext, "Quy định hành lý các hãng hàng không trong nước", "Quy định hành lý các hãng hàng không trong nước");
            InserCatefooter(dbItem.Id, user, domain1, dbContext, "Mẫu giấy xác nhận nhân thân để đi máy bay khi bị mất giấy tờ tùy thân", "Mẫu giấy xác nhận nhân thân để đi máy bay khi bị mất giấy tờ tùy thân");
            InserCatefooter(dbItem.Id, user, domain1, dbContext, "Giấy tờ tùy thân khi đi máy bay", "Giấy tờ tùy thân khi đi máy bay");
        }
        private void InserCatefooter(int? cateid, string user, string domain1, ContextDataContext dbContext, string tieude, string mota)
        {
            var objartical = new TblArticle
            {
                Tenbai = tieude,
                Mota = mota,
                Phienban = 0,
                Thutu = 1,
                Daxoa = false,
                NgayTao = DateTime.Now,
                NgaySua = DateTime.Now,
                Nguoitao = user,
                NguoiSua = user,
                LoaiBaiviet = Constant.CateType.NoiDung,
                ChuyenMuc = cateid,
                Noidung = "",
            };
            dbContext.TblArticles.InsertOnSubmit(objartical);
            dbContext.SubmitChanges();
            var dbItemartical = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == objartical.Id);
            if (dbItemartical != null)
            {
                dbItemartical.SEOUrl = (UrlHeper(dbItemartical.Tenbai) + "-" + dbItemartical.Id);
                dbContext.SubmitChanges();
            }
        }
        private void getinfooter(int? userid, string phone1, string phone2, string user, ContextDataContext dbContext)
        {

            var setting = GetAllSettingUser(userid);
            if (setting.Count > 0)
            {
                var values = string.Format(@"<div class='bocuc bocuc_193'>
<div class='loprong'>
<div class='padding'><strong>{0}</strong><br />
Địa chỉ: {1}<br />
Phone: {2}<br />
Email: {3}</div>
</div>
</div>

<div class='bocuc bocuc_198'>
<div class='loprong'>
<div class='padding'>
<div class='bocuc bocuc_199'>
<div class='loprong'>
<p>&nbsp;</p>
</div>
</div>
</div>
</div>
</div>
", GetsettingByKey(setting, "PRT_COMPANY_NAME"), GetsettingByKey(setting, "PRT_ADDRESS"), (phone1 + " - " + phone2)
   , GetsettingByKey(setting, "PRT_EMAIL"));

                var obj = new tbl_TempleatePropertyUser
                {
                    IdProperty = 29,
                    IdUser = userid,
                    Valued = values,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreateUserd = user,
                    UpdateUser = user
                };
                dbContext.tbl_TempleatePropertyUsers.InsertOnSubmit(obj);
                dbContext.SubmitChanges();

            }


        }
    }
}
