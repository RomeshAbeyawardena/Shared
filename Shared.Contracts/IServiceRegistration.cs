using Microsoft.Extensions.DependencyInjection;
using System;

namespace Shared.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services);
    }
}
