using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using System;
using System.Threading.Tasks;

namespace Shared.Services
{
    internal class DefaultAppHostBuilder : IAppHostBuilder
    {
        public DefaultAppHostBuilder()
        {
            services = new ServiceCollection();
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

        public IAppHost<TStartup> Build<TStartup>(IServiceCollection services = null) where TStartup : class
        {
            UseStartup<TStartup>();
            AppendServices(services);
            return new DefaultAppHost<TStartup>(this.services.BuildServiceProvider());
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


        private Type StartupType;
        private readonly IServiceCollection services;
    }
}
