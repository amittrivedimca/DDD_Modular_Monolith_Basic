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
        CategoryId Add(Category category); 
        CategoryId Update(Category category);
        bool Delete(Category category);
    }
}
