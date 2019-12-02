using System;
using System.Threading.Tasks;

namespace Shared.Contracts.Factories
{
    public interface INotificationHandlerFactory : IDisposable
    {
        INotificationUnsubscriber Subscribe<TEvent>(INotificationSubscriber<TEvent> notificationSubscriber);
        void Notify<TEvent>(TEvent @event);
        Task NotifyAsync<TEvent>(TEvent @event);
    }
}
