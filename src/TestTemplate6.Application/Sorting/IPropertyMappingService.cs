using System.Collections.Generic;
using TestTemplate6.Application.Sorting.Models;

namespace TestTemplate6.Application.Sorting
{
    public interface IPropertyMappingService
    {
        IEnumerable<SortCriteria> Resolve(BaseSortable sortableSource, BaseSortable sortableTarget);
    }
}
