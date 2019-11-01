using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider serviceProvider;

        public IRepository<TEntity> GetRepository<TDbContext, TEntity>()
            where TDbContext : DbContext
            where TEntity : class
        {
            return serviceProvider.Resolve<DefaultRepository<TDbContext, TEntity>>();
        }

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
