using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestTemplate6.Application.Pipelines;

namespace TestTemplate6.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTestTemplate6ApplicationHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
            services.AddPipelines();

            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        }
    }
}
