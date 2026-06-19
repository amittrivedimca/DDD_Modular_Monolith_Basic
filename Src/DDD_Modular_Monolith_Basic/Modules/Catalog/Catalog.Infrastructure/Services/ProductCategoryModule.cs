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
                var result = await _categoryRepository.Insert(new Category(CategoryId.CreateUnique())
                {
                    Name = categoryInfo.Name,
                    Image = categoryInfo.Image == null ? null : new Photo(categoryInfo.Image.Name, categoryInfo.Image.Url)
                });

                id = result.Id;
            }
            else
            {
                // Update logic
                var result = await _categoryRepository.Update(new Category(CategoryId.Create(categoryInfo.CategoryId))
                {
                    Name = categoryInfo.Name,
                    Image = categoryInfo.Image == null ? null : new Photo(categoryInfo.Image.Name, categoryInfo.Image.Url)
                });

                id = result.Id;
            }

            return id;
        }
    }
}
