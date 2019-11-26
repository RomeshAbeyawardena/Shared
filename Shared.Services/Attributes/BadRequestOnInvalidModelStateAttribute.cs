﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Shared.Services.Attributes
{
    public sealed class BadRequestOnInvalidModelStateAttribute : Attribute, IActionFilter, IAsyncActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
                context.Result = BadRequestActionResult(context.ModelState);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.ModelState.IsValid){
                await next();
                return;
            }

            context.Result = BadRequestActionResult(context.ModelState);
        }

        private IActionResult BadRequestActionResult(ModelStateDictionary modelStateDictionary)
        {
            var badRequestActionResult = new BadRequestObjectResult(modelStateDictionary);

            return badRequestActionResult;
        }
    }
}
