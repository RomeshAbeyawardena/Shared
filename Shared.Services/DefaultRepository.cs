using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public async Task<TEntity> SaveChangesAsync(TEntity entity, bool saveChanges = true)
        {
            var entry = DbSet.Attach(entity);

            Console.WriteLine(entry.State);

            if(saveChanges)
                await dbContext.SaveChangesAsync();

            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> whereExpression = null, bool detachEntities = true)
        {
            var query = detachEntities 
                ? DbSet.AsNoTracking() 
                : DbSet;
               
            query.Where(whereExpression);

            return query;
        }
    }
}
