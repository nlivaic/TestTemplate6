using System.Collections.Generic;
using System.Linq;
using TestTemplate6.Common.Paging;
using Xunit;

namespace TestTemplate6.Common.Tests
{
    public class PagedListTests
    {
        [Fact]
        public void PagedListTests_EmptyList()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = new PagedList<int>(list, 1, 0, 5);

            // Assert
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(0, result.TotalItems);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.False(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(1, result.Paging.CurrentPage);
            Assert.Equal(1, result.Paging.TotalPages);
            Assert.Equal(0, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_SingleElementInList()
        {
            // Arrange
            var list = new List<int> { 1 };

            // Act
            var result = new PagedList<int>(list, 1, 1, 5);

            // Assert
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(1, result.TotalItems);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.False(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(1, result.Paging.CurrentPage);
            Assert.Equal(1, result.Paging.TotalPages);
            Assert.Equal(1, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_LessElementsThanOneFullPage()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4 };

            // Act
            var result = new PagedList<int>(list, 1, 4, 5);

            // Assert
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(4, result.TotalItems);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.False(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(1, result.Paging.CurrentPage);
            Assert.Equal(1, result.Paging.TotalPages);
            Assert.Equal(4, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_EnoughElementsForOneFullPage()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = new PagedList<int>(list, 1, 5, 5);

            // Assert
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(5, result.TotalItems);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.False(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(1, result.Paging.CurrentPage);
            Assert.Equal(1, result.Paging.TotalPages);
            Assert.Equal(5, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_FirstPage()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = new PagedList<int>(list, 1, 13, 5);

            // Assert
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(13, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.True(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(1, result.Paging.CurrentPage);
            Assert.Equal(3, result.Paging.TotalPages);
            Assert.Equal(13, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_MiddlePage()
        {
            // Arrange
            var list = new List<int> { 6, 7, 8, 9, 10 };

            // Act
            var result = new PagedList<int>(list, 2, 13, 5);

            // Assert
            Assert.Equal(2, result.CurrentPage);
            Assert.Equal(13, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.True(result.HasNextPage);
            Assert.True(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(2, result.Paging.CurrentPage);
            Assert.Equal(3, result.Paging.TotalPages);
            Assert.Equal(13, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_LastPage()
        {
            // Arrange
            var list = new List<int> { 11, 12, 13, 14, 15 };

            // Act
            var result = new PagedList<int>(list, 3, 13, 5);

            // Assert
            Assert.Equal(3, result.CurrentPage);
            Assert.Equal(13, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.False(result.HasNextPage);
            Assert.True(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(3, result.Paging.CurrentPage);
            Assert.Equal(3, result.Paging.TotalPages);
            Assert.Equal(13, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_PageNumberZero_ReturnsPageNumberAsEntered()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = new PagedList<int>(list, 0, 13, 5);

            // Assert
            Assert.Equal(0, result.CurrentPage);
            Assert.Equal(13, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.True(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(0, result.Paging.CurrentPage);
            Assert.Equal(3, result.Paging.TotalPages);
            Assert.Equal(13, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }

        [Fact]
        public void PagedListTests_PageOutOfScope_ReturnsPageNumberAsEntered()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = new PagedList<int>(list, 10, 13, 5);

            // Assert
            Assert.Equal(10, result.CurrentPage);
            Assert.Equal(13, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.PageSize);
            Assert.False(result.HasNextPage);
            Assert.True(result.HasPreviousPage);
            Assert.Equal(list, result.Items);
            Assert.Equal(10, result.Paging.CurrentPage);
            Assert.Equal(3, result.Paging.TotalPages);
            Assert.Equal(13, result.Paging.TotaItems);
            Assert.Equal(5, result.Paging.CurrentPageSize);
        }
    }
}
