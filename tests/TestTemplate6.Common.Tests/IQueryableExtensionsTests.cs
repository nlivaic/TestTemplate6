using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestTemplate6.Common.Base;
using TestTemplate6.Common.Extensions;
using Xunit;

namespace TestTemplate6.Common.Tests
{
    public class IQueryableExtensionsTests
    {
        [Fact]
        public void IQueryableExtensionsTests_CanProjectEntity_Successfully()
        {
            // Arrange
            var target = new List<TestEntity>
            {
                new TestEntity()
            }.AsQueryable();

            // Act
            var result = target.Projector<TestEntity>(null).First();

            // Assert
            Assert.IsType<TestEntity>(result);
        }

        [Fact]
        public void IQueryableExtensionsTests_CanProjectViewModel_Successfully()
        {
            // Arrange
            var target = new List<TestEntity>
            {
                new TestEntity()
            }.AsQueryable();
            var configurationProvider = new MapperConfiguration(
                cfg => cfg.CreateMap<TestEntity, TestEntityDto>());

            // Act
            var result = target.Projector<TestEntityDto>(configurationProvider).First();

            // Assert
            Assert.IsType<TestEntityDto>(result);
        }

        private class TestEntity : BaseEntity<Guid>
        {
        }

        private class TestEntityDto
        {
            public Guid Id { get; set; }
        }
    }
}
