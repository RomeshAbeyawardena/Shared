using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
            where TEntity : class
    {
        public DefaultRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext DbContext { get; }
    }
}
