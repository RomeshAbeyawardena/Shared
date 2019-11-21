using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Factories
{
    public interface INotificationHandlerFactory
    {
        INotificationUnsubscriber Subscribe<TEvent>(INotificationSubscriber<TEvent> notificationSubscriber);
        void Notify<TEvent>(TEvent @event);
        Task NotifyAsync<TEvent>(TEvent @event);
    }
}
