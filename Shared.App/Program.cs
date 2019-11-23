﻿using Microsoft.Extensions.Configuration;
using Shared.Contracts;
using Shared.Library.Extensions;
using Shared.Services;
using System;
using System.IO;
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
                    .RegisterServiceBroker<MyServiceBroker>();
            })
            .ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "app.json")))
            .Build<Startup>(serviceProvider: serviceProvider => serviceProvider.SubscribeToAllNotifications());
        }
}
