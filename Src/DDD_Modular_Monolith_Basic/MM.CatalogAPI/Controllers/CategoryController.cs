using Catalog.Application.ServiceInterfaces;
using Catalog.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MM.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductCategoryService _prodCategoryService;

        public CategoryController(IProductCategoryService prodCategoryService)
        {
            _prodCategoryService = prodCategoryService;
        }

        [HttpGet(Name = "GetAllCategory")]
        public async Task<IEnumerable<CategoryInfo>> GetAllCategory()
        {
            return await _prodCategoryService.GetAllCategoriesAsync();
        }

        [HttpPost(Name = "InsertOrUpdateCategory")]
        public async Task<ActionResult> InsertOrUpdateCategory([FromBody] CategoryInfo categoryInfo)
        {
            var result = await _prodCategoryService.InsertOrUpdate(categoryInfo);
            return Ok(new { Id = result, Success = true });
        }
    }
}
