using Microsoft.Extensions.DependencyInjection;
using TestTemplate6.Common.Interfaces;
using TestTemplate6.Core.Interfaces;
using TestTemplate6.Data.Repositories;

namespace TestTemplate6.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSpecificRepositories(this IServiceCollection services) =>
            services.AddScoped<IFooRepository, FooRepository>();

        public static void AddGenericRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
