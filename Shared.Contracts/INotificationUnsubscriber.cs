using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface INotificationUnsubscriber : IDisposable
    {
        void Unsubscribe();
    }
}
