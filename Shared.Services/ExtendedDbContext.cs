using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Services
{
    public abstract class ExtendedDbContext : DbContext
    {
        private readonly bool useSingularTableNames;

        private IDefaultEntityProvider<TEntity> GetDefaultEntityProvider<TEntity>() where TEntity : class
        {
            return this.GetService<IDefaultEntityProvider<TEntity>>();
        }
        public ExtendedDbContext(bool useSingularTableNames = true)
        {
            this.useSingularTableNames = useSingularTableNames;
        }

        public ExtendedDbContext(DbContextOptions dbContextOptions, bool useSingularTableNames = true)
            : base(dbContextOptions)
        {
            this.useSingularTableNames = useSingularTableNames;
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            var defaultEntityService = GetDefaultEntityProvider<TEntity>();

            if (defaultEntityService == null)
                return base.Add(entity);

            var entry = base.Add(entity);
            defaultEntityService?
                .GetDefaultAssignAction(entry.State)?
                .Invoke(entity);

            return entry;
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            var defaultEntityService = GetDefaultEntityProvider<TEntity>();

            if (defaultEntityService == null)
                return base.Update(entity);

            var entry = base.Update(entity);
            defaultEntityService?
                .GetDefaultAssignAction(entry.State)?
                .Invoke(entity);

            return entry;
        }

        public override EntityEntry Update(object entity)
        {
            return base.Update(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (useSingularTableNames)
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                    entityType.SetTableName(entityType.GetTableName().Singularize());
        }
    }
}
