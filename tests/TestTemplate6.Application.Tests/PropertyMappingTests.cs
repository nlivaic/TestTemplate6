using System.Collections.Generic;
using System.Linq;
using TestTemplate6.Application.Sorting;
using TestTemplate6.Application.Sorting.Models;
using TestTemplate6.Application.Tests.Helpers;
using Xunit;

namespace TestTemplate6.Application.Tests
{
    public class PropertyMappingTests
    {
        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithOneSimpleMapping_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria> { new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty1) } }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters1()).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty1)}", result[0].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[0].SortDirection);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithOneSimpleMappingReverted_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(true, nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria> { new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty1) } }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters1()).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty1)}", result[0].SortByCriteria);
            Assert.Equal(SortDirection.Desc, result[0].SortDirection);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithSeveralSimpleMappingsAndOneSpecified_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1.TargetProperty2)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria> { new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty1) } }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters1()).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty1)}", result[0].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[0].SortDirection);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithSeveralSimpleMappings_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1.TargetProperty2)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria>
                {
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty1) },
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty2) }
                }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters1()).ToList();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty1)}", result[0].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty2)}", result[1].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[0].SortDirection);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithSeveralComplexMappings_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}", $"{nameof(MappingTargetModel1.TargetProperty2)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1.TargetProperty3)}", $"{nameof(MappingTargetModel1.TargetProperty4)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria>
                {
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty1) },
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty2) }
                }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters1()).ToList();

            // Assert - first mapping
            Assert.Equal(4, result.Count());
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty1)}", result[0].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty2)}", result[1].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[1].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty3)}", result[2].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[2].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty4)}", result[3].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result[3].SortDirection);
        }

        [Fact]
        public void PropertyMappingService_TwoSourceAndTargetModelsWithSeveralComplexMappings_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}", $"{nameof(MappingTargetModel1.TargetProperty2)}")
                        .Add(true, nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1.TargetProperty3)}", $"{nameof(MappingTargetModel1.TargetProperty4)}"),
                    new PropertyMapping<MappingSourceModel2, MappingTargetModel2>()
                        .Add(nameof(MappingSourceModel2.SourceProperty1), $"{nameof(MappingTargetModel2.TargetProperty1)}", $"{nameof(MappingTargetModel2.TargetProperty2)}")
                        .Add(true, nameof(MappingSourceModel2.SourceProperty2), $"{nameof(MappingTargetModel2.TargetProperty3)}", $"{nameof(MappingTargetModel2.TargetProperty4)}")
                }
            };
            var resourceParameters1 = new ResourceParameters1
            {
                SortBy = new List<SortCriteria>
                {
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty1) },
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty2) }
                }
            };
            var resourceParameters2 = new ResourceParameters2
            {
                SortBy = new List<SortCriteria>
                {
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel2.SourceProperty1) },
                    new SortCriteria { SortByCriteria = nameof(MappingSourceModel2.SourceProperty2) }
                }
            };
            var target = new PropertyMappingService(options);

            // Act - first mapping
            var result1 = target.Resolve(resourceParameters1, new TargetParameters1()).ToList();
            // Act - second mapping
            var result2 = target.Resolve(resourceParameters2, new TargetParameters2()).ToList();

            // Assert - first mapping
            Assert.Equal(4, result1.Count());
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty1)}", result1[0].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result1[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty2)}", result1[1].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result1[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty3)}", result1[2].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result1[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel1.TargetProperty4)}", result1[3].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result1[0].SortDirection);

            // Assert - second mapping
            Assert.Equal(4, result2.Count());
            Assert.Equal($"{nameof(MappingTargetModel2.TargetProperty1)}", result2[0].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result2[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel2.TargetProperty2)}", result2[1].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result2[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel2.TargetProperty3)}", result2[2].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result2[0].SortDirection);
            Assert.Equal($"{nameof(MappingTargetModel2.TargetProperty4)}", result2[3].SortByCriteria);
            Assert.Equal(SortDirection.Asc, result2[0].SortDirection);
        }

        [Fact]
        public void PropertyMappingService_UnknownPropertyMapping_GetsSkipped()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1.TargetProperty2)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria> { new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty3) } }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters1()).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void PropertyMappingService_UnknownSourceAndTargetTypeMapping_GetsSkipped()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1.TargetProperty1)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1.TargetProperty2)}")
                }
            };
            var resourceParameters = new ResourceParameters1
            {
                SortBy = new List<SortCriteria> { new SortCriteria { SortByCriteria = nameof(MappingSourceModel1.SourceProperty3) } }
            };
            var target = new PropertyMappingService(options);

            // Act
            var result = target.Resolve(resourceParameters, new TargetParameters2()).ToList();

            // Assert
            Assert.Empty(result);
        }
    }
}
