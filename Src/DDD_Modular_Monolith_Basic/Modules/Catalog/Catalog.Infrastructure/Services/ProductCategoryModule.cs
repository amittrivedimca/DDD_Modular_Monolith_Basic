using Catalog.Application.Contracts;
using Catalog.Application.DTOs;
using Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Services
{
    public sealed class ProductCategoryModule : IProductCategoryModule
    {
        
        private readonly ICategoryRepository _categoryRepository;

        public ProductCategoryModule(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryInfo>> GetAllCategoriesAsync()
        {
            var list = await _categoryRepository.GetAll();

            var result = list.Select(c => new CategoryInfo(c.Id.Id, c.Name))
                .ToList();

            return result;
        }
    }
}
