using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using System.Collections.Generic;

namespace Shared.Services
{
    public abstract class ControllerBase : Controller
    {
        protected IMediator Mediator => GetRequiredService<IMediator>();
        protected TService GetRequiredService<TService>() => HttpContext
            .RequestServices.GetRequiredService<TService>();

        protected TDestination Map<TSource, TDestination>(TSource sourceValue)
        {
            return MapperProvider.Map<TSource, TDestination>(sourceValue);
        }

        protected IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceValue)
        {
            return MapperProvider.Map<TSource, TDestination>(sourceValue);
        }

        private IMapperProvider MapperProvider => GetRequiredService<IMapperProvider>();
    }
}