using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface INotificationUnsubscriber<TEvent> : IDisposable
    {
        void Unsubscribe();
    }
}
