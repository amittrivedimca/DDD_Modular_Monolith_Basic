using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Persistance.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {

        private readonly CatalogDBContext _dbContext;

        public CategoryRepository(CatalogDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CategoryId Add(Category category)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAll()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public CategoryId Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
