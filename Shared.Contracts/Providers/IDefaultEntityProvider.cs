﻿using Microsoft.EntityFrameworkCore;
using System;

namespace Shared.Contracts
{
    public interface IDefaultEntityProvider<TEntity>
        where TEntity : class
    {
        IDefaultEntityProvider<TEntity> AddDefaults(EntityState entityState, Action<TEntity> action);
        Action<TEntity> GetDefaultAssignAction(EntityState entityState);
    }
}
