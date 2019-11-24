using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Providers;
using System;

namespace Shared.Services
{
    public abstract class ExtendedDbContext : DbContext
    {
        private readonly bool useSingularTableNames;

        public ExtendedDbContext(bool useSingularTableNames = true)
        {
            this.useSingularTableNames = useSingularTableNames;
        }

        public ExtendedDbContext(DbContextOptions dbContextOptions, bool useSingularTableNames = true)
            : base(dbContextOptions)
        {
            this.useSingularTableNames = useSingularTableNames;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (useSingularTableNames)
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                    entityType.SetTableName(entityType.GetTableName().Singularize());
        }
    }
}
