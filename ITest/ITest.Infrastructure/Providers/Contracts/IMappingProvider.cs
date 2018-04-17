﻿using System.Collections.Generic;
using System.Linq;

namespace ITest.Infrastructure.Providers.Contracts
{
    public interface IMappingProvider
    {
        TDestination MapTo<TDestination>(object source);

        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable<object> source);

        IEnumerable<TDestination> EnumerableProjectTo<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
