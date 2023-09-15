using System.Collections.Generic;
using AutoMapper;
using TestTemplate6.Application.Sorting.Models;

namespace TestTemplate6.Application.Sorting
{
    public class SortCriteriaResolver<TSource, TTarget>
        : IValueResolver<TSource, TTarget, IEnumerable<SortCriteria>>
        where TSource : BaseSortable
        where TTarget : BaseSortable
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public SortCriteriaResolver(IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService;
        }

        public IEnumerable<SortCriteria> Resolve(
            TSource source,
            TTarget target,
            IEnumerable<SortCriteria> destMember,
            ResolutionContext context) =>
            _propertyMappingService.Resolve(source, target);
    }
}
