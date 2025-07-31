using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders
{
    // PaymentProvider is a service agent that interacts with external payment systems.
    public class PagaFacilProvider : PaymentProvider
    {
        public PagaFacilProvider(HttpClient httpClient, ILogger<PagaFacilProvider> logger) 
        : base(httpClient, logger)
        {
        }

        // private readonly HttpClient _httpClient;
        // private readonly ILogger<IPaymentProvider> _logger;

        // public PagaFacilProvider(HttpClient httpClient, ILogger<IPaymentProvider> logger)
        // {
        //     _httpClient = httpClient;
        //     _logger = logger;
        // }

        // Implementation of the methods defined in IPaymentProvider interface
        public override string ProviderName => "PagaFacil";

        /*        
                public async Task<PaymentOrderExternal> CreatePaymentOrderAsync(PaymentOrder paymentOrder)
                {
                    try
                    {
                        // Logic to create a payment order using PagaFacil API
                        // Ensure the base address is set correctly before making the request
                        _httpClient.BaseAddress = _httpClient.BaseAddress ?? new Uri("https://app-paga-chg-aviva.azurewebsites.net");
                        var response = await _httpClient.PostAsJsonAsync($"/order", MapToOrderModel(paymentOrder));

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

                public async Task<PaymentOrderExternal> GetPaymentOrderByIdAsync(string id)
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

                public async Task<IEnumerable<PaymentOrderExternal>> GetAllPaymentOrdersAsync()
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

                public async Task<PaymentOrderExternal> CancelPaymentOrderAsync(string id)
                {
                    // Logic to cancel a payment order
                    try
                    {
                        var response = await _httpClient.PutAsync($"/cancel/{id}",null);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Failed to cancel payment order with ID {id}. Status code: {response.StatusCode}");
                        }
                        return await response.Content.ReadFromJsonAsync<PaymentOrderExternal>();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error cancelling payment order with ID {Id}", id);
                        throw new Exception($"Internal server error: {ex.Message}");
                    }
                }

                public async Task<PaymentOrderExternal> PayPaymentOrderAsync(string id)
                {
                    // Logic to pay a payment order
                    try
                    {
                        var response = await _httpClient.PutAsync($"/pay?id={id}", null);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Failed to pay payment order with ID {id}. Status code: {response.StatusCode}");
                        }
                        return await response.Content.ReadFromJsonAsync<PaymentOrderExternal>();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error paying payment order with ID {Id}", id);
                        throw new Exception($"Internal server error: {ex.Message}");
                    }
                }
        */
        /*
        private OrderModel MapToOrderModel(PaymentOrder product)
        {
            return new OrderModel
            {
                Method = product.PaymentMethod,
                Products = MapToOrderProductModels(product.PaymentOrderDetails)
            };
        }

        private ICollection<OrderProductModel> MapToOrderProductModels(ICollection<PaymentOrderDetail> paymentOrderDetails)
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
        */
    }
}