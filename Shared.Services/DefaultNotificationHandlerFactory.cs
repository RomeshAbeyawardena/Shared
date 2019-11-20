using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultNotificationHandlerFactory : INotificationHandlerFactory
    {
        public void Notify<TEvent>(TEvent @event)
        {
            GetNotificationHandler<TEvent>().Notify(@event);
        }

        public INotificationUnsubscriber Subscribe<TEvent>(INotificationSubscriber<TEvent> notificationSubscriber)
        {
            return GetNotificationHandler<TEvent>().Subscribe(notificationSubscriber);
        }

        private INotificationHandler<TEvent> GetNotificationHandler<TEvent>()
        {
            var notificationHandlerType = typeof(INotificationHandler<>)
                .MakeGenericType(typeof(TEvent));

            return _serviceProvider.GetService(notificationHandlerType) as INotificationHandler<TEvent>;
        }

        public DefaultNotificationHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
    }
}
