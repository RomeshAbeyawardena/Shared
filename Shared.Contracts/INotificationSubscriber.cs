using System.Threading.Tasks;

namespace DotNetInsights.Shared.Contracts
{
    public interface INotificationSubscriber
    {
        void OnChange(object @event);
        Task OnChangeAsync(object @event);
    }

    public interface INotificationSubscriber<TEvent> : INotificationSubscriber
    {
        void OnChange(TEvent @event);
        Task OnChangeAsync(TEvent @event);
    }
}
