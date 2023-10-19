using System.Linq;
using System.Web.UI.WebControls;

namespace VDMMutiline.SharedComponent.Sort
{
    public interface ISortCriteria<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> query);
        IOrderedQueryable<T> ApplyThenby(IOrderedQueryable<T> query);

        SortDirection Direction { get; set; }
    }
}
