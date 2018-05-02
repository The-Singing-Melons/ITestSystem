using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITest.Infrastructure.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITest.Infrastructure.Providers
{
    public class MappingProvider : IMappingProvider
    {
        private IMapper mapper;

        public MappingProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public TDestination MapTo<TDestination>(object source)
        {
            return this.mapper.Map<TDestination>(source);
        }

        public TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination)
        {
            return this.mapper.Map<TSource, TDestination>(source, destination);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable<object> source)
        {
            return source.ProjectTo<TDestination>();
        }

        public IEnumerable<TDestination> EnumerableProjectTo<TSource, TDestination>(IEnumerable<TSource> source)
        {
            // AsQuryable cast to avoid query materialization errors
            return source.AsQueryable().ProjectTo<TDestination>();
        }
    }
}
