using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Contracts.Providers;
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

        public static IServiceProvider SubscribeToAllNotifications(this IServiceProvider serviceProvider)
        {
            var subscriberEventTypeList = serviceProvider.GetRequiredService<IList<Type>>();
            var subscriberNotificationHandlerFactory = serviceProvider.GetRequiredService<INotificationHandlerFactory>();
            var notificationUnsubscribers = serviceProvider.GetRequiredService<IList<INotificationUnsubscriber>>();
            foreach (var subscriberEventType in subscriberEventTypeList)
            {
                var subscriberEvent = serviceProvider.GetRequiredService(subscriberEventType);
                var genericArgs = subscriberEventType.GetGenericArguments();

                var factoryType = subscriberNotificationHandlerFactory.GetType();

                var unsubscriber = factoryType.GetMethod("Subscribe")
                    .MakeGenericMethod(genericArgs)
                    .Invoke(subscriberNotificationHandlerFactory, new object [] {
                        subscriberEvent
                });

                notificationUnsubscribers.Add((INotificationUnsubscriber)unsubscriber);
            }

            return serviceProvider;
        }

        public static TService Resolve<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.Resolve(typeof(TService));
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
