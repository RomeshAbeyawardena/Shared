﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface IMapperProvider
    {
        TDestination Map<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
    }
}