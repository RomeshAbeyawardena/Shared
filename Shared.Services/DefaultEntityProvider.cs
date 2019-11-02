using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultEntityProvider<TEntity> : IDefaultEntityProvider<TEntity>
        where TEntity : class
    {
        public Action<TEntity> GetDefaultAssignAction(EntityState entityState)
        {
            if(defaultEntitySwitch.ContainsKey(entityState))
                return defaultEntitySwitch.Case(entityState);

            return entity => {};
        }

        public static IDefaultEntityProvider<TEntity> Create()
        {
            return new DefaultEntityProvider<TEntity>();
        }

        private DefaultEntityProvider()        
        {
            defaultEntitySwitch = DefaultSwitch.Create<EntityState, Action<TEntity>>();
        }

        public IDefaultEntityProvider<TEntity> AddDefaults(EntityState entityState, Action<TEntity> action)
        {
            defaultEntitySwitch.CaseWhen(entityState, action);
            return this; 
        }

        private readonly ISwitch<EntityState, Action<TEntity>> defaultEntitySwitch;
    }
}
