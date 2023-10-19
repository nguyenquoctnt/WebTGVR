using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
using VDMMutiline.SharedComponent.Params.TempleteSetting;

namespace VDMMutiline.Dao.TempleteSetting
{
    public class TempleatePropertyGroupDao : BaseDao
    {
        #region Action
        public int Insert(tbl_TempleatePropertyGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                item.Active = false;
                dbContext.tbl_TempleatePropertyGroups.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_TempleatePropertyGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleatePropertyGroups.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Note = item.Note;
                    dbItem.ThuoctinhCha = item.ThuoctinhCha;
                    dbItem.UpdateDate = item.UpdateDate;
                    dbItem.UpdateUser = item.UpdateUser;
                    dbItem.ThuTu = item.ThuTu;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_TempleatePropertyGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleatePropertyGroups.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.tbl_TempleatePropertyGroups.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_TempleatePropertyGroup GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyGroups
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public TempleatePropertyGroupInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyGroups
                            where n.Id == id
                            select new TempleatePropertyGroupInfo()
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                ThuoctinhCha = n.ThuoctinhCha,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                ThuTu = n.ThuTu,
                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void Search(TempleatePropertyGroupParam param)
        {
            var filter = param.TempleatePropertyGroupFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyGroups
                            join p in dbContext.tbl_TempleatePropertyGroups on n.ThuoctinhCha equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.ItemId.HasValue == false || filter.ItemId == 0 || n.ThuoctinhCha == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.Name.Contains(filter.Search))
                            && n.Active != true
                            select new TempleatePropertyGroupInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                ThuoctinhCha = n.ThuoctinhCha,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                TenThuocTinhCha = parent.Name,
                                ThuTu = n.ThuTu,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleatePropertyGroupInfos = query.OrderBy(z => z.ThuTu).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleatePropertyGroupInfos = query.OrderBy(z => z.ThuTu).AsParallel().ToList();

                }
            }
        }
        public void GetAll(TempleatePropertyGroupParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyGroups
                            where   n.Active != true
                            select new TempleatePropertyGroupInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                ThuoctinhCha = n.ThuoctinhCha,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                TenThuocTinhCha = "",
                                ThuTu = n.ThuTu,
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleatePropertyGroupInfos = query.OrderBy(z => z.ThuTu).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleatePropertyGroupInfos = query.OrderBy(z => z.ThuTu).AsParallel().ToList();

                }
            }
        }
        public void PrepareTree(TempleatePropertyGroupParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyGroups
                            where  n.Active != true
                            select new TempleatePropertyGroupInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                ThuoctinhCha = n.ThuoctinhCha,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                TenThuocTinhCha = "",
                                ThuTu = n.ThuTu,
                            };
                var datalist = query.OrderBy(z => z.ThuTu).AsParallel().ToList();
                var prepard = new List<TempleatePropertyGroupInfo>();
                prepard.Add(new TempleatePropertyGroupInfo() { Name = "[Chọn]", Id = 0 });
                PrepareData(prepard, datalist, 0, "");
                param.TempleatePropertyGroupInfos = prepard;
            }
        }
        public void PrepareData(List<TempleatePropertyGroupInfo> list, List<TempleatePropertyGroupInfo> listsource, int ParentId, string saperator = "")
        {
            var listdata = ParentId == 0 ? listsource.Where(p => p.ThuoctinhCha.HasValue == false).ToList() :
                listsource.Where(p => p.ThuoctinhCha == ParentId).AsParallel().ToList();
            foreach (var n in listdata.AsParallel().OrderBy(p => p.ThuTu))
            {
                var additem = new TempleatePropertyGroupInfo()
                {
                    Id = n.Id,
                    Name = saperator + n.Name,
                    ThuTu = n.ThuTu,
                    ThuoctinhCha = n.ThuoctinhCha,
                };
                list.Add(additem);
                PrepareData(list, listsource, additem.Id, saperator + "--");
            }
        }
        public void SearchListParam(TempleatePropertyGroupParam param)
        {
            var filter = param.TempleatePropertyGroupFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var list = (from n in dbContext.tbl_TempleatePropertyGroups
                            where (filter.Id.HasValue == false || filter.Id == n.ThuoctinhCha)
                             && n.Active != true
                            select new TempleatePropertyGroupInfo()
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                ThuoctinhCha = n.ThuoctinhCha,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                TenThuocTinhCha = "",
                                ThuTu = n.ThuTu,
                            }).ToList();
                param.TempleatePropertyGroupInfos = new List<TempleatePropertyGroupInfo>();
                if (list != null)
                {
                    PrepareData(param.TempleatePropertyGroupInfos, list, 0, "-");
                }
            }
        }
        public void SearchAllMenu(TempleatePropertyGroupParam param)
        {
            var filter = param.TempleatePropertyGroupFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyGroups
                            join p in dbContext.tbl_TempleatePropertyGroups on n.ThuoctinhCha equals p.Id into ps
                            from parent in ps.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.ItemId.HasValue == false || filter.ItemId == 0 || n.ThuoctinhCha == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.Name.Contains(filter.Search))
                             && n.Active != true
                            select new TempleatePropertyGroupInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                ThuoctinhCha = n.ThuoctinhCha,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                TenThuocTinhCha = parent.Name,
                                ThuTu = n.ThuTu,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleatePropertyGroupInfos = query.OrderBy(z => z.ThuTu).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleatePropertyGroupInfos = query.OrderBy(z => z.ThuTu).AsParallel().ToList();

                }
            }
        }
        #endregion
    }
}
