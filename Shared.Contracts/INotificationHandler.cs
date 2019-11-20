using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface INotificationHandler
    {
        INotificationUnsubscriber Subscribe(INotificationSubscriber notificationSubscriber);
        void Notify(object @event);
    }

    public interface INotificationHandler<TEvent> : INotificationHandler
    {
        INotificationUnsubscriber Subscribe(INotificationSubscriber<TEvent> notificationSubscriber);
        void Notify(TEvent @event);
    }
}
