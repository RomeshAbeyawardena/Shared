using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.WebApp.Handlers;
using System.Threading.Tasks;
using Shared.Domains.Enumerations;
using Shared.Services.Exceptions;

namespace Shared.WebApp.Controllers
{
    [Route("{controller}/{action}")]
    public class TestController : Controller
    {
        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Test([FromQuery]Test test)
        {
            throw new ModelStateException(modelState => modelState.Add("cat", "invalid value"));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Push(new Test()).ConfigureAwait(false);

            await _mediator.NotifyAsync(DefaultEntityChangedEvent.Create(test, entityEventType: EntityEventType.Added)).ConfigureAwait(false);
            return Ok();
        }

        private readonly IMediator _mediator;
    }
}