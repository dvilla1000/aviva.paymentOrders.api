using System.ComponentModel.DataAnnotations.Schema;

namespace Aviva.PaymentOrders.Domain.Entities
{
    public class PaymentOrder : Entity
    {
        public string PaymentMethod { get; set; }
        public double Amount { get; set; } // Amount of the payment order
        public string Status { get; set; } // Status of the order, e.g., "Pending", "Completed", "Cancelled"
        public string OrderIdProvider { get; set; } // OrderId from Provider API
        public string ProviderRef { get; set; } // // Response from the payment provider
        public string ProviderName { get; set; } // Name of the payment provider
        public ICollection<PaymentOrderDetail> PaymentOrderDetails { get; set; } // Navigation property to PaymentOrderDetail entity
    }
}