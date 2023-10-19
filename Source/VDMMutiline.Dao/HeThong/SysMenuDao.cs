using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class SysMenuDao : BaseDao
    {
        #region Action
        public int Insert(SysMenu item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.SysMenus.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(SysMenu item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenus.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.TenHienthi = item.TenHienthi;
                    dbItem.Thutu = item.Thutu;
                    dbItem.ParentId = item.ParentId;
                    dbItem.Path = item.Path;
                    dbItem.IsSystem = item.IsSystem;
                    dbItem.cssClass = item.cssClass;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.Phienban++;
                    dbItem.IdDonvi = item.IdDonvi;
                    dbItem.SiteID = item.SiteID;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(SysMenu item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenus.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.SysMenus.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(SysMenu item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenus.FirstOrDefault(en => en.Id == item.Id);
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
        public SysMenu GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public SysMenuInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            where n.Id == id
                            select new SysMenuInfo()
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Thutu = n.Thutu,
                                ParentId = n.ParentId,
                                Path = n.Path,
                                IsSystem = n.IsSystem,
                                cssClass = n.cssClass,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                IdDonvi = n.IdDonvi,
                                SiteID=n.SiteID

                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void Search(SysMenuParam param)
        {
            var filter = param.SysMenuFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            join p in dbContext.SysMenus on n.ParentId equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.ItemId.HasValue == false || filter.ItemId == 0 || n.ParentId == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.TenHienthi.Contains(filter.Search))
                            && n.Daxoa == Constant.IsNotDeleted
                            select new SysMenuInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Thutu = n.Thutu,
                                ParentId = n.ParentId,
                                Path = n.Path,
                                IsSystem = n.IsSystem,
                                cssClass = n.cssClass,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                IdDonvi = n.IdDonvi,
                                TenCMCha = parent.TenHienthi,
                                SiteID=n.SiteID,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysMenuInfos = query.OrderBy(z => z.Thutu).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.SysMenuInfos = query.OrderBy(z => z.Thutu).AsParallel().ToList();

                }
            }
        }
        public void GetAll(SysMenuParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            where n.Daxoa == Constant.IsNotDeleted
                            select new SysMenuInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Thutu = n.Thutu,
                                ParentId = n.ParentId,
                                Path = n.Path,
                                IsSystem = n.IsSystem,
                                cssClass = n.cssClass,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                IdDonvi = n.IdDonvi,
                                SiteID=n.SiteID,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysMenuInfos = query.OrderBy(z => z.Thutu).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.SysMenuInfos = query.OrderBy(z => z.Thutu).AsParallel().ToList();

                }
            }
        }

        public void PrepareTree(SysMenuParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            where n.Daxoa == Constant.IsNotDeleted
                            select new SysMenuInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Thutu = n.Thutu,
                                ParentId = n.ParentId,
                                SiteID=n.SiteID,
                            };
                var datalist = query.OrderBy(z => z.Thutu).AsParallel().ToList();
                var prepard = new List<SysMenuInfo>();
                prepard.Add(new SysMenuInfo() { TenHienthi = "[Chọn menu]", Id = 0 });
                PrepareData(prepard, datalist, 0, "");
                param.SysMenuInfos = prepard;
            }
        }


        public void PrepareData(List<SysMenuInfo> list, List<SysMenuInfo> listsource, int ParentId, string saperator = "")
        {
            var listdata = ParentId == 0 ? listsource.Where(p => p.ParentId.HasValue == false).ToList() :
                listsource.Where(p => p.ParentId == ParentId).AsParallel().ToList();
            foreach (var n in listdata.AsParallel().OrderBy(p => p.Thutu))
            {
                var additem = new SysMenuInfo()
                {
                    Id = n.Id,
                    TenHienthi = saperator + n.TenHienthi,
                    Thutu = n.Thutu,
                    ParentId = n.ParentId,
                    SiteID=n.SiteID,
                };
                list.Add(additem);
                PrepareData(list, listsource, additem.Id, saperator + "--");
            }
        }


        public void SearchListParam(SysMenuParam param)
        {
            var filter = param.SysMenuFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var list = (from n in dbContext.SysMenus
                            where n.Daxoa == Constant.IsNotDeleted && (filter.Id.HasValue == false || filter.Id == n.ParentId)
                            select new SysMenuInfo()
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Thutu = n.Thutu,
                                ParentId = n.ParentId,
                                Path = n.Path,
                                IsSystem = n.IsSystem,
                                cssClass = n.cssClass,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                IdDonvi = n.IdDonvi,
                                SiteID=n.SiteID,
                            }).ToList();
                param.SysMenuInfos = new List<SysMenuInfo>();
                if (list != null)
                {
                    PrepareData(param.SysMenuInfos, list, 0, "-");
                }
            }
        }

        public void SearchAllMenu(SysMenuParam param)
        {
            var filter = param.SysMenuFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            join p in dbContext.SysMenus on n.ParentId equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.ItemId.HasValue == false || filter.ItemId == 0 || n.ParentId == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.TenHienthi.Contains(filter.Search))
                            && n.Daxoa == Constant.IsNotDeleted
                            select new SysMenuInfo
                            {
                                Id = n.Id,
                                TenHienthi = n.TenHienthi,
                                Thutu = n.Thutu,
                                ParentId = n.ParentId,
                                Path = n.Path,
                                IsSystem = n.IsSystem,
                                cssClass = n.cssClass,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                Phienban = n.Phienban,
                                IdDonvi = n.IdDonvi,
                                TenCMCha = parent.TenHienthi,
                                SiteID=n.SiteID,
                                SysMenuGroups= dbContext.SysMenuGroups.Where(z=>z.MenuId==n.Id).Select(z=>z.GroupId)
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysMenuInfos = query.OrderBy(z => z.Thutu).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.SysMenuInfos = query.OrderBy(z => z.Thutu).AsParallel().ToList();

                }
            }
        }
        #endregion
    }
}
