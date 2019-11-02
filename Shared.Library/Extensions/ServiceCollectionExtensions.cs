using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TService Resolve<TService>(this IServiceProvider serviceProvider)
        {
            var constructorParameters = new List<object>();
            var serviceType =typeof(TService);
            var publicConstructor = serviceType.GetConstructors().FirstOrDefault(c => c.IsPublic);
            foreach (var t in publicConstructor.GetParameters())
            {
                constructorParameters.Add(serviceProvider.GetService(t.ParameterType));
            }

            var resolvedService = Activator.CreateInstance(serviceType, constructorParameters.ToArray());
            return (TService)resolvedService;
        }

        public static IServiceCollection RegisterServiceBroker<TServiceBroker>(this IServiceCollection services)
            where TServiceBroker : IServiceBroker
        {
            var serviceBroker = Activator.CreateInstance<TServiceBroker>();
            serviceBroker.RegisterServiceAssemblies(serviceBroker.GetAssemblies);

            return this;
        }
    }
}
