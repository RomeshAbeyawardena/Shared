using System;

namespace Shared.Contracts
{
    public interface INotificationUnsubscriber : IDisposable
    {
        void Unsubscribe();
    }
}
