using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public abstract class DefaultNotificationSubscriber<TEvent> : INotificationSubscriber<TEvent>
    {
        public abstract void OnChange(TEvent @event);

        public void OnChange(object @event)
        {
            OnChange((TEvent)@event);
        }
    }
}
