using Catalog.Application.Contracts;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Domain.ValueObjects;
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

            var result = list.Select(c => new CategoryInfo(c.Id.Id, c.Name, 
                c.Image == null ? null : new Photo(c.Image.Name, c.Image.Url)))
                .ToList();

            return result;
        }

        public async Task<Guid> InsertOrUpdate(CategoryInfo categoryInfo)
        {
            Guid id = Guid.Empty;

            if (categoryInfo.CategoryId == null || categoryInfo.CategoryId == Guid.Empty)
            {
                // Insert logic
                var category = Category.CreateNew(categoryInfo.Name)
                    .SetImage(categoryInfo.Image?.Name, categoryInfo.Image?.Url);
                category.OnCategoryCreated();// Event will be raised via interceptor on save changes

                var result = await _categoryRepository.Insert(category); 

                id = result.Id;
            }
            else
            {
                // Update logic
                var category = Category.CreateWithId(categoryInfo.CategoryId.Value,
                    categoryInfo.Name
                    ).SetImage(categoryInfo.Image?.Name, categoryInfo.Image?.Url);

                category.OnCategoryUpdated();// Event will be raised via interceptor on save changes

                var result = await _categoryRepository.Update(category);

                id = result.Id;
            }

            return id;
        }
    }
}
