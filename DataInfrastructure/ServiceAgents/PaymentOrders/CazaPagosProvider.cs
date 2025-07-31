using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders
{

    // PagaFacilProvider is a service agent that interacts with the PagaFacil payment system.
    public class CazaPagosProvider : PaymentProvider
    {
        public override string ProviderName => "CazaPagos";
        public CazaPagosProvider(HttpClient httpClient, ILogger<PagaFacilProvider> logger)
        : base(httpClient, logger)
        {
        }

        public override async Task<HttpResponseMessage> CancelPaymentOrderAsync(string id)
        {
            // Logic to cancel a payment order
            try
            {
                var response = await _httpClient.PutAsync($"/cancellation?id={id}", null);
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

        public override async Task<HttpResponseMessage> PayPaymentOrderAsync(string id)
        {
            // Logic to pay a payment order
            try
            {
                var response = await _httpClient.PutAsync($"/payment?id={id}", null);
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

        protected override OrderModel MapToOrderModel(PaymentOrder product)
        {
            string method = product.PaymentMethod ?? "none";
            if (method.ToLower() == "card")
                method = "CreditCard";
            return new OrderModel
            {
                Method = method,
                Products = MapToOrderProductModels(product.PaymentOrderDetails)
            };
        }

    }
}