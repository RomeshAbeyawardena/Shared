using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using Shared.Contracts;
using Shared.Library;
using Shared.Services;
using System;
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
                .AddDbContext<TestDbContext>()
                .AddSingleton<RecyclableMemoryStreamManager>()
                    .AddSingleton<IRepositoryFactory, RepositoryFactory>()
                    .AddSingleton<IBinarySerializer, BinarySerializer>()
                    .AddSingleton<IMessagePackBinarySerializer, MessagePackBinarySerializer>()
                    .AddSingleton<IOptions<DefaultCloneOptions>>(new Options<DefaultCloneOptions>(opt =>
                    {
                        opt.DefaultCloneType = Domains.CloneType.Deep;
                        opt.UseMessagePack = true;
                    }))
                    .AddSingleton(typeof(ICloner<>), typeof(DefaultCloner<>));
            })
            .Build<Startup>();
    }
}
