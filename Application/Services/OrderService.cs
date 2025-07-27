using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using Aviva.PaymentOrders.DataInfrastructure.Repositories;
using Aviva.PaymentOrders.Application.Adapters;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aviva.PaymentOrders.Application.Services
{

    // OrderService is a service class that provides methods to interact with Order entities.
    public class OrderService
    {
        private readonly OrdersRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(OrdersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GetAllOrdersAsync retrieves all Orders and maps them to a list of OrderDTO.
        // It returns an IEnumerable of OrderDTO.
        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        // GetOrderByIdAsync retrieves a Order by its ID and maps it to a OrderDTO.
        // If the Order is not found, it returns null.
        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        // AddOrderAsync maps the OrderDTO to a Order entity and adds it to the repository.
        public async Task AddOrderAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<PaymentOrder>(orderDTO);
            // Process the order in external api before adding it to the repository
            // Select the payment provider based on the order details
            order.ProviderRef = "Provider"; // This should be set based on the actual provider logic
            order.OrderIdProvider = 12345; // This should be set based on the actual
            await _repository.AddAsync(order);
        }

        // UpdateOrderAsync updates an existing Order in the repository.
        public async Task UpdateOrderAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<PaymentOrder>(orderDTO);
            // Update the existing order in the repository
            await _repository.UpdateAsync(order);
        }

        // DeleteOrderAsync deletes a Order by its ID from the repository.
        // It does not return any value.
        public async Task DeleteOrderAsync(int id) => await _repository.DeleteAsync(id);
        
        public async Task CancelOrderAsync(int id) => await _repository.CancelOrderAsync(id);  // Logic to cancel an order by id
    }
}