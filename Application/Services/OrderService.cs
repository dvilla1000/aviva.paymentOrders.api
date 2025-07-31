using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using Aviva.PaymentOrders.DataInfrastructure.Repositories;
using Aviva.PaymentOrders.Application.Adapters;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders;
using System.Text.Json;

namespace Aviva.PaymentOrders.Application.Services
{

    // OrderService is a service class that provides methods to interact with Order entities.
    public class OrderService
    {
        private readonly OrdersRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPaymentProviderFactory _paymentProviderFactory;

        public OrderService(OrdersRepository repository, IMapper mapper, IPaymentProviderFactory paymentProviderFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _paymentProviderFactory = paymentProviderFactory;
        }

        // GetAllOrdersAsync retrieves all Orders and maps them to a list of OrderDTO.
        // It returns an IEnumerable of OrderDTO.
        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<OrderDTO>>(orders);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }

        }

        // GetOrderByIdAsync retrieves a Order by its ID and maps it to a OrderDTO.
        // If the Order is not found, it returns null.
        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalid order ID.");
            try
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {id} not found.");
                }
                return _mapper.Map<OrderDTO>(order);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        // AddOrderAsync maps the OrderDTO to a Order entity and adds it to the repository.
        public async Task<OrderDTO> AddOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO == null)
                    throw new ValidationException("Order data is null.");
                // Validate the order data
                if (orderDTO.Products == null || !orderDTO.Products.Any())
                    throw new ValidationException("Order must have at least one product.");
                if (string.IsNullOrEmpty(orderDTO.PaymentMethod))
                    throw new ValidationException("Payment method is required.");
                                
                orderDTO.Status = "Pending"; // Default status for new orders
                var order = _mapper.Map<PaymentOrder>(orderDTO);
                IPaymentProvider paymentProvider = _paymentProviderFactory.CreatePaymentProvider(order); // Create the payment provider based on the order details
                PaymentOrderExternal orderExternal = await paymentProvider.CreatePaymentOrderAsync(order);
                order.ProviderName = paymentProvider.ProviderName; 
                order.OrderIdProvider = orderExternal.OrderId; 
                order.Status = orderExternal.Status;
                order.ProviderRef = JsonSerializer.Serialize(orderExternal); // Store the response from the payment provider.
                // Add the order to the repository
                order = await _repository.AddAsync(order);
                orderDTO = _mapper.Map<OrderDTO>(order);
                // Return the created order DTO
                return orderDTO;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        // UpdateOrderAsync updates an existing Order in the repository.
        public async Task UpdateOrderAsync(OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO == null)
                    throw new ValidationException("Order data is null.");
                // Validate the order ID
                if (orderDTO.Id <= 0)
                    throw new ValidationException("Order ID must be greater than zero.");

                // Validate the order data
                if (orderDTO.Products == null || !orderDTO.Products.Any())
                    throw new ValidationException("Order must have at least one product.");
                if (string.IsNullOrEmpty(orderDTO.PaymentMethod))
                    throw new ValidationException("Payment method is required.");
                orderDTO.Status = "Pending"; // Default status for updated orders
                // Map the OrderDTO to a Order entity
                var order = _mapper.Map<PaymentOrder>(orderDTO);
                // Update the existing order in the repository
                await _repository.UpdateAsync(order);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        // DeleteOrderAsync deletes a Order by its ID from the repository.
        // It does not return any value.
        public async Task DeleteOrderAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<OrderDTO> CancelOrderAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalid order ID.");
            try
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {id} not found.");
                }
                // Logic to cancel the order
                IPaymentProvider paymentProvider = _paymentProviderFactory.CreatePaymentProvider(order.ProviderName); // Create the payment provider based on the name
                HttpResponseMessage msj = await paymentProvider.CancelPaymentOrderAsync(order.OrderIdProvider);
                if (!msj.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to cancel payment order with ID {id}. Status code: {msj.StatusCode}");
                }
                order.Status = "Cancelled"; // Update the status to Cancelled
                await _repository.UpdateAsync(order); // Save the changes
                return _mapper.Map<OrderDTO>(order);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        public async Task<OrderDTO> PayOrderAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalid order ID.");
            try
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {id} not found.");
                }
                // Logic to process payment for the order
                IPaymentProvider paymentProvider = _paymentProviderFactory.CreatePaymentProvider(order.ProviderName); // Create the payment provider based on the name
                HttpResponseMessage msj = await paymentProvider.PayPaymentOrderAsync(order.OrderIdProvider);
                if (!msj.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to cancel payment order with ID {id}. Status code: {msj.StatusCode}");
                }
                order.Status = "Paid"; // Update the status to Paid
                                       // Process the payment in external api before updating the order
                await _repository.UpdateAsync(order); // Save the changes
                return _mapper.Map<OrderDTO>(order);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }
    }
}