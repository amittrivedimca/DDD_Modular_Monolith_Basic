using Catalog.Application.DTOs;
using Catalog.Application.ServiceInterfaces;
using Catalog.Domain.Repositories;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductInfo>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAll();
            List<ProductInfo> result = products.Select(p => new ProductInfo( 
                p.Id.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Image != null ? new Photo(p.Image.Name, p.Image.Url) : null,
                p.CategoryId,
                p.Category != null ? p.Category.Name : string.Empty
                ))
                .ToList();

            return result;
        }

        public async Task<ProductInfo> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null)
            {
                return null;
            }

            return new ProductInfo(
                product.Id.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Image != null ? new Photo(product.Image.Name, product.Image.Url) : null,
                product.CategoryId,
                product.Category != null ? product.Category.Name : string.Empty
            );
        }

        public Task<Guid> InsertOrUpdateAsync(ProductInfo productInfo)
        {
            throw new NotImplementedException();
        }
    }
}
