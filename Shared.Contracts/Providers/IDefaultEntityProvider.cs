using Microsoft.EntityFrameworkCore;
using System;

namespace Shared.Contracts.Providers
{
    public interface IDefaultEntityProvider<TEntity>
        where TEntity : class
    {
        IDefaultEntityProvider<TEntity> AddDefaults(EntityState entityState, Action<IServiceProvider, TEntity> action);
        Action<IServiceProvider, TEntity> GetDefaultAssignAction(EntityState entityState);
    }
}
