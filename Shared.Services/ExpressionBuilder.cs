using Shared.Contracts;
using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shared.Services
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        private readonly IDictionary<string, ExpressionParameter> expressionParameterDictionary;

        private IExpressionBuilder Add(string key, ExpressionCondition expressionCondition, object value = null, bool? isNull = null)
        {
            expressionParameterDictionary.Add(key, new ExpressionParameter
            {
                Name = key,
                Value = value,
                Condition = expressionCondition,
                IsNull = isNull
            });

            return this;
        }

        private ExpressionBuilder()
        {
            expressionParameterDictionary = new Dictionary<string, ExpressionParameter>();
        }

        public static IExpressionBuilder Create()
        {
            return new ExpressionBuilder();
        }

        public IExpressionBuilder And(string name, object value = null, bool? isNull = null)
        {
            return Add(name, ExpressionCondition.And, value, isNull);
        }

        public IExpressionBuilder Or(string name, object value = null, bool? isNull = null)
        {
            return Add(name, ExpressionCondition.Or, value, isNull);
        }

        public IExpressionBuilder Not(string name, object value = null, bool? isNull = null)
        {
            return Add(name, ExpressionCondition.Not, value, isNull);
        }

        public Expression<Func<TEntity, bool>> ToExpression<TEntity>()
        {
            var entityType = typeof(TEntity);
            var parameterExpression = Expression.Parameter(entityType, "model");
            Expression combinedExpression = null;
            var variableExpression = Expression.Variable(entityType);
            var nullConstantExpression = Expression.Constant(null);

            foreach (var (key, value) in expressionParameterDictionary)
            {
                var constantExpression = Expression.Constant(value.Value);

                var memberAccess = Expression.PropertyOrField(parameterExpression, key);

                var equalExpression = (value.IsNull.HasValue)
                    ? value.IsNull.Value ? Expression.Equal(memberAccess, nullConstantExpression)
                        : Expression.NotEqual(memberAccess, nullConstantExpression)
                    : Expression.Equal(memberAccess, constantExpression);

                if (combinedExpression == null)
                    combinedExpression = equalExpression;

                if (value.Condition == ExpressionCondition.And)
                    combinedExpression = Expression.And(combinedExpression, equalExpression);

                if (value.Condition == ExpressionCondition.Or)
                    combinedExpression = Expression.Or(combinedExpression, equalExpression);

                if (value.Condition == ExpressionCondition.Not)
                    combinedExpression = Expression.And(combinedExpression, Expression.Not(equalExpression));
            }

            return Expression.Lambda<Func<TEntity, bool>>(combinedExpression,  parameterExpression) ;
        }
    }
    
}
