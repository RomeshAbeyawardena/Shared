using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public sealed class DefaultNotificationHandler<TEvent> : INotificationHandler<TEvent>
    {
        public void Notify(TEvent @event)
        {
            foreach(var notificationSubscriber in _notificationSubscribersList)
            {
                notificationSubscriber.OnChange(@event);
            }
        }

        public void Notify(object @event)
        {
            Notify((TEvent)@event);
        }

        public INotificationUnsubscriber Subscribe(INotificationSubscriber<TEvent> notificationSubscriber)
        {
            return Subscribe((INotificationSubscriber)notificationSubscriber);
        }

        public INotificationUnsubscriber Subscribe(INotificationSubscriber notificationSubscriber)
        {
            if(!_notificationSubscribersList.Contains(notificationSubscriber))
                _notificationSubscribersList.Add(notificationSubscriber);

            return new DefaultNotificationUnsubscriber(_notificationSubscribersList, notificationSubscriber);
        }

        public DefaultNotificationHandler()
        {
            _notificationSubscribersList = new List<INotificationSubscriber>();
        }

        private readonly IList<INotificationSubscriber> _notificationSubscribersList;
    }
}
