using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.Services.Extensions;
using Shared.WebApp.Handlers;
using System.Threading.Tasks;

namespace Shared.WebApp.Controllers
{
    [Route("{controller}/{action}")]
    public class TestController : Controller
    {
        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Test()
        {
            await _mediator.Push(new Test());
            return Ok();
        }

        private readonly IMediator _mediator;
    }
}