using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class DmQuocgiaDao : BaseDao
    {
        #region Action
        public int Insert(DmQuocgia item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.DmQuocgias.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(DmQuocgia item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmQuocgias.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Maquogia = item.Maquogia;
                    dbItem.Tenquocgia = item.Tenquocgia;
                    dbItem.MaBuuchinh = item.MaBuuchinh;
                    dbItem.MavungDT = item.MavungDT;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.Thutu = item.Thutu;
                    dbItem.Trangthai = item.Trangthai;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(DmQuocgia item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmQuocgias.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.DmQuocgias.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(DmQuocgia item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.DmQuocgias.FirstOrDefault(en => en.Id == item.Id);
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
        public DmQuocgia GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmQuocgias
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public DmQuocgiaInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmQuocgias
                            where n.Id == id
                            select new DmQuocgiaInfo()
                            {
                                Id = n.Id,
                                Maquogia = n.Maquogia,
                                Tenquocgia = n.Tenquocgia,
                                MaBuuchinh = n.MaBuuchinh,
                                MavungDT = n.MavungDT,
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
        public void Search(DmQuocgiaParam param)
        {
            var filter = param.DmQuocgiaFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmQuocgias
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                            && (string.IsNullOrEmpty(filter.Search)|| n.Tenquocgia.Contains(filter.Search))
                            select new DmQuocgiaInfo
                            {
                                Id = n.Id,
                                Maquogia = n.Maquogia,
                                Tenquocgia = n.Tenquocgia,
                                MaBuuchinh = n.MaBuuchinh,
                                MavungDT = n.MavungDT,
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
                    param.DmQuocgiaInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmQuocgiaInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void GetAll(DmQuocgiaParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.DmQuocgias
                            where n.Daxoa == Constant.IsNotDeleted
                            select new DmQuocgiaInfo
                            {
                                Id = n.Id,
                                Maquogia = n.Maquogia,
                                Tenquocgia = n.Tenquocgia,
                                MaBuuchinh = n.MaBuuchinh,
                                MavungDT = n.MavungDT,
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
                    param.DmQuocgiaInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DmQuocgiaInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }


        #endregion
    }
}
