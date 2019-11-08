using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Shared.Domains;

namespace Shared.Contracts
{
    public interface IExpressionBuilder
    {
        IExpressionBuilder And(string name, ExpressionComparer? expressionComparer = null, object value = null);
        IExpressionBuilder Or(string name, ExpressionComparer? expressionComparer = null, object value = null);
        IExpressionBuilder Not(string name, ExpressionComparer? expressionComparer = null, object value = null);
        Expression<Func<TEntity, bool>> ToExpression<TEntity>();
    }
}
