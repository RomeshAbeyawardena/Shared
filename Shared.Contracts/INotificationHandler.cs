﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface INotificationHandler
    {
        INotificationUnsubscriber Subscribe(INotificationSubscriber notificationSubscriber);
        void Notify(object @event);
        Task NotifyAsync(object @event);
    }

    public interface INotificationHandler<TEvent> : INotificationHandler
    {
        INotificationUnsubscriber Subscribe(INotificationSubscriber<TEvent> notificationSubscriber);
        void Notify(TEvent @event);
        Task NotifyAsync(TEvent @event);
    }
}
