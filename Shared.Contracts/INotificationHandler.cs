using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface INotificationHandler<TEvent>
    {
        void Notify(TEvent @event);
    }
}
