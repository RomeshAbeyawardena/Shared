using Shared.Contracts;
using Shared.Contracts.Factories;
using System;
using System.Threading.Tasks;

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

        public async Task NotifyAsync<TEvent>(TEvent @event)
        {
            await GetNotificationHandler<TEvent>().NotifyAsync(@event);
        }

        public DefaultNotificationHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
    }
}
