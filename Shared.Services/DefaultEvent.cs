using System;
using Shared.Contracts;

namespace Shared.Domains
{
    public static class DefaultEvent
    {
        public static IEvent<T> Create<T>(T result)
            where T : class
        {
            return new DefaultEvent<T>(result);
        }

        public static IEvent<T> Create<T>(bool isSuccessful, Exception exception = null, T result = null)
            where T : class
        {
            return new DefaultEvent<T>(isSuccessful, exception, result);
        }
    }

    public class DefaultEvent<T> : IEvent<T>
        where T : class
    {
        public DefaultEvent(T result)
        {
            Result = result;
            IsSuccessful = true;
        }

        public DefaultEvent(bool isSuccessful, Exception exception = null, T result = null)
        {
            Result = result;
            IsSuccessful = isSuccessful;
            Exception = exception;
        }

        public T Result { get; }
        public bool IsSuccessful { get; }
        public Exception Exception { get; }

        object IEvent.Result { get => Result; }
    }
}
