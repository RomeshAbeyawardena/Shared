using System;
using System.Linq.Expressions;

namespace Shared.Contracts.Builders
{
    public interface IQueryBuilder<TEntity, TEnum>
        where TEntity : class
        where TEnum : struct
    {
        Expression<Func<TEntity, bool>> GetExpression(TEnum? enumeration);
    }
}
