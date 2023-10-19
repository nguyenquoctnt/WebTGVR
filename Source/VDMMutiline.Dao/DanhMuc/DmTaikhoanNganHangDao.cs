using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class DmTaikhoanNganHangDao : BaseDao
    {
        #region Action
        public int Insert(DmTaikhoanNganHang item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.DmTaikhoanNganHangs.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(DmTaikhoanNganHang item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmTaikhoanNganHangs.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Tentaikhoan = item.Tentaikhoan;
                    dbItem.Sotaikhoan = item.Sotaikhoan;
                    dbItem.Nganhang = item.Nganhang;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.Thutu = item.Thutu;
                    dbItem.ChiNhanh = item.ChiNhanh;
                    dbItem.Logo = item.Logo;
                    dbItem.Trangthai = item.Trangthai;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(DmTaikhoanNganHang item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmTaikhoanNganHangs.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.DmTaikhoanNganHangs.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(DmTaikhoanNganHang item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmTaikhoanNganHangs.FirstOrDefault(en => en.Id == item.Id);
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
        public DmTaikhoanNganHang GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTaikhoanNganHangs
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public DmTaikhoanNganHangInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTaikhoanNganHangs
                            where n.Id == id
                            select new DmTaikhoanNganHangInfo()
                            {
                                Id = n.Id,
                                Tentaikhoan = n.Tentaikhoan,
                                Sotaikhoan = n.Sotaikhoan,
                                Nganhang = n.Nganhang,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,
                                ChiNhanh = n.ChiNhanh,
                                Logo = n.Logo,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(DmTaikhoanNganHangParam param)
        {
            var filter = param.DmTaikhoanNganHangFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTaikhoanNganHangs
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                            && (string.IsNullOrEmpty(filter.Search) || n.Tentaikhoan.Contains(filter.Search))
                               && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                            select new DmTaikhoanNganHangInfo
                            {
                                Id = n.Id,
                                Tentaikhoan = n.Tentaikhoan,
                                Sotaikhoan = n.Sotaikhoan,
                                Nganhang = n.Nganhang,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,
                                ChiNhanh = n.ChiNhanh,
                                Logo = n.Logo,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DmTaikhoanNganHangInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmTaikhoanNganHangInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(DmTaikhoanNganHangParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmTaikhoanNganHangs
                            where n.Daxoa == Constant.IsNotDeleted
                            select new DmTaikhoanNganHangInfo
                            {
                                Id = n.Id,
                                Tentaikhoan = n.Tentaikhoan,
                                Sotaikhoan = n.Sotaikhoan,
                                Nganhang = n.Nganhang,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Trangthai = n.Trangthai,
                                ChiNhanh = n.ChiNhanh,
                                Logo = n.Logo,
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DmTaikhoanNganHangInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmTaikhoanNganHangInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        #endregion
    }
}
