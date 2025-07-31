using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Runtime.InteropServices.Marshalling;


namespace Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders
{

    // PagaFacilProvider is a service agent that interacts with the PagaFacil payment system.
    public abstract class PaymentProvider : IPaymentProvider
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger<PagaFacilProvider> _logger;

        public PaymentProvider(HttpClient httpClient, ILogger<PagaFacilProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Implementation of the methods defined in IPaymentProvider interface

        public virtual string ProviderName => "GenericProvider";

        public virtual async Task<PaymentOrderExternal> CreatePaymentOrderAsync(PaymentOrder paymentOrder)
        {
            try
            {
                // Logic to create a payment order using PagaFacil API
                // Ensure the base address is set correctly before making the request
                _httpClient.BaseAddress = _httpClient.BaseAddress ?? new Uri("https://app-caza-chg-aviva.azurewebsites.net");
                var response = await _httpClient.PostAsJsonAsync("/order", MapToOrderModel(paymentOrder));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Payment failed for Order ID {OrderId}", paymentOrder.Id);
                    throw new Exception($"Payment failed with status code: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<PaymentOrderExternal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment order for PaymentOrder ID {PaymentOrderId}", paymentOrder.Id);
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        public virtual async Task<PaymentOrderExternal> GetPaymentOrderByIdAsync(string id)
        {
            try
            {
                // Logic to get a payment order by ID
                var response = await _httpClient.GetAsync($"/order/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve payment order with ID {id}. Status code: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<PaymentOrderExternal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment order with ID {Id}", id);
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<PaymentOrderExternal>> GetAllPaymentOrdersAsync()
        {
            // Logic to get all payment orders
            try
            {
                var response = await _httpClient.GetAsync("/order");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve payment orders. Status code: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<PaymentOrderExternal>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payment orders");
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        public virtual async Task<HttpResponseMessage> CancelPaymentOrderAsync(string id)
        {
            // Logic to cancel a payment order
            try
            {
                var response = await _httpClient.PutAsync($"/cancel?id={id}", null);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to cancel payment order with ID {id}. Status code: {response.StatusCode}");
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment order with ID {Id}", id);
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        public virtual async Task<HttpResponseMessage> PayPaymentOrderAsync(string id)
        {
            // Logic to pay a payment order
            try
            {
                var response = await _httpClient.PutAsync($"/pay?id={id}", null);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to pay payment order with ID {id}. Status code: {response.StatusCode}");
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error paying payment order with ID {Id}", id);
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        protected virtual OrderModel MapToOrderModel(PaymentOrder product)
        {
            return new OrderModel
            {
                Method = product.PaymentMethod,
                Products = MapToOrderProductModels(product.PaymentOrderDetails)
            };
        }        

        public virtual ICollection<OrderProductModel> MapToOrderProductModels(ICollection<PaymentOrderDetail> paymentOrderDetails)
        {
            var orderProductModels = new List<OrderProductModel>();
            foreach (var detail in paymentOrderDetails)
            {
                orderProductModels.Add(new OrderProductModel
                {
                    Name = detail.ProductName,
                    UnitPrice = detail.UnitPrice
                });
            }
            return orderProductModels;
        }
    }

    public class OrderModel
    {
        public string Method { get; set; }
        public ICollection<OrderProductModel> Products { get; set; }
    }

    public class OrderProductModel
    {
        public string Name { get; set; }
        public double UnitPrice { get; set; }
    }


}