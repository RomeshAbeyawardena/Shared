using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface IEntityChangedEvent<TEntity> : IEvent<TEntity>
        where TEntity : class
    {
        EntityEventType EventType { get; }
    }
}
