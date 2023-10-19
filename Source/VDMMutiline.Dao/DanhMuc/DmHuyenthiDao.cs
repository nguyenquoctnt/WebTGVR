using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class DmHuyenthiDao : BaseDao
    {
        #region Action
        public int Insert(DmHuyenthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.DmHuyenthis.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(DmHuyenthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmHuyenthis.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Mahuyen = item.Mahuyen;
                    dbItem.Tenhuyen = item.Tenhuyen;
                    dbItem.IdTinh = item.IdTinh;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.Thutu = item.Thutu;
                    dbItem.Trangthai = item.Trangthai;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(DmHuyenthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmHuyenthis.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.DmHuyenthis.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(DmHuyenthi item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmHuyenthis.FirstOrDefault(en => en.Id == item.Id);
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
        public DmHuyenthi GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmHuyenthis
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public DmHuyenthiInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmHuyenthis
                            join tinhthanh in dbContext.DmTinhthanhs on n.IdTinh equals tinhthanh.Id
                            join quocgia in dbContext.DmQuocgias on tinhthanh.IdQuocgia equals quocgia.Id
                            where n.Id == id
                            select new DmHuyenthiInfo()
                            {
                                Id = n.Id,
                                Mahuyen = n.Mahuyen,
                                Tenhuyen = n.Tenhuyen,
                                IdTinh = n.IdTinh,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,
                                QuocgiaId = quocgia.Id
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(DmHuyenthiParam param)
        {
            var filter = param.DmHuyenthiFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmHuyenthis
                            join tinhthanh in dbContext.DmTinhthanhs on n.IdTinh equals tinhthanh.Id
                            join quocgia in dbContext.DmQuocgias on tinhthanh.IdQuocgia equals quocgia.Id
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.Idtinhthanh.HasValue == false || n.IdTinh == filter.Idtinhthanh)
                            && (string.IsNullOrEmpty(filter.Search) || n.Tenhuyen.Contains(filter.Search))
                            && n.Daxoa == Constant.IsNotDeleted
                            select new DmHuyenthiInfo
                            {
                                Id = n.Id,
                                Mahuyen = n.Mahuyen,
                                Tenhuyen = n.Tenhuyen,
                                IdTinh = n.IdTinh,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,
                                QuocgiaId = quocgia.Id

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DmHuyenthiInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmHuyenthiInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(DmHuyenthiParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmHuyenthis
                            join tinhthanh in dbContext.DmTinhthanhs on n.IdTinh equals tinhthanh.Id
                            join quocgia in dbContext.DmQuocgias on tinhthanh.IdQuocgia equals quocgia.Id
                            where n.Daxoa == Constant.IsNotDeleted

                            select new DmHuyenthiInfo
                            {
                                Id = n.Id,
                                Mahuyen = n.Mahuyen,
                                Tenhuyen = n.Tenhuyen,
                                IdTinh = n.IdTinh,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,
                                QuocgiaId = quocgia.Id
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DmHuyenthiInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmHuyenthiInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }


        #endregion
    }
}
