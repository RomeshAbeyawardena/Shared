using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shared.Contracts.Builders
{
    public interface IQueryBuilder<TEntity, TEnum>
        where TEntity : class
        where TEnum : struct
    {
        Expression<Func<TEntity, bool>> GetExpression(TEnum? enumeration);
    }
}
