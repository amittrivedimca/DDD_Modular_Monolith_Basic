using Catalog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Contracts
{
    public interface IProductCategoryModule
    {
        Task<List<CategoryInfo>> GetAllCategoriesAsync();
    }

    
}
