using Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Persistance
{
    public sealed class CartDBContext : DbContext
    {
        public DbSet<Domain.Entities.Cart> Carts => Set<Domain.Entities.Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        public CartDBContext(DbContextOptions<CartDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
