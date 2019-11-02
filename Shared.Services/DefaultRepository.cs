using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class DefaultRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
            where TEntity : class
    {
        private readonly TDbContext dbContext;

        public DefaultRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbSet<TEntity> DbSet => dbContext.Set<TEntity>();

        public async Task<TEntity> FindAsync(object key)
        {
            return await DbSet.FindAsync(key);
        }

        public async Task<TEntity> FindAsync(params object[] key)
        {
            return await DbSet.FindAsync(key);
        }

        public async Task<int> Remove(TEntity entity, bool saveChanges = true)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbContext.Remove(entity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(bool saveChanges = true, params object[] keys)
        {
            var entity = await FindAsync(keys);
            return await Remove(entity);
        }

        public async Task<TEntity> SaveChangesAsync(TEntity entity, bool saveChanges = true)
        {
            var model = dbContext.Model.GetEntityTypes().SingleOrDefault(entityType => entityType.ClrType == typeof(TEntity));
            
            foreach (var key in model.GetKeys())
            {
                var keyPropertyInfo = key.Properties.SingleOrDefault().PropertyInfo;

                var nullObject = keyPropertyInfo.PropertyType.IsValueType 
                    ? Activator.CreateInstance(keyPropertyInfo.PropertyType) 
                    : null;
                
                var keyValue = keyPropertyInfo.GetValue(entity);
                if(keyValue.Equals(nullObject))
                    DbSet.Add(entity);
                else
                    DbSet.Update(entity);
            }
            if(saveChanges)
                await SaveChangesAsync();

            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> whereExpression = null, bool detachEntities = true)
        {
            var query = detachEntities 
                ? DbSet.AsNoTracking() 
                : DbSet;
            
            if(whereExpression == null)
                return query;
            
            return query.Where(whereExpression);
        }
    }
}
