using Catalog.Application.DTOs;
using Catalog.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MM.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
                
        [HttpGet(Name = "GetAllProducts")]
        public async Task<IEnumerable<ProductInfo>> GetAllProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpGet("{id}", Name = "GetProductById")]        
        public async Task<ActionResult<ProductInfo>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost(Name = "InsertOrUpdateProduct")]
        public async Task<ActionResult> InsertOrUpdateProduct([FromBody] ProductInfo productInfo)
        {
            var result = await _productService.InsertOrUpdateAsync(productInfo);
            return Ok(new { Id = result, Success = true });
        }
    }
}
