using Shared.Contracts;
using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultEntityChangedEvent
    {
        public static IEntityChangedEvent<T> Create<T>(T result, IEnumerable<T> results = null, EntityEventType entityEventType = EntityEventType.None)
            where T: class
        {
            return new DefaultEntityChangedEvent<T>(result, results, entityEventType);
        }
    }

    public class DefaultEntityChangedEvent<T> : DefaultEvent<T>, IEntityChangedEvent<T>
        where T: class
    {
        public DefaultEntityChangedEvent(T result, IEnumerable<T> results = null, EntityEventType entityEventType = EntityEventType.None)
            : base(result, results)
        {
            EventType = entityEventType;
        }

        public EntityEventType EventType { get; }
    }
}
