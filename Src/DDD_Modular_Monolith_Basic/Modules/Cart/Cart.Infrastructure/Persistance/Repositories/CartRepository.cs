using Cart.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Persistance.Repositories
{
    public sealed class CartRepository : ICartRepository
    {
        private readonly CartDBContext _dbContext;

        public CartRepository(CartDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Domain.Entities.Cart>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                .ToListAsync(cancellationToken);
        }

        public async Task<Domain.Entities.Cart?> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id.Id == id, cancellationToken);
        }

        public async Task Add(Domain.Entities.Cart cart, CancellationToken cancellationToken)
        {
            _dbContext.Carts.Add(cart);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(Domain.Entities.Cart cart, CancellationToken cancellationToken)
        {
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var cart = await GetById(id, cancellationToken);
            if (cart is not null)
            {
                _dbContext.Carts.Remove(cart);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
