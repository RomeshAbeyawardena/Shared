using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface INotificationHandlerFactory
    {
        INotificationUnsubscriber<TEvent> Subscribe<TEvent>(INotificationSubscriber<TEvent> notificationSubscriber);
        void Notify<TEvent>(INotificationHandler<TEvent> @event);
    }
}
