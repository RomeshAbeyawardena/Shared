using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Library.Extensions;
using System;

namespace Shared.Services
{
    public class DefaultRepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider serviceProvider;

        public IRepository<TEntity> GetRepository<TDbContext, TEntity>()
            where TDbContext : DbContext
            where TEntity : class
        {
            return serviceProvider.Resolve<DefaultRepository<TDbContext, TEntity>>();
        }

        public DefaultRepositoryFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
