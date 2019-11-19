using System.Threading.Tasks;

namespace Shared.Contracts.Factories
{
    public interface IEventHandlerFactory
    {
        IEventHandler<TEvent> GetEventHandler<TEvent>() where TEvent : IEvent;
        Task Push<TEvent>(TEvent @event) where TEvent : IEvent;
        Task<TEvent> Send<TEvent, TCommand>(TCommand command) 
            where TEvent : IEvent
            where TCommand : ICommand;
    }
}
