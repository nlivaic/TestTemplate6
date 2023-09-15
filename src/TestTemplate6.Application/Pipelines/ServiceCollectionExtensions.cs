using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TestTemplate6.Application.Pipelines
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register MediatR pipelines (middleware).
        /// Keeping the registrations in the specified order is important.
        /// </summary>
        /// <param name="services">.</param>
        public static void AddPipelines(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipeline<,>));
        }
    }
}
