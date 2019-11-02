﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IRepository<TEntity> 
        where TEntity: class
    {
        DbSet<TEntity> DbSet {get;}
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> whereExpression = null, bool detachEntities = true);
        Task<TEntity> FindAsync(object key);
        Task<TEntity> FindAsync(params object[] keys);
        Task<int> SaveChangesAsync();
        Task<TEntity> SaveChangesAsync(TEntity entity, bool saveChanges = true);
        Task<int> Remove(TEntity entity, bool saveChanges = true);
        Task<int> RemoveAsync(bool saveChanges = true, params object[] keys);
    }
}
