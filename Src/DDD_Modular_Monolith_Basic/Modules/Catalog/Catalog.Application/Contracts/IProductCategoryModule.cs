using Catalog.Application.DTOs;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Contracts
{
    public interface IProductCategoryModule
    {
        Task<List<CategoryInfo>> GetAllCategoriesAsync();
        Task<Guid> InsertOrUpdate(CategoryInfo categoryInfo);
    }

    
}
