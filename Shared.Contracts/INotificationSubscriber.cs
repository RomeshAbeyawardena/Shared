using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface INotificationSubscriber
    {
        void OnChange(object @event);
    }

    public interface INotificationSubscriber<TEvent> : INotificationSubscriber
    {
        void OnChange(TEvent @event);
    }
}
