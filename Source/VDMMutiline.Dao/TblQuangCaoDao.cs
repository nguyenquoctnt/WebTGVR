using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class TblQuangCaoDao : BaseDao
    {
        #region Action
        public int Insert(TblQuangCao item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.TblQuangCaos.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(TblQuangCao item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblQuangCaos.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Loai = item.Loai;
                    dbItem.Vitri = item.Vitri;
                    dbItem.Path = item.Path;
                    dbItem.Url = item.Url;
                    dbItem.Mota = item.Mota;
                    dbItem.LuotXem = item.LuotXem;
                    dbItem.LuotClick = item.LuotClick;
                    dbItem.ThoigianTu = item.ThoigianTu;
                    dbItem.ThoigianDen = item.ThoigianDen;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Thutu = item.Thutu;
                    dbItem.Trangthai = item.Trangthai;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(TblQuangCao item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblQuangCaos.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.TblQuangCaos.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(TblQuangCao item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblQuangCaos.FirstOrDefault(en => en.Id == item.Id);
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
        public TblQuangCao GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblQuangCaos
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public TblQuangCaoInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblQuangCaos
                            where n.Id == id
                            select new TblQuangCaoInfo()
                            {
                                Id = n.Id,
                                Loai = n.Loai,
                                Vitri = n.Vitri,
                                Path = n.Path,
                                Url = n.Url,
                                Mota = n.Mota,
                                LuotXem = n.LuotXem,
                                LuotClick = n.LuotClick,
                                ThoigianTu = n.ThoigianTu,
                                ThoigianDen = n.ThoigianDen,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(TblQuangCaoParam param)
        {
            var filter = param.TblQuangCaoFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblQuangCaos
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                              && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                            select new TblQuangCaoInfo
                            {
                                Id = n.Id,
                                Loai = n.Loai,
                                Vitri = n.Vitri,
                                Path = n.Path,
                                Url = n.Url,
                                Mota = n.Mota,
                                LuotXem = n.LuotXem,
                                LuotClick = n.LuotClick,
                                ThoigianTu = n.ThoigianTu,
                                ThoigianDen = n.ThoigianDen,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TblQuangCaoInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.TblQuangCaoInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(TblQuangCaoParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblQuangCaos
                            where n.Daxoa == Constant.IsNotDeleted
                            select new TblQuangCaoInfo
                            {
                                Id = n.Id,
                                Loai = n.Loai,
                                Vitri = n.Vitri,
                                Path = n.Path,
                                Url = n.Url,
                                Mota = n.Mota,
                                LuotXem = n.LuotXem,
                                LuotClick = n.LuotClick,
                                ThoigianTu = n.ThoigianTu,
                                ThoigianDen = n.ThoigianDen,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TblQuangCaoInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.TblQuangCaoInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }

        public void GetKhoaVung(TblQuangCaoParam param)
        {
            var filter = param.TblQuangCaoFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblQuangCaos
                            join vung in dbContext.SysVungHienthis on n.Vitri equals vung.Id
                            where vung.Id == filter.Vung
                            && (vung.ThoigianTu.HasValue == false || vung.ThoigianTu >= System.DateTime.Now.Date)
                            && (vung.ThoigianDen.HasValue == false || vung.ThoigianDen < System.DateTime.Now.Date.AddDays(1))
                            && n.Daxoa == Constant.IsNotDeleted
                              && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                            select new TblQuangCaoInfo
                            {
                                Id = n.Id,
                                Loai = n.Loai,
                                LuotClick = n.LuotClick,
                                NgaySua = n.NgaySua,
                                Vitri = n.Vitri,
                                Mota = n.Mota,
                                Daxoa = n.Daxoa,
                                Thutu = n.Thutu,
                                LuotXem = n.LuotXem,
                                NgayTao = n.NgayTao,
                                NguoiSua = n.NguoiSua,
                                Nguoitao = n.Nguoitao,
                                Path = n.Path,
                                Url = n.Url,
                                ItemWidth = n.SysVungHienthi.Width,
                                ItemHeight = n.SysVungHienthi.Height
                            };
                param.TblQuangCaoInfos = query.ToList();
            }
        }
        #endregion
    }
}
