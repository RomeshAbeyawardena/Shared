using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Shared.Contracts.Builders
{
    public interface IAppHostBuilder
    {
        IAppHost<TStartup> Build<TStartup>(IServiceCollection services = null, Action<IServiceProvider> serviceProvider = null) where TStartup : class;
        IAppHostBuilder RegisterServices(Action<IServiceCollection> services);
        IAppHost Build(IServiceCollection services = null);
        IAppHostBuilder UseStartup<TStartup>();
        IAppHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configuration);
    }
}
