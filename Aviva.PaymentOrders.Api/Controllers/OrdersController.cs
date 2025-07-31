using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aviva.PaymentOrders.Application.Services;
using Aviva.PaymentOrders.Application.Adapters;
using System.ComponentModel.DataAnnotations;

namespace Aviva.PaymentOrders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                // Logic to retrieve orders
                var orders = await _orderService.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                    return NotFound("No orders found.");
                // Return the list of orders
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            // Validate the id
            if (id <= 0)
                return BadRequest("Invalid order ID.");
            try
            {
                // Logic to retrieve a specific order by id
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound();
                return Ok(order);
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

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO order)
        {
            if (order == null)
                return BadRequest("Order data is null.");
            try
            {
                // Logic to create a new order
                var orderCreated = await _orderService.AddOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = orderCreated.Id }, orderCreated);
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

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO order)
        {
            try
            {
                // Logic to update a specific order by id
                if (id != order.Id)
                    return BadRequest("Order ID mismatch.");
                // var existingOrder = await _orderService.GetOrderByIdAsync(id);
                // if (existingOrder == null)
                //     return NotFound();
                await _orderService.UpdateOrderAsync(order);
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

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Validate the id
            if (id <= 0)
                return BadRequest("Invalid order ID.");
            try
            {
                // Logic to delete a specific order by id
                await _orderService.DeleteOrderAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
            // If the order was successfully deleted, return NoContent
            return NoContent();
        }

        // PUT: api/orders/cancel/5
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            // Logic to cancel a specific order by id
            if (id <= 0)
                return BadRequest("Invalid order ID.");
            try
            {
                var order = await _orderService.CancelOrderAsync(id);
                return Ok(order);
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

        // PUT: api/orders/pay/5
        [HttpPut("pay/{id}")]
        public async Task<IActionResult> PayOrder(int id)
        {
            // Logic to pay a specific order by id
            if (id <= 0)
                return BadRequest("Invalid order ID.");
            try
            {
                var order = await _orderService.PayOrderAsync(id);
                return Ok(order);
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
    }
}
