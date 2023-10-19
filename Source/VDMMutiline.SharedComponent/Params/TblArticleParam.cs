using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class TblArticleParam:BaseParam
    {
        #region Action
        public decimal Id { get; set; }
        public int? Take { get; set; }
        public string key { get; set; }
        public int? ChuyenMucID { get; set; }
        public PagedList.IPagedList<TblArticleInfo> PagedList { get; set; }
        public TblArticle TblArticle { get; set; }
        public List<TblArticle> TblArticles { get; set; }
        public TblArticleInfo TblArticleInfo { get; set; }
        public List<TblArticleInfo> TblArticleInfos { get; set; }
        public TblArticleFilter TblArticleFilter { get; set; }
        public List<TblCategoryArticleInfo> TblCategoryArticleInfos { get; set; }
        public SysCategoryInfo SysCategoryInfo { get; set; }
        public List<SysCategoryInfo> SysCategoryInfos { get; set; }
        public List<int> Listcate { get; set; }
        #endregion
    }
    public class TblArticleFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? typeid  { get; set; }
        public int? ChuyenMucID { get; set; }
        public List<string> ListUserName { get; set; }
        public bool? Ishot { get; set; }
        public bool? IsShowMenu { get; set; }
    }

}