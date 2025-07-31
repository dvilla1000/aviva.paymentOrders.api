using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aviva.PaymentOrders.Application.Services;
using Aviva.PaymentOrders.Application.Adapters;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                // Logic to retrieve products
                var products = await _productService.GetAllProductsAsync();
                if (products == null || !products.Any())
                    return NotFound("No products found.");
                // Return the list of products
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            // Validate the id
            if (id <= 0)
                return BadRequest("Invalid product ID.");
            try
            {
                // Logic to retrieve a specific product by id
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
        {
            if (product == null)
                return BadRequest("Product data is null.");
            try
            {
                // Logic to create a new product
                var productCreated = await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = productCreated.Id }, productCreated);
            }
            catch(ValidationException ex)
            {
                // Return a BadRequest with validation errors
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO product)
        {
            
            try
            {
                // Logic to update a specific product by id
                if (id != product.Id)
                    return BadRequest("Order ID mismatch.");
                await _productService.UpdateProductAsync(product);
            }
            catch(ValidationException ex)
            {
                // Return a BadRequest with validation errors
                return BadRequest($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Validate the id
            if (id <= 0)
                return BadRequest("Invalid product ID.");
            try
            {
                // Logic to delete a specific product by id
                await _productService.DeleteProductAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
            // If the product was successfully deleted, return NoContent
            return NoContent();
        }
    }
}
