using System;
using System.Collections.Generic;
using System.Linq;
using TestTemplate6.Application.Sorting.Models;

namespace TestTemplate6.Application.Sorting
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IEnumerable<IPropertyMapping> _propertyMappings;

        public PropertyMappingService(PropertyMappingOptions propertyMappingOptions)
        {
            _propertyMappings = propertyMappingOptions.PropertyMappings ?? new List<IPropertyMapping>();
        }

        /// <summary>
        /// Map source and destination properties. Returns a list of destination properties for each source property.
        /// One caveat: you cannot sort on destination properties that have additional aggregation methods applied to them
        /// in the AutoMapper profile (e.g. `Question.Answers.Count()` mapping to `QuestionSummariesGetViewModel.Answers`,
        /// because the resulting Linq query will cause EF Core to break.
        /// </summary>
        /// <param name="source">Resource parameters, with zero or more properties to source from.</param>
        /// <param name="target">Target (query) parameters.</param>
        /// <returns>A list of properties to execute the ordering on.</returns>
        public IEnumerable<SortCriteria> Resolve(BaseSortable source, BaseSortable target)
        {
            var sortCriterias = new List<SortCriteria>();
            foreach (var s in source.SortBy)
            {
                PropertyMappingValue targetMapping = null;
                try
                {
                    targetMapping = GetMapping(source.ResourceType, s.SortByCriteria, target.ResourceType);
                }
                catch (InvalidPropertyMappingException)
                {
                    // Skip erroneous mapping and move on to next sort criteria.
                    continue;
                }
                sortCriterias.AddRange(
                    targetMapping.TargetPropertyNames.Select(tpn =>
                        new SortCriteria
                        {
                            SortByCriteria = tpn,
                            SortDirection = targetMapping.Revert
                                ? s.SortDirection == SortDirection.Desc
                                    ? SortDirection.Asc
                                    : SortDirection.Desc
                                : s.SortDirection == SortDirection.Asc
                                    ? SortDirection.Asc
                                    : SortDirection.Desc
                        }));
            }
            return sortCriterias;
        }

        private PropertyMappingValue GetMapping(Type source, string sourcePropertyName, Type target)
        {
            var propertyMapping = _propertyMappings
                .SingleOrDefault(pm => pm.Source == source && pm.Target == target)
                ?? throw new InvalidPropertyMappingException($"Unknown property mapping types: {source}, {target}.");
            return propertyMapping.GetMapping(sourcePropertyName);
        }
    }
}
