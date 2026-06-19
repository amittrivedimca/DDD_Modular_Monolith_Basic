using Catalog.Domain.Entities;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        Task<CategoryId> Insert(Category category);
        Task<CategoryId> Update(Category category);
        Task<bool> Delete(Category category);
    }
}
