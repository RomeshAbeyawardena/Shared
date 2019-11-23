using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Contracts.Providers;
using System;

namespace Shared.Services.Providers
{
    public static class DefaultEntityProvider
    {
        public static IDefaultEntityProvider<TEntity> Create<TEntity>()
            where TEntity : class
        {
            return DefaultEntityProvider<TEntity>.Create();
        }
    }

    public class DefaultEntityProvider<TEntity> : IDefaultEntityProvider<TEntity>
        where TEntity : class
    {
        public Action<IServiceProvider, TEntity> GetDefaultAssignAction(EntityState entityState)
        {
            if(defaultEntitySwitch.ContainsKey(entityState))
                return defaultEntitySwitch.Case(entityState);

            return (serviceProvider, entity) => {};
        }

        public static IDefaultEntityProvider<TEntity> Create()
        {
            return new DefaultEntityProvider<TEntity>();
        }

        private DefaultEntityProvider()
        {
            defaultEntitySwitch = DefaultSwitch.Create<EntityState, Action<IServiceProvider, TEntity>>();
        }

        public IDefaultEntityProvider<TEntity> AddDefaults(EntityState entityState, Action<IServiceProvider, TEntity> action)
        {
            defaultEntitySwitch.CaseWhen(entityState, action);
            return this; 
        }

        private readonly ISwitch<EntityState, Action<IServiceProvider, TEntity>> defaultEntitySwitch;
    }
}
