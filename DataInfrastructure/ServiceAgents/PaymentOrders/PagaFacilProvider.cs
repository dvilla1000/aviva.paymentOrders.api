using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders
{
    // PaymentProvider is a service agent that interacts with external payment systems.
    public class PagaFacilProvider : PaymentProvider
    {
        public override string ProviderName => "PagaFacil";
        public PagaFacilProvider(HttpClient httpClient, ILogger<PagaFacilProvider> logger)
        : base(httpClient, logger)
        {
        }
    }
}