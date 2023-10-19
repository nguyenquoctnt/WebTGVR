using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VDMMutiline.SharedComponent.Search
{
    public class ExpressionFilterGroup<TEntity> : List<Expression<Func<TEntity, bool>>>
    {
    }
}
