﻿using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IMediator
    {
        void Notify<TEvent>(TEvent @event);
        Task NotifyAsync<TEvent>(TEvent @event);
        Task<TEvent> Push<TEvent>(TEvent @event)
            where TEvent : IEvent;
        Task<TEvent> Send<TEvent, TCommand>(TCommand command)
            where TEvent : IEvent
            where TCommand : ICommand;
    }
}
