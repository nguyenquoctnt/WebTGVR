using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class TblCategoryArticleParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public TblCategoryArticle TblCategoryArticle { get; set; }

        public List<TblCategoryArticle> TblCategoryArticles { get; set; }

        public TblCategoryArticleInfo TblCategoryArticleInfo { get; set; }

        public List<TblCategoryArticleInfo> TblCategoryArticleInfos { get; set; }

        public TblCategoryArticleFilter TblCategoryArticleFilter { get; set; }
        #endregion
    }
    public class TblCategoryArticleFilter : BaseFilter
    {
        public int? Id { get; set; }
        public int? ArticalId { get; set; }
    }

}