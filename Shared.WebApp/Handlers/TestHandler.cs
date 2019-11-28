using System.Threading.Tasks;
using Shared.Contracts;
using Shared.Services;

namespace Shared.WebApp.Handlers
{
    public class TestHandler : DefaultEventHandler<IEvent<Test>>
    {
        public override Task<IEvent<Test>> Push(IEvent<Test> @event)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Test
    {

    }
}