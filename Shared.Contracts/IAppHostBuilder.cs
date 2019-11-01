using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IAppHostBuilder
    {
        IAppHost<TStartup> Build<TStartup>(IServiceCollection services = null) where TStartup : class;
        IAppHostBuilder RegisterServices(Action<IServiceCollection> services);
        IAppHost Build(IServiceCollection services = null);
        IAppHostBuilder UseStartup<TStartup>();
    }
}
