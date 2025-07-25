using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aviva.PaymentOrders.Application.Services;
using Aviva.PaymentOrders.Application.Adapters;

namespace Aviva.PaymentOrders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public Task<IActionResult> GetProducts()
        {
            // Logic to retrieve products
            return Task.FromResult<IActionResult>(Ok(_productService.GetAllProductsAsync()));
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            // Logic to retrieve a specific product by id
            var product = _productService.GetProductByIdAsync(id).Result;
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDTO product)
        {
            // Logic to create a new product
            _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = 1 }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] object product)
        {
            // Logic to update a specific product by id
            return NoContent();
        }
        
        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // Logic to delete a specific product by id
            return NoContent();
        }
    }
}
