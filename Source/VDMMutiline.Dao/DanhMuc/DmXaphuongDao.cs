using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class DmXaphuongDao : BaseDao
    {
        #region Action
        public int Insert(DmXaphuong item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.DmXaphuongs.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(DmXaphuong item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmXaphuongs.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Maxa = item.Maxa;
                    dbItem.Tenxa = item.Tenxa;
                    dbItem.IdHuyen = item.IdHuyen;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.Thutu = item.Thutu;
                    dbItem.Trangthai = item.Trangthai;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(DmXaphuong item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmXaphuongs.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.DmXaphuongs.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(DmXaphuong item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmXaphuongs.FirstOrDefault(en => en.Id == item.Id);
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
        public DmXaphuong GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmXaphuongs
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public DmXaphuongInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmXaphuongs
                            where n.Id == id
                            select new DmXaphuongInfo()
                            {
                                Id = n.Id,
                                Maxa = n.Maxa,
                                Tenxa = n.Tenxa,
                                IdHuyen = n.IdHuyen,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(DmXaphuongParam param)
        {
            var filter = param.DmXaphuongFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmXaphuongs
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                            && (filter.IdHuyen.HasValue == false || n.IdHuyen == filter.IdHuyen)
                            select new DmXaphuongInfo
                            {
                                Id = n.Id,
                                Maxa = n.Maxa,
                                Tenxa = n.Tenxa,
                                IdHuyen = n.IdHuyen,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DmXaphuongInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmXaphuongInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(DmXaphuongParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmXaphuongs
                            where n.Daxoa == Constant.IsNotDeleted
                            select new DmXaphuongInfo
                            {
                                Id = n.Id,
                                Maxa = n.Maxa,
                                Tenxa = n.Tenxa,
                                IdHuyen = n.IdHuyen,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DmXaphuongInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmXaphuongInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }


        #endregion
    }
}
