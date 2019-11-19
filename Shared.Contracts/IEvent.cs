using System;

namespace Shared.Contracts
{
    public interface IEvent
    {
        object Result { get;  }
        bool IsSuccessful { get; }
        Exception Exception { get; }
    }

    public interface IEvent<TResult> : IEvent
        where TResult : class
    {
        new TResult Result { get; }
    }
}
