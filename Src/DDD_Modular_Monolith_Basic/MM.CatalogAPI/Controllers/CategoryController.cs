using Catalog.Application.Contracts;
using Catalog.Application.DTOs;
using Catalog.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MM.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductCategoryModule _prodCategoryModule;

        public CategoryController(IProductCategoryModule prodCategoryModule)
        {
            _prodCategoryModule = prodCategoryModule;
        }

        [HttpGet(Name = "GetAllCategory")]
        public async Task<IEnumerable<CategoryInfo>> GetAllCategory()
        {
            return await _prodCategoryModule.GetAllCategoriesAsync();
        }
    }
}
