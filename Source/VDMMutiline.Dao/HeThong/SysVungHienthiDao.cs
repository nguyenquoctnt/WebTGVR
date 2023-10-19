using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class SysVungHienthiDao : BaseDao
    {
        #region Action
        public int Insert(SysVungHienthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.SysVungHienthis.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(SysVungHienthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysVungHienthis.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                   
                    dbItem.Id = item.Id;
dbItem.LoaiVung = item.LoaiVung;
dbItem.Tenvung = item.Tenvung;
dbItem.Sohienthi = item.Sohienthi;
dbItem.ThoigianTu = item.ThoigianTu;
dbItem.ThoigianDen = item.ThoigianDen;
dbItem.ChophepCache = item.ChophepCache;
dbItem.ChophepLienKet = item.ChophepLienKet;
dbItem.HienthiTieude = item.HienthiTieude;
dbItem.Width = item.Width;
dbItem.Height = item.Height;
dbItem.SpecStyle = item.SpecStyle;
dbItem.ItemCss = item.ItemCss;
dbItem.NgaySua = item.NgaySua;
dbItem.NguoiSua = item.NguoiSua;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(SysVungHienthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysVungHienthis.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.SysVungHienthis.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
		 public bool DeleteUpdate(SysVungHienthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysVungHienthis.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Daxoa = true;
dbItem.NgaySua = item.NgaySua;
dbItem.NguoiSua = item.NguoiSua;

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public SysVungHienthi GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysVungHienthis
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public SysVungHienthiInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysVungHienthis
                            where n.Id == id
                            select new SysVungHienthiInfo()
                            {
                                Id = n.Id,
LoaiVung = n.LoaiVung,
Tenvung = n.Tenvung,
Sohienthi = n.Sohienthi,
ThoigianTu = n.ThoigianTu,
ThoigianDen = n.ThoigianDen,
ChophepCache = n.ChophepCache,
ChophepLienKet = n.ChophepLienKet,
HienthiTieude = n.HienthiTieude,
Width = n.Width,
Height = n.Height,
SpecStyle = n.SpecStyle,
ItemCss = n.ItemCss,
Daxoa = n.Daxoa,
NgayTao = n.NgayTao,
NgaySua = n.NgaySua,
Nguoitao = n.Nguoitao,
NguoiSua = n.NguoiSua,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(SysVungHienthiParam param)
        {
            var filter = param.SysVungHienthiFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysVungHienthis
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
							&& n.Daxoa == Constant.IsNotDeleted
                            select new SysVungHienthiInfo
                            {
                                Id = n.Id,
LoaiVung = n.LoaiVung,
Tenvung = n.Tenvung,
Sohienthi = n.Sohienthi,
ThoigianTu = n.ThoigianTu,
ThoigianDen = n.ThoigianDen,
ChophepCache = n.ChophepCache,
ChophepLienKet = n.ChophepLienKet,
HienthiTieude = n.HienthiTieude,
Width = n.Width,
Height = n.Height,
SpecStyle = n.SpecStyle,
ItemCss = n.ItemCss,
Daxoa = n.Daxoa,
NgayTao = n.NgayTao,
NgaySua = n.NgaySua,
Nguoitao = n.Nguoitao,
NguoiSua = n.NguoiSua,

                            };
              
				  if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysVungHienthiInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysVungHienthiInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(SysVungHienthiParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysVungHienthis
							where n.Daxoa == Constant.IsNotDeleted
                            select new SysVungHienthiInfo
                            {
								Id = n.Id,
LoaiVung = n.LoaiVung,
Tenvung = n.Tenvung,
Sohienthi = n.Sohienthi,
ThoigianTu = n.ThoigianTu,
ThoigianDen = n.ThoigianDen,
ChophepCache = n.ChophepCache,
ChophepLienKet = n.ChophepLienKet,
HienthiTieude = n.HienthiTieude,
Width = n.Width,
Height = n.Height,
SpecStyle = n.SpecStyle,
ItemCss = n.ItemCss,
Daxoa = n.Daxoa,
NgayTao = n.NgayTao,
NgaySua = n.NgaySua,
Nguoitao = n.Nguoitao,
NguoiSua = n.NguoiSua,

                            };
              if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysVungHienthiInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysVungHienthiInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }

       
        #endregion
    }
}
