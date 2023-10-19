using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class SysCategoryDao : BaseDao
    {
        #region Action
        public int Insert(SysCategory item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.SysCategories.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.SEOUrl = (StringFormat.UrlHeper(item.TenHienthi) + "-" + dbItem.Id);
                    dbContext.SubmitChanges();
                }
                return item.Id;
            }
        }
        public void Update(SysCategory item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.TenHienthi = item.TenHienthi;
                    dbItem.Ma = item.Ma;
                    dbItem.ParentId = item.ParentId;
                    dbItem.LoaiCate = item.LoaiCate;
                    dbItem.Thutu = item.Thutu;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.LeftCode = item.LeftCode;
                    dbItem.IdDonvi = item.IdDonvi;
                    dbItem.Thumb = item.Thumb;
                    dbItem.Mota = item.Mota;
                    dbItem.SEOUrl = (StringFormat.UrlHeper(item.TenHienthi) + "-" + dbItem.Id);
                    dbItem.SEODescription = item.SEODescription;
                    dbItem.SEOKeyword = item.SEOKeyword;
                    dbItem.SEOTitle = item.SEOTitle;
                    dbItem.CoThuoctinh = item.CoThuoctinh;
                    dbItem.Image = item.Image;
                    dbItem.LuotClick = item.LuotClick;
                    dbItem.ChophepNhieu = item.ChophepNhieu;
                    dbItem.IsHome = item.IsHome;
                    dbItem.IsHot = item.IsHot;
                    dbItem.IsMenu = item.IsMenu;
                    dbItem.IsContend = item.IsContend;
                    dbItem.Isfooter = item.Isfooter;
                    dbItem.UrlConten = item.UrlConten;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(SysCategory item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysCategories.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.SysCategories.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(SysCategory item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysCategories.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Daxoa = true;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public SysCategory GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysCategories
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public SysCategoryInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysCategories
                            where n.Id == id
                            select new SysCategoryInfo()
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                Isfooter = n.Isfooter,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                UrlConten = n.UrlConten,
                            };
                return query.FirstOrDefault();
            }
        }
        public SysCategoryInfo GetInfoBykey(string key)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysCategories
                            where n.SEOUrl == key
                            select new SysCategoryInfo()
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                Isfooter = n.Isfooter,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                UrlConten = n.UrlConten,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(SysCategoryParam param)
        {
            var filter = param.SysCategoryFilter;
            var listid = new List<int?>();
            if (filter.Id.HasValue)
            {
                listid = Getsubcate(filter.Id.Value, filter.ListUserName);
            }
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysCategories
                            join p in dbContext.SysCategories on n.ParentId equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where
                             (filter.type.HasValue == false || n.LoaiCate == filter.type)
                            && (listid.Count <= 0 || listid.Contains(n.Id))
                            && (string.IsNullOrEmpty(filter.Search) || n.TenHienthi.Contains(filter.Search))
                            && n.Daxoa == Constant.IsNotDeleted
                             && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                             && (filter.IsHome.HasValue == false || n.IsHome == filter.IsHome)
                              && (filter.Isfooter.HasValue == false || n.Isfooter == filter.Isfooter)
                               && (filter.IsContend.HasValue == false || n.IsContend == filter.IsContend)
                                && (filter.IsMenu.HasValue == false || n.IsMenu == filter.IsMenu)
                            select new SysCategoryInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                Chuyenmuccha = parent.TenHienthi,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                Isfooter = n.Isfooter,
                                UrlConten = n.UrlConten,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(SysCategoryParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysCategories
                            join p in dbContext.SysCategories on n.ParentId equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where n.Daxoa == Constant.IsNotDeleted
                            select new SysCategoryInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                Chuyenmuccha = parent.TenHienthi,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                Isfooter = n.Isfooter,
                                UrlConten = n.UrlConten,
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void Getparentbycate(SysCategoryParam param)
        {
            var filter = param.SysCategoryFilter;
            var listid = new List<int?>();
            if (filter.Id.HasValue)
            {
                listid = Getsubcate(filter.Id.Value, new List<string>());
            }
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysCategories
                            join p in dbContext.SysCategories on n.ParentId equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.ParentId == filter.Id)
                            && (filter.type.HasValue == false || n.LoaiCate == filter.type)
                            && (listid.Count <= 0 || listid.Contains(n.Id))
                            && (string.IsNullOrEmpty(filter.Search) || n.TenHienthi.Contains(filter.Search))
                            && n.Daxoa == Constant.IsNotDeleted

                            select new SysCategoryInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                Chuyenmuccha = parent.TenHienthi,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                Isfooter = n.Isfooter,
                                UrlConten = n.UrlConten,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void PrepareData(List<SysCategoryInfo> list, List<SysCategoryInfo> listsource, int ParentId, string saperator = "")
        {
            var listdata = ParentId == 0 ? listsource.Where(p => p.ParentId.HasValue == false).ToList() :
                listsource.Where(p => p.ParentId == ParentId).ToList();
            foreach (var n in listdata.OrderBy(p => p.Thutu))
            {
                var additem = new SysCategoryInfo()
                {
                    Id = n.Id,
                    TenHienthi = saperator + n.TenHienthi,
                    Ma = n.Ma,
                    ParentId = n.ParentId,
                    LoaiCate = n.LoaiCate,
                    Thutu = n.Thutu,
                    Daxoa = n.Daxoa,
                    NgayTao = n.NgayTao,
                    NgaySua = n.NgaySua,
                    Nguoitao = n.Nguoitao,
                    NguoiSua = n.NguoiSua,
                    Phienban = n.Phienban,
                    LeftCode = n.LeftCode,
                    IdDonvi = n.IdDonvi,
                    Thumb = n.Thumb,
                    Mota = n.Mota,
                    SEOUrl = n.SEOUrl,
                    SEODescription = n.SEODescription,
                    SEOKeyword = n.SEOKeyword,
                    SEOTitle = n.SEOTitle,
                    CoThuoctinh = n.CoThuoctinh,
                    Image = n.Image,
                    LuotClick = n.LuotClick,
                    ChophepNhieu = n.ChophepNhieu,
                    IsHot = n.IsHot,
                    IsHome = n.IsHome,
                    IsMenu = n.IsMenu,
                    IsContend = n.IsContend,
                    IdContend = n.IdContend,
                    Isfooter = n.Isfooter,
                    UrlConten = n.UrlConten,
                };
                list.Add(additem);
                PrepareData(list, listsource, additem.Id, saperator + "--");
            }
        }
        public void SearchListParam(SysCategoryParam param)
        {
            var filter = param.SysCategoryFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var list = (from n in dbContext.SysCategories
                            where n.Daxoa == Constant.IsNotDeleted && (filter.Id.HasValue == false || filter.Id == n.ParentId)
                            && (filter.type.HasValue == false || n.LoaiCate == filter.type)
                            && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                            select new SysCategoryInfo()
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                Isfooter = n.Isfooter,
                                UrlConten = n.UrlConten,
                            }).ToList();
                param.SysCategoryInfos = new List<SysCategoryInfo>();
                if (list != null)
                {
                    PrepareData(param.SysCategoryInfos, list, 0, "");
                }
            }
        }
        private List<int?> Getsubcate(int parent, List<string> listNguoiTao)
        {
            var sublist = new List<int?>();
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var querycatelist = (from n in dbContext.SysCategories
                                     where
                                         n.Daxoa == Constant.IsNotDeleted &&
                                         (n.ParentId == parent || (parent == 0 && n.ParentId.HasValue == false))
                                         && (listNguoiTao.Count() == 0 || listNguoiTao.Contains(n.Nguoitao))
                                     select n.Id).ToList();
                foreach (var sub in querycatelist)
                {
                    sublist.AddRange(Getsubcate(sub, listNguoiTao));
                    sublist.Add(sub);
                }
            }
            return sublist;

        }
        public void GetCatebygianhang(SysCategoryParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var finter = param.SysCategoryFilter;
                var query = from n in dbContext.SysCategories
                            join p in dbContext.SysCategories on n.ParentId equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where n.Daxoa == Constant.IsNotDeleted
                            select new SysCategoryInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Ma = n.Ma,
                                ParentId = n.ParentId,
                                LoaiCate = n.LoaiCate,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                LeftCode = n.LeftCode,
                                IdDonvi = n.IdDonvi,
                                Thumb = n.Thumb,
                                Mota = n.Mota,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                CoThuoctinh = n.CoThuoctinh,
                                Image = n.Image,
                                LuotClick = n.LuotClick,
                                ChophepNhieu = n.ChophepNhieu,
                                Chuyenmuccha = parent.TenHienthi,
                                IsHot = n.IsHot,
                                IsHome = n.IsHome,
                                IsMenu = n.IsMenu,
                                IsContend = n.IsContend,
                                IdContend = n.IdContend,
                                Isfooter = n.Isfooter,
                                UrlConten = n.UrlConten,
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysCategoryInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        #endregion
    }
}
