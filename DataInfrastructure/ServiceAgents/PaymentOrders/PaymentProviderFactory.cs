using System.ComponentModel.DataAnnotations;
using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;


namespace Aviva.PaymentOrders.DataInfrastructure.ServiceAgents.PaymentOrders
{
    // PaymentProviderCreator is a factory class that creates instances of PaymentProvider.
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // CreatePaymentProvider returns an instance of IPaymentProvider.
        public IPaymentProvider CreatePaymentProvider(PaymentOrder order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Payment order cannot be null.");
            }
            // Validate the order before creating the payment provider
            if (string.IsNullOrEmpty(order.PaymentMethod))
            {
                throw new ValidationException("Payment method is not specified in the order.");
            }
            if (order.Amount <= 0)
            {
                throw new ValidationException("Order amount must be greater than zero.");
            }
            try
            {
                // Here you can add any additional validation logic if needed.
                // For example, checking if the order ID is valid or if the customer exists.
                switch (order.PaymentMethod.ToLower())
                {
                    case "cash":
                        return _serviceProvider.GetRequiredService<PagaFacilProvider>();
                    case "card":
                        {
                            // double commision = 0;
                            // // Logic to create a card payment provider
                            // if (order.Amount >= 0 && order.Amount <= 1500) //Monto entre 0 y 1500 → 2%
                            //     commision = order.Amount * 0.02;
                            // else if (order.Amount > 1500 && order.Amount <= 5000) //Monto mayor a 1500 y hasta 5000 → 1.5%
                            //     commision = order.Amount * 0.015;
                            // else if (order.Amount > 5000) //Monto mayor a 5000 → 0.5%
                            //     commision = order.Amount * 0.005;
                            // if (commision < order.Amount * 0.01) //
                            //     return _serviceProvider.GetRequiredService<PagaFacilProvider>();//CazaPagos Provider
                            // else
                            //     return _serviceProvider.GetRequiredService<PagaFacilProvider>();//PagaFacil Provider

                            if (order.Amount > 5000) // Logic to create a payment provider for amounts greater than 5000 (rate 0.5% commission)
                                return _serviceProvider.GetRequiredService<CazaPagosProvider>();// CazaPagos Provider
                            else // Logic to create a payment provider for amounts less than or equal to 5000  // (rate 1% commission)
                                return _serviceProvider.GetRequiredService<PagaFacilProvider>();// PagaFacil Provider
                        }
                    case "transfer":
                        // Logic to create a transfer payment provider
                        return _serviceProvider.GetRequiredService<CazaPagosProvider>();
                    case "crypto":
                        // Logic to create a crypto payment provider
                        // return _serviceProvider.GetRequiredService<CryptoProvider>();
                        throw new NotSupportedException($"Payment method '{order.PaymentMethod}' is not supported.");
                    default:
                        throw new NotSupportedException($"Payment method '{order.PaymentMethod}' is not supported.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating payment provider: {ex.Message}");
            }
        }


        public IPaymentProvider CreatePaymentProvider(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                throw new ArgumentNullException(nameof(providerName), "Payment method cannot be null or empty.");
            }
            try
            {
                switch (providerName.ToLower())
                {
                    case "pagafacil":
                        return _serviceProvider.GetRequiredService<PagaFacilProvider>();
                    case "cazapagos":
                        return _serviceProvider.GetRequiredService<CazaPagosProvider>();
                    default:
                        throw new NotSupportedException($"Payment method '{providerName}' is not supported.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating payment provider: {ex.Message}");
            }
        }
    }
 }