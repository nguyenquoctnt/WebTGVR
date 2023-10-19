using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
namespace VDMMutiline.Dao
{
    public class TblArticleDao : BaseDao
    {
        #region Action
        public decimal Insert(TblArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.TblArticles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                var dbItem = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.SEOUrl = (StringFormat.UrlHeper(item.Tenbai) + "-" + dbItem.Id);
                    dbContext.SubmitChanges();
                }
                return item.Id;
            }
        }
        public void Update(TblArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Tenbai = item.Tenbai;
                    dbItem.Mota = item.Mota;
                    dbItem.Noidung = item.Noidung;
                    dbItem.Image = item.Image;
                    dbItem.Thumb = item.Thumb;
                    dbItem.SEODescription = item.SEODescription;
                    dbItem.SEOKeyword = item.SEOKeyword;
                    dbItem.SEOTitle = item.SEOTitle;
                    dbItem.LuotXem = item.LuotXem;
                    dbItem.ChophepBinhluan = item.ChophepBinhluan;
                    dbItem.TheodoiXem = item.TheodoiXem;
                    dbItem.Phienban++;
                    dbItem.Thutu = item.Thutu;
                    dbItem.NgaySua = item.NgaySua;
                    dbItem.NguoiSua = item.NguoiSua;
                    dbItem.ChophepVote = item.ChophepVote;
                    dbItem.LoaiBaiviet = item.LoaiBaiviet;
                    dbItem.DiemBinhchon = item.DiemBinhchon;
                    dbItem.LuotBinhluan = item.LuotBinhluan;
                    dbItem.TinNoiBat = item.TinNoiBat;
                    dbItem.Hienthitrangchu = item.Hienthitrangchu;
                    dbItem.HienThiMenu = item.HienThiMenu;
                    dbItem.SEOUrl = (StringFormat.UrlHeper(item.Tenbai) + "-" + dbItem.Id);
                    dbContext.SubmitChanges();
                }
            }
        }
        public void UpdateCateID(int? cateid, decimal? articalID)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysCategories.FirstOrDefault(sitem => sitem.Id == cateid);
                if (dbItem != null)
                {
                    dbItem.IdContend = (int?)articalID;
                    dbContext.SubmitChanges();
                }
            }
        }
        public decimal Updata(TblArticle Article, List<int> Listcate)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                decimal id = 0;
                #region Article 
                if (Article.Id <= 0)
                {
                    Article.Daxoa = Constant.IsNotDeleted;
                    Article.Phienban = Constant.FirstVersion;
                    dbContext.TblArticles.InsertOnSubmit(Article);
                    dbContext.SubmitChanges();
                    id = Article.Id;
                }
                else
                {
                    var dbItem = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == Article.Id);
                    if (dbItem != null)
                    {
                        dbItem.Tenbai = Article.Tenbai;
                        dbItem.Mota = Article.Mota;
                        dbItem.Noidung = Article.Noidung;
                        dbItem.Image = Article.Image;
                        dbItem.Thumb = Article.Thumb;
                        dbItem.SEOUrl = Article.SEOUrl;
                        dbItem.SEODescription = Article.SEODescription;
                        dbItem.SEOKeyword = Article.SEOKeyword;
                        dbItem.SEOTitle = Article.SEOTitle;
                        dbItem.LuotXem = Article.LuotXem;
                        dbItem.ChophepBinhluan = Article.ChophepBinhluan;
                        dbItem.TheodoiXem = Article.TheodoiXem;
                        dbItem.Phienban++;
                        dbItem.Thutu = Article.Thutu;
                        dbItem.NgaySua = Article.NgaySua;
                        dbItem.NguoiSua = Article.NguoiSua;
                        dbItem.ChophepVote = Article.ChophepVote;
                        dbItem.LoaiBaiviet = Article.LoaiBaiviet;
                        dbItem.DiemBinhchon = Article.DiemBinhchon;
                        dbItem.LuotBinhluan = Article.LuotBinhluan;
                        dbItem.TinNoiBat = Article.TinNoiBat;
                        dbItem.Hienthitrangchu = Article.Hienthitrangchu;
                        dbItem.ChuyenMuc = Article.ChuyenMuc;
                        dbItem.HienThiMenu = Article.HienThiMenu;
                        dbContext.SubmitChanges();
                        id = dbItem.Id;
                    }
                    Article = dbItem;
                }
                #endregion
                #region Cate 
                var listdelete = dbContext.TblCategoryArticles.Where(z => z.IdArticle == Article.Id && !Listcate.Contains(z.IdCate.Value));
                dbContext.TblCategoryArticles.DeleteAllOnSubmit(listdelete);
                dbContext.SubmitChanges();
                var listcateold = dbContext.TblCategoryArticles.Where(z => z.IdArticle == Article.Id);
                var list = new List<TblCategoryArticle>();
                foreach (var item in Listcate)
                {
                    var checkdata = listcateold.FirstOrDefault(z => z.IdCate == item);
                    if (checkdata == null)
                    {
                        var obj = new TblCategoryArticle
                        {
                            IdCate = item,
                            IdArticle = Article.Id
                        };
                        list.Add(obj);
                    }
                }
                dbContext.TblCategoryArticles.InsertAllOnSubmit(list);
                dbContext.SubmitChanges();
                #endregion
                var dbItemseourl = dbContext.TblArticles.FirstOrDefault(sitem => sitem.Id == Article.Id);
                if (dbItemseourl != null)
                {
                    dbItemseourl.SEOUrl = (StringFormat.UrlHeper(Article.Tenbai) + "-" + dbItemseourl.Id);
                    dbContext.SubmitChanges();
                }
                return id;
            }
        }

        public bool Delete(TblArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblArticles.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.TblArticles.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(TblArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblArticles.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Phienban++;
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
        public TblArticle GetbyId(decimal id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblArticles
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public TblArticleInfo GetInfo(decimal id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblArticles
                            where n.Id == id
                            select new TblArticleInfo()
                            {
                                Id = n.Id,
                                Tenbai = n.Tenbai,
                                Mota = n.Mota,
                                Noidung = n.Noidung,
                                Image = n.Image,
                                Thumb = n.Thumb,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                LuotXem = n.LuotXem,
                                ChophepBinhluan = n.ChophepBinhluan,
                                TheodoiXem = n.TheodoiXem,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                ChophepVote = n.ChophepVote,
                                LoaiBaiviet = n.LoaiBaiviet,
                                DiemBinhchon = n.DiemBinhchon,
                                LuotBinhluan = n.LuotBinhluan,
                                TinNoiBat = n.TinNoiBat,
                                Hienthitrangchu = n.Hienthitrangchu,
                                ChuyenMuc = n.ChuyenMuc,
                                HienThiMenu = n.HienThiMenu,
                            };
                return query.FirstOrDefault();
            }
        }
        public TblArticleInfo GetInfo(string key)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblArticles
                            where n.SEOUrl == key
                            select new TblArticleInfo()
                            {
                                Id = n.Id,
                                Tenbai = n.Tenbai,
                                Mota = n.Mota,
                                Noidung = n.Noidung,
                                Image = n.Image,
                                Thumb = n.Thumb,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                LuotXem = n.LuotXem,
                                ChophepBinhluan = n.ChophepBinhluan,
                                TheodoiXem = n.TheodoiXem,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                ChophepVote = n.ChophepVote,
                                LoaiBaiviet = n.LoaiBaiviet,
                                DiemBinhchon = n.DiemBinhchon,
                                LuotBinhluan = n.LuotBinhluan,
                                TinNoiBat = n.TinNoiBat,
                                Hienthitrangchu = n.Hienthitrangchu,
                                ChuyenMuc = n.ChuyenMuc,
                                HienThiMenu = n.HienThiMenu,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(TblArticleParam param)
        {
            var filter = param.TblArticleFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblArticles

                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                            && (filter.typeid.HasValue == false || n.LoaiBaiviet == filter.typeid)
                               && (filter.ChuyenMucID.HasValue == false || n.ChuyenMuc == filter.ChuyenMucID)
                            && (string.IsNullOrEmpty(filter.Search) || n.Tenbai.Contains(filter.Search))
                              && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                                && (filter.Ishot.HasValue == false || n.Hienthitrangchu == filter.Ishot)
                                 && (filter.IsShowMenu.HasValue == false || n.HienThiMenu == filter.IsShowMenu)
                            select new TblArticleInfo
                            {
                                Id = n.Id,
                                Tenbai = n.Tenbai,
                                Mota = n.Mota,
                                Noidung = n.Noidung,
                                Image = n.Image,
                                Thumb = n.Thumb,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                LuotXem = n.LuotXem,
                                ChophepBinhluan = n.ChophepBinhluan,
                                TheodoiXem = n.TheodoiXem,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                ChophepVote = n.ChophepVote,
                                LoaiBaiviet = n.LoaiBaiviet,
                                DiemBinhchon = n.DiemBinhchon,
                                LuotBinhluan = n.LuotBinhluan,
                                TinNoiBat = n.TinNoiBat,
                                Hienthitrangchu = n.Hienthitrangchu,
                                ChuyenMuc = n.ChuyenMuc,
                                HienThiMenu = n.HienThiMenu,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TblArticleInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    if (param.Take > 0)
                    {
                        param.TblArticleInfos = query.OrderByDescending(z => z.NgayTao).Take(param.Take.Value).ToList();
                    }
                    else
                        param.TblArticleInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        public void SearchHome(TblArticleParam param)
        {
            var filter = param.TblArticleFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblArticles

                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && n.Daxoa == Constant.IsNotDeleted
                            && (filter.typeid.HasValue == false || n.LoaiBaiviet == filter.typeid)
                            && (string.IsNullOrEmpty(filter.Search) || n.Tenbai.Contains(filter.Search))
                            && (filter.ListUserName.Count == 0 || filter.ListUserName.Contains(n.Nguoitao))
                            && (filter.Ishot.HasValue == false || n.Hienthitrangchu == filter.Ishot)
                            && (filter.ChuyenMucID.HasValue == false || n.ChuyenMuc == filter.ChuyenMucID)
                            && (filter.IsShowMenu.HasValue == false || n.HienThiMenu == filter.IsShowMenu)
                            select new TblArticleInfo
                            {
                                Id = n.Id,
                                Tenbai = n.Tenbai,
                                Mota = n.Mota,
                                Noidung = n.Noidung,
                                Image = n.Image,
                                Thumb = n.Thumb,
                                SEOUrl = n.SEOUrl,
                                SEODescription = n.SEODescription,
                                SEOKeyword = n.SEOKeyword,
                                SEOTitle = n.SEOTitle,
                                LuotXem = n.LuotXem,
                                ChophepBinhluan = n.ChophepBinhluan,
                                TheodoiXem = n.TheodoiXem,
                                Phienban = n.Phienban,
                                Thutu = n.Thutu,
                                Daxoa = n.Daxoa,
                                NgayTao = n.NgayTao,
                                NgaySua = n.NgaySua,
                                Nguoitao = n.Nguoitao,
                                NguoiSua = n.NguoiSua,
                                ChophepVote = n.ChophepVote,
                                LoaiBaiviet = n.LoaiBaiviet,
                                DiemBinhchon = n.DiemBinhchon,
                                LuotBinhluan = n.LuotBinhluan,
                                TinNoiBat = n.TinNoiBat,
                                Hienthitrangchu = n.Hienthitrangchu,
                                ChuyenMuc = n.ChuyenMuc,
                                HienThiMenu = n.HienThiMenu,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TblArticleInfos =
                        query.OrderBy(z => z.Thutu).Skip(param.PagingInfo.RowsSkip).Take(param.PagingInfo.PageSize).ToList().ToList();
                }
                else
                {
                    if (param.Take > 0)
                    {
                        param.TblArticleInfos = query.OrderBy(z => z.Thutu).Take(param.Take.Value).ToList();
                    }
                    else
                        param.TblArticleInfos = query.OrderByDescending(z => z.Thutu).ToList();

                }
            }
        }
        #endregion
    }
}
