using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class SiteInfoDao : BaseDao
    {
        #region Action
        public int Insert(tbl_SiteInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_SiteInfos.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_SiteInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_SiteInfos.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.SiteName = item.SiteName;
                    dbItem.SiteNote = item.SiteNote;
                    dbItem.ThumPic = item.ThumPic;
                    dbItem.UrlPart = item.UrlPart;
                    dbItem.UrlPartMobile = item.UrlPartMobile;
                    dbItem.Actives = item.Actives;
                    dbItem.UpdateUser = item.UpdateUser;
                    dbItem.EditDate = item.EditDate;
                    dbItem.UrlDemo = item.UrlDemo;
                    dbItem.TieuDeBaiVietCung = item.TieuDeBaiVietCung;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_SiteInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_SiteInfos.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.tbl_SiteInfos.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_SiteInfo GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_SiteInfos
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public SiteInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_SiteInfos
                            where n.Id == id
                            select new SiteInfo()
                            {
                                Id = n.Id,
                                SiteName = n.SiteName,
                                SiteNote = n.SiteNote,
                                ThumPic = n.ThumPic,
                                UrlPart = n.UrlPart,
                                UrlPartMobile = n.UrlPartMobile,
                                Actives = n.Actives,
                                UpdateUser = n.UpdateUser,
                                EditDate = n.EditDate,
                                CreateDate = n.CreateDate,
                                CreateUser = n.CreateUser,
                                TieuDeBaiVietCung = n.TieuDeBaiVietCung,

                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void Search(SiteInfoParam param)
        {
            var filter = param.SiteInfoFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_SiteInfos
                            where
                             (string.IsNullOrEmpty(filter.Search) || n.SiteName.Contains(filter.Search))
                            select new SiteInfo()
                            {
                                Id = n.Id,
                                SiteName = n.SiteName,
                                SiteNote = n.SiteNote,
                                ThumPic = n.ThumPic,
                                UrlPart = n.UrlPart,
                                UrlPartMobile = n.UrlPartMobile,
                                Actives = n.Actives,
                                UpdateUser = n.UpdateUser,
                                EditDate = n.EditDate,
                                CreateDate = n.CreateDate,
                                CreateUser = n.CreateUser,
                                TieuDeBaiVietCung = n.TieuDeBaiVietCung

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SiteInfos = query.OrderBy(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SiteInfos = query.OrderBy(z => z.Id).ToList();

                }
            }
        }
        public void CopyContenSite(int? SiteID, string username)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var objsite = dbContext.tbl_SiteInfos.FirstOrDefault(z => z.Id == SiteID);
                if (objsite != null)
                {
                    var getlistquangcao = dbContext.TblQuangCaos.Where(z => z.Nguoitao == objsite.CopyIDUserDemo);
                    foreach (var item in getlistquangcao)
                    {
                        item.Nguoitao = username;
                        item.NguoiSua = username;
                        dbContext.TblQuangCaos.InsertOnSubmit(item);
                        dbContext.SubmitChanges();
                    }
                    var getlisttintuc = dbContext.TblArticles.Where(z => z.Nguoitao == objsite.CopyIDUserDemo);
                    foreach (var item in getlisttintuc)
                    {
                        item.Nguoitao = username;
                        item.NguoiSua = username;
                        dbContext.TblArticles.InsertOnSubmit(item);
                        dbContext.SubmitChanges();
                    }
                    var objgetinfofrom = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == objsite.CopyIDUserDemo);
                    if (objgetinfofrom != null)
                    {
                        var objgetinfoto = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == username);
                        if (objgetinfoto != null)
                        {
                            objgetinfoto.LogoUrl = objgetinfofrom.LogoUrl;
                            objgetinfoto.Gmap = objgetinfofrom.Gmap;
                            dbContext.SubmitChanges();
                        }
                    }

                }
            }
        }
        public void GetStyleBySiteID(SiteInfoParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_StyleSites
                            where
                            n.SiteId == param.Id
                            select n;
                param.StyleSites = query.ToList();
            }
        }
        #endregion
    }
}
