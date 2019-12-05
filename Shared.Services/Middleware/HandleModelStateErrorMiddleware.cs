using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Middleware
{
    public class HandleModelStateErrorFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is ModelStateException modelStateException)
            {
                context.ExceptionHandled = true;
                foreach (var modelState in modelStateException.ModelStateBuilder)
                    context.ModelState.AddModelError(modelState.Key, modelState.Value);

                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
