﻿using DotNetInsights.Shared.Contracts;
using System.Threading.Tasks;

namespace DotNetInsights.Shared.Services
{
    public abstract class DefaultNotificationSubscriber<TEvent> : INotificationSubscriber<TEvent>
    {
        public abstract void OnChange(TEvent @event);

        public void OnChange(object @event)
        {
            OnChange((TEvent)@event);
        }

        public abstract Task OnChangeAsync(TEvent @event);

        public async Task OnChangeAsync(object @event)
        {
            await OnChangeAsync((TEvent)@event).ConfigureAwait(false);
        }
    }
}
