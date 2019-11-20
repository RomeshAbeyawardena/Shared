using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultNotificationHandlerFactory : INotificationHandlerFactory
    {
        public void Notify<TEvent>(TEvent @event)
        {
            foreach(var notificationSubscriber in _notificationSubscribersList)
            {
                notificationSubscriber.OnChange(@event);
            }
        }

        public INotificationUnsubscriber<TEvent> Subscribe<TEvent>(INotificationSubscriber<TEvent> notificationSubscriber)
        {
            if(!_notificationSubscribersList.Contains(notificationSubscriber))
                _notificationSubscribersList.Add(notificationSubscriber);

            return new DefaultNotificationUnsubscriber<TEvent>(_notificationSubscribersList, notificationSubscriber);
        }

        public DefaultNotificationHandlerFactory(IList<INotificationSubscriber> notificationSubscribersList)
        {
            _notificationSubscribersList = notificationSubscribersList;
        }

        private readonly IList<INotificationSubscriber> _notificationSubscribersList;
    }
}
