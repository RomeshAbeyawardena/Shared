using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shared.Contracts
{
    public interface IExpressionBuilder
    {
        IExpressionBuilder And(string name, object value = null, bool? isNull = null);
        IExpressionBuilder Or(string name, object value = null, bool? isNull = null);
        IExpressionBuilder Not(string name, object value = null, bool? isNull = null);
        Expression<Func<TEntity, bool>> ToExpression<TEntity>();
    }
}
