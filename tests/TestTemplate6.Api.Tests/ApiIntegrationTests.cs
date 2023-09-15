using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TestTemplate6.Api.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace TestTemplate6.Api.Tests
{
    public class ApiIntegrationTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly ApiWebApplicationFactory _factory;

        public ApiIntegrationTests(
            ITestOutputHelper testOutput,
            ApiWebApplicationFactory factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/");
            _testOutput = testOutput;
            _factory = factory;
        }

        [Fact]
        public async Task Api_CreateNewFoo_SuccessfullyWith201()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var ctx = _factory.CreateDbContext(_testOutput);
            var initialCount = ctx.Foos.Count();

            // Act
            var response = await client.PostAsJsonAsync("foos", new
            {
                Text = "My_Test_Title"
            });
            using var ctx1 = _factory.CreateDbContext(_testOutput);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(initialCount + 1, ctx1.Foos.Count());
        }

        [Fact]
        public async Task Api_TwoFoosWithSameText_FailWith422()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            using var ctx = _factory.CreateDbContext(_testOutput);
            var response = await client.PostAsJsonAsync("foos", new
            {
                Text = "Text 1"
            });

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }
    }
}
