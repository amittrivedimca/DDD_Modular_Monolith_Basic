using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Catalog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
