using Catalog.Application.Contracts;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Persistance;
using Catalog.Infrastructure.Persistance.Interceptors;
using Catalog.Infrastructure.Persistance.Repositories;
using Catalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register interceptors
            services.AddScoped<DomainEventDispatcherInterceptor>();

            var connectionString = configuration.GetConnectionString("CatalogDatabase")
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CatalogDBContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<DomainEventDispatcherInterceptor>();

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "catalog");
                });

                options.AddInterceptors(interceptor);
            });
           
            //services.AddScoped<IInventoryUnitOfWork>(sp => sp.GetRequiredService<InventoryDbContext>());
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Application Services
            services.AddScoped<IProductCategoryModule, ProductCategoryModule>();

            return services;
        }
    }
}
