using System.Linq.Expressions;

namespace VDMMutiline.SharedComponent.Search
{
    public interface IFilter<T>
    {
        Expression getExpression(ParameterExpression param);
    }
}
