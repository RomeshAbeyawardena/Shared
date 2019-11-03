using AutoMapper;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class MapperProvider : IMapperProvider
    {
        private readonly IMapper mapper;

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }

        public MapperProvider(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
