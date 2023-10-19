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
    public class TempleatePropertyDao : BaseDao
    {
        #region Action
        public int Insert(tbl_TempleateProperty item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_TempleateProperties.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_TempleateProperty item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleateProperties.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.IdGroup = item.IdGroup;
                    dbItem.Keyd = item.Keyd;
                    dbItem.Name = item.Name;
                    dbItem.Note = item.Note;
                    dbItem.TypeControl = item.TypeControl;
                    dbItem.DefaultValue = item.DefaultValue;
                    dbItem.UpdateDate = item.UpdateDate;
                    dbItem.UpdateUser = item.UpdateUser;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_TempleateProperty item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleateProperties.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbContext.tbl_TempleateProperties.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_TempleateProperty GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateProperties
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public TempleatePropertyInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateProperties
                            where n.Id == id
                            select new TempleatePropertyInfo()
                            {
                                Id = n.Id,
                                IdGroup = n.IdGroup,
                                Keyd = n.Keyd,
                                Name = n.Name,
                                Note = n.Note,
                                TypeControl = n.TypeControl,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                DefaultValue = n.DefaultValue
                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void Search(TempleatePropertyParam param)
        {
            var filter = param.TempleatePropertyFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateProperties
                            join p in dbContext.tbl_TempleatePropertyGroups on n.IdGroup equals p.Id
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                               && (filter.ItemId.HasValue == false || filter.ItemId == 0 || n.IdGroup == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.Name.Contains(filter.Search))
                            select new TempleatePropertyInfo
                            {
                                Id = n.Id,
                                IdGroup = n.IdGroup,
                                Keyd = n.Keyd,
                                Name = n.Name,
                                Note = n.Note,
                                TypeControl = n.TypeControl,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                NameGroup = p.Name,
                                DefaultValue = n.DefaultValue
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleatePropertyInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleatePropertyInfos = query.AsParallel().ToList();

                }
                foreach (var item in param.TempleatePropertyInfos)
                {
                    item.TypeName = Ultilities.Utility.GetDictionaryValue(Constant.LoaiThuocTinh.dctName, item.TypeControl);
                }
            }
        }
        public void GetAll(TempleatePropertyParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateProperties
                            join p in dbContext.tbl_TempleatePropertyGroups on n.IdGroup equals p.Id
                            select new TempleatePropertyInfo
                            {
                                Id = n.Id,
                                IdGroup = n.IdGroup,
                                Keyd = n.Keyd,
                                Name = n.Name,
                                Note = n.Note,
                                TypeControl = n.TypeControl,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                NameGroup = p.Name,
                                DefaultValue = n.DefaultValue
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleatePropertyInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleatePropertyInfos = query.AsParallel().ToList();

                }
            }
        }
        #endregion
    }
}
