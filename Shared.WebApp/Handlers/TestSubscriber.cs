using System.Threading.Tasks;
using DotNetInsights.Shared.Contracts;
using DotNetInsights.Shared.Services;

namespace DotNetInsights.Shared.WebApp.Handlers
{
    public class TestSubscriber : DefaultNotificationSubscriber<IEvent<Test>>
    {
        public override void OnChange(IEvent<Test> @event)
        {
            throw new System.NotImplementedException();
        }

        public override Task OnChangeAsync(IEvent<Test> @event)
        {
            throw new System.NotImplementedException();
        }
    }
}