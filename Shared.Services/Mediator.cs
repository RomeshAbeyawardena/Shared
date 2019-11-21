using Shared.Contracts;
using Shared.Contracts.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class Mediator : IMediator
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly INotificationHandlerFactory _notificationHandlerFactory;

        public void Notify<TEvent>(TEvent @event)
        {
            _notificationHandlerFactory.Notify(@event);
        }

        public async Task NotifyAsync<TEvent>(TEvent @event)
        {
            await _notificationHandlerFactory.NotifyAsync(@event);
        }

        public async Task<TEvent> Push<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            return await _eventHandlerFactory.Push(@event);
        }

        public async Task<TEvent> Send<TEvent, TCommand>(TCommand command) 
            where TEvent : IEvent
            where TCommand : ICommand
        {
            return await _eventHandlerFactory.Send<TEvent,TCommand>(command);
        }

        public Mediator(IEventHandlerFactory eventHandlerFactory, INotificationHandlerFactory notificationHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _notificationHandlerFactory = notificationHandlerFactory;
        }
    }
}
