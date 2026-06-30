using Cart.Domain.Repositories;
using Cart.Infrastructure.Persistance;
using Cart.Infrastructure.Persistance.Interceptors;
using Cart.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cart.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register interceptors
            services.AddScoped<DomainEventDispatcherInterceptor>();

            var connectionString = configuration.GetConnectionString("CartDatabase")
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CartDBContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<DomainEventDispatcherInterceptor>();

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "cart");
                });

                options.AddInterceptors(interceptor);
            });

            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }
    }
}
