using Microsoft.EntityFrameworkCore;
using Microsoft.IO;
using Shared.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shared.App
{
    public class Startup
    {
        private readonly IRepositoryFactory repositoryFactory;

        public async Task Start()
        {
            var testRepository = repositoryFactory.GetRepository<TestDbContext, Property>();
        }

        public Startup(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Property> Properties {get;set;}
    }

    public class Property
    {
        [Key]
        public int Id {get;set;}
    }
}
