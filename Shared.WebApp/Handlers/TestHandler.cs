using System.Threading.Tasks;
using Shared.Contracts;
using Shared.Domains;
using Shared.Services;

namespace Shared.WebApp.Handlers
{
    public class TestHandler : DefaultEventHandler<IEvent<Test>>
    {
        public override async Task<IEvent<Test>> Push(IEvent<Test> @event)
        {
            return @event;
        }
    }

    public class Test
    {

    }
}