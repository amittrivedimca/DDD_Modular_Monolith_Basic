using Catalog.Application.DTOs;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.ServiceInterfaces
{
    public interface IProductCategoryService
    {
        Task<List<CategoryInfo>> GetAllCategoriesAsync();
        Task<Guid> InsertOrUpdate(CategoryInfo categoryInfo);
    }

    
}
