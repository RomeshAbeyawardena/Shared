﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shared.Contracts.Factories
{
    public interface IQueryBuilderFactory
    {
        Expression<Func<TEntity, bool>> GetExpression<TEntity, TEnum>(TEnum enumeration)
            where TEntity : class
            where TEnum : struct;
    }
}
