using Shared.Contracts;
using System;

namespace Shared.Library.Exceptions
{
    public class EventException : Exception
    {
        public EventException(IEvent @event, string message, Exception innerException = null)
            : base($"Event exception occured: {message}", innerException)
        {
            Event = @event;
        }

        public IEvent Event { get; }
    }
}
