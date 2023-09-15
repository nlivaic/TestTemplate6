using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestTemplate6.Common.MessageBroker.Middlewares.ErrorLogging
{
    public class ExceptionLoggerFilter<T> : IFilter<T>
        where T : class, PipeContext
    {
        private readonly ILogger<ExceptionLoggerFilter<T>> _logger;

        public ExceptionLoggerFilter(IServiceCollection serviceCollection)
        {
            _logger = serviceCollection.BuildServiceProvider().GetRequiredService<ILogger<ExceptionLoggerFilter<T>>>();
        }

        public void Probe(ProbeContext context)
        {
            // var scope = context.CreateFilterScope("exceptionLogger");
        }

        /// <summary>
        /// Log exceptions thrown from consumers. Handles any consumer.
        /// </summary>
        /// <param name="context">The context sent through the pipeline.</param>
        /// <param name="next">The next filter in the pipe, must be called or the pipe ends here.</param>
        public async Task Send(T context, IPipe<T> next)
        {
            try
            {
                // here the next filter in the pipe is called
                await next.Send(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                // propagate the exception up the call stack
                throw;
            }
        }
    }
}
