using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aviva.PaymentOrders.Application.Services;
using Aviva.PaymentOrders.Application.Adapters;


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
            // Logic to retrieve orders
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders == null || !orders.Any())
                return NotFound("No orders found.");
            // Return the list of orders
            return Ok(orders);
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            // Logic to retrieve a specific order by id
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO order)
        {
            // Logic to create a new order
            await _orderService.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderDTO order)
        {
            // Logic to update a specific order by id
            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Logic to delete a specific order by id
            await _orderService.DeleteOrderAsync(id);
            // If the order was successfully deleted, return NoContent
            if (id <= 0)
                return BadRequest("Invalid order ID.");
            return NoContent();
        }

        // POST: api/orders/cancel/5
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            // Logic to cancel a specific order by id
            if (id <= 0)
                return BadRequest("Invalid order ID.");
            await _orderService.CancelOrderAsync(id);
            return NoContent();
        }
    }
}
