using Shared.Contracts;
using Shared.Contracts.Builders;
using Shared.Domains;
using System;
using System.Threading.Tasks;

namespace Shared.Services.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task NotifyAsync<TEntity>(this IMediator mediator, TEntity @event)
            where TEntity: class
        {
            await mediator.NotifyAsync(DefaultEvent.Create(@event));
        }
        public static async Task<IEvent<TEntity>> Push<TEntity>(this IMediator mediator, TEntity entity)
            where TEntity: class
        {
            return await mediator
                .Push(DefaultEvent.Create(entity)); ;
        }
        public static async Task<IEvent<TEntity>> Send<TEntity>(this IMediator mediator, string commandName, Action<IDictionaryBuilder<string, object>> dictionaryBuilderAction)
            where TEntity: class
        {
            return await mediator
                .Send<IEvent<TEntity>>(DefaultCommand
                    .Create<TEntity>(commandName, dictionaryBuilderAction));
        }
    }
}
