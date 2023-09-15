using System.Collections.Generic;
using TestTemplate6.Application.Sorting.Models;

namespace TestTemplate6.Application.Tests.Helpers
{
    public class ResourceParameters2
        : BaseSortable<MappingSourceModel2>
    {
        public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
