using Shared.Domains.Enumerations;

namespace Shared.Contracts
{
    public interface IEntityChangedEvent<TEntity> : IEvent<TEntity>
        where TEntity : class
    {
        EntityEventType EventType { get; }
    }
}
