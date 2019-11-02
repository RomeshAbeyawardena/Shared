using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace Shared.App
{
    public class Startup
    {
        private readonly IRepositoryFactory repositoryFactory;

        public async Task Start()
        {
            var testRepository = repositoryFactory.GetRepository<TestDbContext, Account>();

            //await testRepository.SaveChangesAsync(new Account
            //{
                
            //    Reference = "SDC346787",
            //    ShortName = "SDC",
            //    Name = "Samuel Duncan",
            //    Registered = DateTimeOffset.Now,
            //    Created = DateTimeOffset.Now
            //});

            await testRepository.RemoveAsync(true, 1);
        }

        public Startup(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }
    }

    public class MyServiceBroker : DefaultServiceBroker
    {
        public override Assembly[] GetAssemblies => new [] { Assembly.GetExecutingAssembly() };
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Account> Accounts {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                entityType.SetTableName(entityType.GetTableName().Singularize());
        }
    }

    public class Account
    {
        [Key]
        public int Id {get;set;}
        public string Reference { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Registered { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
