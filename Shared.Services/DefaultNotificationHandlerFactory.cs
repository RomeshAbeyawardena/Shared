using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultNotificationHandlerFactory : INotificationHandlerFactory
    {
        public void Notify<TEvent>(INotificationHandler<TEvent> @event)
        {
            throw new NotImplementedException();
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
