using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class DmTinhthanhDao : BaseDao
    {
        #region Action
        public int Insert(DmTinhthanh item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.DmTinhthanhs.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(DmTinhthanh item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmTinhthanhs.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Matinh = item.Matinh;
                    dbItem.Tentinh = item.Tentinh;
                    dbItem.MaBuuchinh = item.MaBuuchinh;
                    dbItem.MavungDT = item.MavungDT;
                    dbItem.BiensoXe = item.BiensoXe;
                    dbItem.IdQuocgia = item.IdQuocgia;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.Thutu = item.Thutu;
                    dbItem.Trangthai = item.Trangthai;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(DmTinhthanh item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmTinhthanhs.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.DmTinhthanhs.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(DmTinhthanh item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmTinhthanhs.FirstOrDefault(en => en.Id == item.Id);
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
        public DmTinhthanh GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTinhthanhs
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public DmTinhthanhInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTinhthanhs
                            where n.Id == id
                            select new DmTinhthanhInfo()
                            {
                                Id = n.Id,
                                Matinh = n.Matinh,
                                Tentinh = n.Tentinh,
                                MaBuuchinh = n.MaBuuchinh,
                                MavungDT = n.MavungDT,
                                BiensoXe = n.BiensoXe,
                                IdQuocgia = n.IdQuocgia,
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
        public void Search(DmTinhthanhParam param)
        {
            var filter = param.DmTinhthanhFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTinhthanhs
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                                  && (string.IsNullOrEmpty(filter.Search) || n.Tentinh.Contains(filter.Search))
                                  && (filter.Idquocgia.HasValue == false || n.IdQuocgia == filter.Idquocgia)
                            && n.Daxoa == Constant.IsNotDeleted
                            select new DmTinhthanhInfo
                            {
                                Id = n.Id,
                                Matinh = n.Matinh,
                                Tentinh = n.Tentinh,
                                MaBuuchinh = n.MaBuuchinh,
                                MavungDT = n.MavungDT,
                                BiensoXe = n.BiensoXe,
                                IdQuocgia = n.IdQuocgia,
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
                    param.DmTinhthanhInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmTinhthanhInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(DmTinhthanhParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTinhthanhs
                            where n.Daxoa == Constant.IsNotDeleted
                            select new DmTinhthanhInfo
                            {
                                Id = n.Id,
                                Matinh = n.Matinh,
                                Tentinh = n.Tentinh,
                                MaBuuchinh = n.MaBuuchinh,
                                MavungDT = n.MavungDT,
                                BiensoXe = n.BiensoXe,
                                IdQuocgia = n.IdQuocgia,
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
                    param.DmTinhthanhInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmTinhthanhInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }


        #endregion
    }
}
