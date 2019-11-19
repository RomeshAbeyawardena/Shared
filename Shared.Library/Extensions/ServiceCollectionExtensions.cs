using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static object Resolve(this IServiceProvider serviceProvider, Type serviceType)
        {
            var constructorParameters = new List<object>();

            var publicConstructor = serviceType.GetConstructors().FirstOrDefault(c => c.IsPublic);
            foreach (var t in publicConstructor.GetParameters())
            {
                constructorParameters.Add(serviceProvider.GetService(t.ParameterType));
            }

            var resolvedService = Activator.CreateInstance(serviceType, constructorParameters.ToArray());
            return resolvedService;
        }

        public static TService Resolve<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.Resolve(typeof(TService));
        }

        public static TService GetRequiredService<TService>(this IServiceCollection services)
        {
            return services
                .BuildServiceProvider()
                .GetRequiredService<TService>();
        }

        public static IServiceCollection RegisterServiceBroker<TServiceBroker>(this IServiceCollection services)
            where TServiceBroker : IServiceBroker
        {
            var serviceBroker = Activator.CreateInstance<TServiceBroker>();
            serviceBroker.RegisterServiceAssemblies(services, serviceBroker.GetAssemblies);

            return services;
        }
    }
}
