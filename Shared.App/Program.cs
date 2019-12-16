using Microsoft.Extensions.Configuration;
using DotNetInsights.Shared.Contracts;
using DotNetInsights.Shared.Library.Extensions;
using DotNetInsights.Shared.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace DotNetInsights.Shared.App
{
    public static class Program
    {
        public static async Task Main()
        {
            await AppHostBuilder.RunAsync(startup => startup.Start()).ConfigureAwait(false);
        }

        public static IAppHost<Startup> AppHostBuilder => AppHost
            .CreateBuilder()
            .RegisterServices(services =>
            {
                services
                    .RegisterServiceBroker<MyServiceBroker>();
            })
            .ConfigureLogging(logBuilder => logBuilder.AddConsole())
            .ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "app.json")))
            .Build<Startup>();
        }
}
