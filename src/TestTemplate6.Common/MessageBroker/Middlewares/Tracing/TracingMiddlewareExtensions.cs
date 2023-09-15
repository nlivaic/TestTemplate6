using MassTransit;

namespace TestTemplate6.Common.MessageBroker.Middlewares.Tracing
{
    public static class TracingMiddlewareExtensions
    {
        /// <summary>
        /// Start a new activity by reading MT-Activity-Id header off of read messages.
        /// If the header is not present, a new activity without a parent is started.
        /// </summary>
        /// <param name="configurator">.</param>
        public static void UseMessageBrokerTracing(this IPipeConfigurator<ConsumeContext> configurator) =>
            configurator.AddPipeSpecification(new TracingSpecification());
    }
}
