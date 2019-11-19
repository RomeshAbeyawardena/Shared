using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public abstract class DefaultEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract Task<TEvent> Push(TEvent @event);

        public abstract Task<TEvent> Send<TCommand>(TCommand command) where TCommand : ICommand;

        public async Task<IEvent> Push(IEvent @event)
        {
            return await Push((TEvent)@event);
        }

        async Task<IEvent> IEventHandler.Send<TCommand>(TCommand command)
        {
            return await Send(command);
        }
    }
}
