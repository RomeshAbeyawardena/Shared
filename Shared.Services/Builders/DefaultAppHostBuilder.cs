using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.Contracts.Builders;
using System;

namespace Shared.Services.Builders
{
    internal class DefaultAppHostBuilder : IAppHostBuilder
    {
        public DefaultAppHostBuilder()
        {
            services = new ServiceCollection();
            configurationBuilder = new ConfigurationBuilder();
        }

        public IAppHost Build(IServiceCollection services = null)
        {
            if (StartupType == null)
                throw new NullReferenceException("Expected UseStartup<TStartup> method first, use Build<TStartup> instead");
            
            AppendServices(services);
            return new DefaultAppHost(StartupType, this.services.BuildServiceProvider());
        }

        public IAppHostBuilder UseStartup<TStartup>()
        {
            StartupType = typeof(TStartup);
            return this;
        }

        public IAppHost<TStartup> Build<TStartup>(IServiceCollection services = null, 
            Action<IServiceProvider> serviceProviderAction = null) where TStartup : class
        {
            UseStartup<TStartup>();
            AppendServices(services);
            var serviceProvider = this.services
                .AddSingleton<IConfiguration>(configurationBuilder.Build())
                .BuildServiceProvider();
            serviceProviderAction?.Invoke(serviceProvider);
            return new DefaultAppHost<TStartup>(serviceProvider);
        }

        private void AppendServices(IServiceCollection services)
        {
            if (services != null)
                foreach (var service in services)
                {
                    if (this.services.Contains(service))
                        continue;

                    this.services.Add(service);
                }

            this.services.AddSingleton(StartupType);
        }

        public IAppHostBuilder RegisterServices(Action<IServiceCollection> registerServices)
        {
            registerServices(services);
            return this;
        }

        public IAppHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configuration)
        {
            configuration(configurationBuilder);
            return this;
        }

        private readonly ConfigurationBuilder configurationBuilder;
        private Type StartupType;
        private readonly IServiceCollection services;
    }
}
