using Shared.Contracts;
using System;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shared.Library.Extensions;

namespace Shared.Services
{
    public abstract class DefaultServiceBroker : IServiceBroker
    {
        public abstract Assembly[] GetAssemblies {get;}
        public static Assembly DefaultAssembly => Assembly.GetAssembly(typeof(DefaultAppHost));
        
        public void RegisterServiceAssemblies(IServiceCollection services, params Assembly[] assemblies)
        {
            foreach(var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                var serviceRegistrationTypes = assemblyTypes
                    .Where(type => type.GetInterface(nameof(IServiceRegistration)) != null);

                foreach (var item in serviceRegistrationTypes)
                {
                   var serviceRegistration = Activator.CreateInstance(item) as IServiceRegistration;
                    serviceRegistration.RegisterServices(services);
                }
            }
        }
    }
}
