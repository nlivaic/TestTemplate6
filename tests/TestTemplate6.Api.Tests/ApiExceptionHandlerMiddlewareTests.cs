using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using TestTemplate6.Api.Middlewares;
using TestTemplate6.Common.Exceptions;
using Xunit;

namespace TestTemplate6.Api.Tests
{
    public class ApiExceptionHandlerMiddlewareTests
    {
        [Fact]
        public async Task Api_ApiExceptionHandlerMiddleware_CatchesNotFoundExceptionAsync_AsProblemDetails404()
        {
            // Arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseApiExceptionHandler(options =>
                            {
                            });
                            app.Run(ctx => throw new EntityNotFoundException("Test Entity", default(Guid)));
                        });
                })
                .StartAsync();
            // Act
            var response = await host.GetTestClient().GetAsync("/");
            var responseBody = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsType<ProblemDetails>(responseBody);
        }

        [Fact]
        public async Task Api_ApiExceptionHandlerMiddleware_CatchesBusinessExceptionAsync_AsProblemDetails422()
        {
            // Arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseApiExceptionHandler(options =>
                            {
                            });
                            app.Run(ctx => throw new BusinessException("Test Message"));
                        });
                })
                .StartAsync();
            // Act
            var response = await host.GetTestClient().GetAsync("/");
            var responseBody = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.IsType<ProblemDetails>(responseBody);
        }

        [Fact]
        public async Task Api_ApiExceptionHandlerMiddleware_CatchesOtherExceptionsAsync_AsProblemDetails500()
        {
            // Arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseApiExceptionHandler(options =>
                            {
                            });
                            app.Run(ctx => throw new Exception("Test Message"));
                        });
                })
                .StartAsync();
            // Act
            var response = await host.GetTestClient().GetAsync("/");
            var responseBody = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsType<ProblemDetails>(responseBody);
        }
    }
}
