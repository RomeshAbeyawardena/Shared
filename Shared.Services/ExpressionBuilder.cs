﻿using Shared.Contracts;
using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shared.Services
{
    public sealed class ExpressionBuilder : IExpressionBuilder
    {
        private readonly IDictionary<string, ExpressionParameter> expressionParameterDictionary;

        private IExpressionBuilder Add(string key, ExpressionCondition expressionCondition, ExpressionComparer? expressionComparer, object value = null)
        {
            expressionParameterDictionary.Add(key, new ExpressionParameter
            {
                Name = key,
                Value = value,
                Condition = expressionCondition,
                ExpressionComparer = expressionComparer ?? ExpressionComparer.Equal
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

        public IExpressionBuilder And(string name, ExpressionComparer? expressionComparer  = null, object value = null)
        {
            return Add(name, ExpressionCondition.And, expressionComparer, value);
        }

        public IExpressionBuilder Or(string name, ExpressionComparer? expressionComparer = null, object value = null)
        {
            return Add(name, ExpressionCondition.Or,  expressionComparer, value);
        }

        public IExpressionBuilder Not(string name, ExpressionComparer? expressionComparer = null, object value = null)
        {
            return Add(name, ExpressionCondition.Not, expressionComparer, value);
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

                BinaryExpression equalExpression = null;
                switch (value.ExpressionComparer)
                {
                    case ExpressionComparer.IsNull:
                        equalExpression = Expression.Equal(memberAccess, nullConstantExpression);
                        break;
                    default:
                    case ExpressionComparer.Equal:
                        equalExpression = Expression.Equal(memberAccess, constantExpression);
                        break;
                    case ExpressionComparer.GreaterThan:
                        equalExpression = Expression.GreaterThan(memberAccess, constantExpression);
                        break;
                    case ExpressionComparer.GreaterThanOrEqual:
                        equalExpression = Expression.GreaterThanOrEqual(memberAccess, constantExpression);
                        break;
                    case ExpressionComparer.LessThan:
                        equalExpression = Expression.LessThan(memberAccess, constantExpression);
                        break;
                    case ExpressionComparer.LessThanOrEqual:
                        equalExpression = Expression.LessThanOrEqual(memberAccess, constantExpression);
                        break;
                }
                
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
