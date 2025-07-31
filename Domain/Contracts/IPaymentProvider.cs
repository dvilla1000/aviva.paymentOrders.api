using Aviva.PaymentOrders.Domain.Entities;

public interface IPaymentProvider
{
    string ProviderName { get; } // Name of the payment provider
    Task<PaymentOrderExternal> CreatePaymentOrderAsync(PaymentOrder paymentOrder);
    Task<PaymentOrderExternal> GetPaymentOrderByIdAsync(string id);
    Task<IEnumerable<PaymentOrderExternal>> GetAllPaymentOrdersAsync();
    Task<HttpResponseMessage> CancelPaymentOrderAsync(string id);
    Task<HttpResponseMessage> PayPaymentOrderAsync(string id);
}