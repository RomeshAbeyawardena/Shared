using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Library.Extensions;
using System;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class DefaultEventHandlerFactory : IEventHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public IEventHandler<TEvent> GetEventHandler<TEvent>()
            where TEvent : IEvent
        {
            return (IEventHandler<TEvent>) serviceProvider.GetService(typeof(IEventHandler<TEvent>));
        }

        public async Task<TEvent> Push<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            var eventHandler = GetEventHandler<TEvent>();
            return await eventHandler.Push(@event);
        }

        public async Task<TEvent> Send<TEvent, TCommand>(TCommand command)
            where TEvent : IEvent
            where TCommand : ICommand
        {
            var eventHandler = GetEventHandler<TEvent>();
            return await eventHandler.Send(command);
        }

        public DefaultEventHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
