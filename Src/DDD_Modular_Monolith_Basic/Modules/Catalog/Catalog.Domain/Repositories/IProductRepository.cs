using Catalog.Domain.Entities;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(Guid productId);
        Task<ProductId> Insert(Product product);
        Task<ProductId> Update(Product product);
        Task<bool> Delete(Guid productId);
    }
}
