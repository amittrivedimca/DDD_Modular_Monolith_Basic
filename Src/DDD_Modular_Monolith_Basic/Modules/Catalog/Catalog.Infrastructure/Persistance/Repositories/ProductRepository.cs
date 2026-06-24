using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistance.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly CatalogDBContext _dbContext;

        public ProductRepository(CatalogDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _dbContext.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetById(Guid productId)
        {
            return await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id.Id == productId);
        }

        public async Task<bool> Delete(Guid productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        

        public async Task<ProductId> Insert(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task<ProductId> Update(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }
    }
}
