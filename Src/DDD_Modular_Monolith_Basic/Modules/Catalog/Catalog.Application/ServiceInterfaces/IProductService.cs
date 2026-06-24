using Catalog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.ServiceInterfaces
{
    public interface IProductService
    {
        Task<List<ProductInfo>> GetAllProductsAsync();
        Task<ProductInfo> GetProductByIdAsync(Guid productId);
        Task<Guid> InsertOrUpdateAsync(ProductInfo productInfo);
    }
}
