using System;
using System.Linq.Expressions;

namespace Shared.Contracts.Factories
{
    public interface IQueryBuilderFactory
    {
        Expression<Func<TEntity, bool>> GetExpression<TEntity, TEnum>(TEnum? enumeration)
            where TEntity : class
            where TEnum : struct;
    }
}
