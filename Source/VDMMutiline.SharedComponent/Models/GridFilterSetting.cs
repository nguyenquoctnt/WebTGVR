using VDMMutiline.SharedComponent.Search;
using VDMMutiline.SharedComponent.Sort;

namespace VDMMutiline.SharedComponent.Models
{
    public class GridFilterSetting<TEntity> where TEntity : class
    {
        public int GetSortColumnIndex()
        {
            return iSortCol_0;
        }

        public string GetSortDirection()
        {
            return sSortDir_0;
        }

        ///public ExpressionFilterGroup<TEntity> ExpressionFilterGroup { get; set; }

        public FilterGroup<TEntity> FilterGroup { get; set; }

        public int iColumns { get; set; }

        public int iDisplayLength { get; set; }

        public int iDisplayStart { get; set; }

        public int iSortCol_0 { get; set; }

        public int iSortCol_1 { get; set; }

        public int iSortingCols { get; set; }

        public string sColumns { get; set; }

        public string sEcho { get; set; }

        public ISortCriteria<TEntity> sortCriteria { get; set; }

        public ISortCriteria<TEntity> sortCriteria_1 { get; set; }

        public string sSearch { get; set; }

        public string sSortDir_0 { get; set; }

        public string sSortDir_1 { get; set; }
    }
}
