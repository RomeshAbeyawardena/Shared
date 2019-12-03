using System.Threading.Tasks;
using Shared.Contracts;
using Shared.Services;
using Shared.Services.Attributes;

namespace Shared.WebApp.Handlers
{
    public class TestHandler : DefaultEventHandler<IEvent<Test>>
    {
        public override async Task<IEvent<Test>> Push(IEvent<Test> @event)
        {
            return @event;
        }
    }

    [OptionalRequired(nameof(Samuel), nameof(Tom))]
    [OptionalRequired(nameof(Harry))]
    public class Test
    {
        public string Samuel { get; set; }
        public string Tom { get; set; }
        public string Harry { get; set; }
    }
}