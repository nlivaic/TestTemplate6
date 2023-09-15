using System.Collections.Generic;
using TestTemplate6.Application.Sorting.Models;

namespace TestTemplate6.Application.Tests.Helpers
{
    public class ResourceParameters1
        : BaseSortable<MappingSourceModel1>
    {
        public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
