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
            this.mapper = mapper;
        }

        public TDestination MapTo<TDestination>(object source)
        {
            return this.mapper.Map<TDestination>(source);
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
