using Aviva.PaymentOrders.Domain.Entities;

namespace Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders
{
    // IPaymentProviderFactory is an interface that defines a factory for creating payment providers.
    // It allows for the creation of different payment provider instances based on the PaymentOrder.
    public interface IPaymentProviderFactory
    {
        IPaymentProvider CreatePaymentProvider(PaymentOrder order);
        IPaymentProvider CreatePaymentProvider(string providerName);
    }
}