using Microsoft.Extensions.DependencyInjection;

namespace Shared.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}
