using System;
using Microsoft.Extensions.DependencyInjection;

namespace TestTemplate6.Application.Sorting
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPropertyMappingService(
            this IServiceCollection services,
            Action<PropertyMappingOptions> configuration)
        {
            var options = new PropertyMappingOptions();
            configuration?.Invoke(options);
            services.AddSingleton<IPropertyMappingService>(_ => new PropertyMappingService(options));
        }
    }
}
