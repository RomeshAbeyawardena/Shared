using Shared.Contracts;
using System;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

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

                var eventHandlerTypes = assemblyTypes.Where(type => type.GetInterfaces().Any(a => a.IsAssignableFrom(typeof(IEventHandler))));

                RegisterEventHandlerTypes(services, eventHandlerTypes);

                foreach (var item in serviceRegistrationTypes)
                {
                   var serviceRegistration = Activator.CreateInstance(item) as IServiceRegistration;
                    serviceRegistration.RegisterServices(services);
                }
            }
        }

        private void RegisterEventHandlerTypes(IServiceCollection services, IEnumerable<Type> eventHandlerTypes)
        {
            foreach(var eventHandlerType in eventHandlerTypes)
            {
                if(eventHandlerType.IsAbstract)
                    continue;
                
                var genericServiceType = typeof(IEventHandler<>);
                
                var genericArguments = eventHandlerType.GetInterfaces().FirstOrDefault().GetGenericArguments();

                services.AddSingleton(genericServiceType.MakeGenericType(genericArguments), eventHandlerType);
            }
        }
    }
}
