using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using Shared.Contracts;
using Shared.Library;
using Shared.Library.Extensions;
using Shared.Services;
using System.Threading.Tasks;

namespace Shared.App
{
    public static class Program
    {
        public static async Task Main()
        {
            await AppHostBuilder.RunAsync(startup => startup.Start());
        }

        public static IAppHost<Startup> AppHostBuilder => AppHost
            .CreateBuilder()
            .RegisterServices(services =>
            {
                services
                .RegisterServiceBroker<MyServiceBroker>()
                .AddDbContext<TestDbContext>(options => options.UseSqlServer("Server=localhost;Database=PaymentsCalculator;Trusted_Connection=true;MultipleActiveResultSets=true"));
            })
            .Build<Startup>();
    }
}
