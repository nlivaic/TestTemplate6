using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TestTemplate6.Application.Pipelines;
using TestTemplate6.Application.Tests.Helpers;
using Xunit;

namespace TestTemplate6.Application.Tests
{
    public class LoggingPipelineTests
    {
        [Fact]
        public async Task LoggingPipeline_CallsNextThenSaves_Successfully()
        {
            // Arrange
            var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Response>>();
            var fakeLogger = new FakeLogger<Request>();
            var target = new LoggingPipeline<Request, Response>(fakeLogger);

            // Act
            var result = await target.Handle(new Request(), default(CancellationToken), requestHandlerDelegateMock.Object);

            // Assert
            Assert.Equal(2, fakeLogger.LogEntriesInformation.Count);
            Assert.Equal($"Starting execution of {typeof(Request)}.", fakeLogger.LogEntriesInformation[0]);
            Assert.Equal($"Finished executing {typeof(Request)}.", fakeLogger.LogEntriesInformation[1]);
            requestHandlerDelegateMock.Verify(m => m(), Times.Once);
        }

        [Fact]
        public async Task LoggingPipeline_ReturnsResponse_Successfully()
        {
            // Arrange
            var fakeLogger = new FakeLogger<Request>();
            var response = new Response("Test Response");
            RequestHandlerDelegate<Response> requestHandlerDelegate = () => Task.FromResult(response);
            var target = new LoggingPipeline<Request, Response>(fakeLogger);

            // Act
            var result = await target.Handle(new Request(), default(CancellationToken), requestHandlerDelegate);

            // Assert
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task LoggingPipeline_OnException_DoesNothing()
        {
            // Arrange
            var fakeLogger = new FakeLogger<Request>();
            RequestHandlerDelegate<Response> requestHandlerDelegate = () => throw new Exception("");
            var target = new LoggingPipeline<Request, Response>(fakeLogger);

            // Act, Assert
            await Assert.ThrowsAsync<Exception>(
                () => target.Handle(new Request(), default(CancellationToken), requestHandlerDelegate));
        }
    }
}
